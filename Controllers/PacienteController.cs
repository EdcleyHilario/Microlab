using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using X.PagedList.Extensions;
namespace Microlab.web.Controllers;

using Microlab.web.Data;
using Microlab.web.Models;
using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class PacienteController : Controller
    {
        private readonly ApplicationDbContext db;

        public PacienteController(ApplicationDbContext context)
        {
            db = context;
        }

        // GET: Pacientes da clínica
        [HttpGet]
        public async Task<IActionResult> Index(Guid clinicaId, string searchTerm, int? page, int pageSize = 10)
        {
            // Busca a clínica
            var clinica = await db.Clinicas
                .FirstOrDefaultAsync(c => c.ClinicaId == clinicaId);

            if (clinica == null)
                return NotFound();

            // Base de pacientes
            var pacientesQuery = db.Pacientes
                .Where(p => p.ClinicaId == clinicaId);

            // Aplicar filtro de busca
            if (!string.IsNullOrEmpty(searchTerm))
            {
                pacientesQuery = pacientesQuery.Where(p =>
                    Convert.ToString(p.Protocolo).Contains(searchTerm) ||
                    p.Nome.Contains(searchTerm) ||
                    p.Cpf.Contains(searchTerm));
            }

            int pageNumber = page ?? 1;

        // Projeção para ViewModel com contagem de exames
        var pacientesList = await pacientesQuery
            .OrderBy(p => p.Nome)
            .Select(p => new PacienteItemViewModel
            {
                PacienteId = p.PacienteId,
                Protocolo = p.Protocolo,
                Nome = p.Nome,
                Idade = p.Idade,
                DataNascimento = p.DataNascimento,
                Cpf = p.Cpf,
                Rg = p.Rg,
                QtdExames = db.Exames.Count(e => e.PacienteId == p.PacienteId)
            })
            .ToListAsync(); // pega todos os registros da query

                    var pacientesVm = pacientesList.ToPagedList(pageNumber, pageSize);


        var vm = new ClinicaPacientesViewModel
            {
                Clinica = clinica,
                Pacientes = pacientesVm,
                SearchTerm = searchTerm,
                PageSize = pageSize
            };

            return View(vm);
        }


        // GET: Adicionar paciente
        [HttpGet]
        public async Task<IActionResult> AddAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Buscar a clínica vinculada ao usuário logado
            var clinica = await db.Clinicas.FirstOrDefaultAsync(x => x.UsuarioId == userId);
            if (clinica == null)
            {
                // Redireciona para a tela de cadastro de clínica se não existir
                return RedirectToAction("Create", "Clinicas");
            }

            // Trazer os últimos 10 pacientes da clínica
            var ultimos10Pacientes = await db.Pacientes
                .Where(p => p.ClinicaId == clinica.ClinicaId)
                .OrderByDescending(p => p.DataSolicitacao)
                .Take(10)
                .ToListAsync();

            // Preencher o ViewModel com o nome da clínica
            var viewModel = new AddPacienteViewModel
            {
                ClinicaId = clinica.ClinicaId,
                ClinicaNome = clinica.Nome, 
                Pacientes = ultimos10Pacientes
            };

            return View(viewModel);
        }


        // POST: Adicionar paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddPacienteViewModel viewModel)
        {
            

            // Garantir que paciente será vinculado à clínica correta
            var clinica = await db.Clinicas.FindAsync(viewModel.ClinicaId);
            if (clinica == null)
            {
                TempData["ErrorMessage"] = "Erro ao cadastrar paciente. Verifique os dados.";
                return RedirectToAction("Create", "Clinicas");
            }

            var paciente = new Paciente
            {
                PacienteId = Guid.NewGuid(),
                Nome = viewModel.Nome,
                Cpf = viewModel.Cpf,
                Rg = viewModel.Rg,
                Idade = viewModel.Idade,
                DataNascimento = viewModel.DataNascimento,
                DataSolicitacao = viewModel.DataSolicitacao,
                Exame = viewModel.Exame,
                Material = viewModel.Material,
                Protocolo = viewModel.Protocolo,
                Solicitante = clinica.Nome,
                ClinicaId = clinica.ClinicaId
            };
            var exame = new Exame
            {
                ExameId = Guid.NewGuid(),
                NmExame = viewModel.Exame,
                Material = viewModel.Material,
                Metodo = "Exame Físico-Químico / Microscopia",
                Digitador = "Sem digitador",
                Status = "Sem Resultado",
                Observacao = "Nenhuma",
                PacienteId = paciente.PacienteId
            };
            await db.Pacientes.AddAsync(paciente);
            await db.Exames.AddAsync(exame);
            await db.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Paciente cadastrado com sucesso!";
            return RedirectToAction("Add", new { clinicaId = clinica.ClinicaId });
        }

        // GET: Editar paciente
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var paciente = await db.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound();
            return View(paciente);
        }

        // POST: Editar paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Paciente viewModel)
        {
            var paciente = await db.Pacientes.FindAsync(viewModel.PacienteId);
            if (paciente == null) return NotFound();

            // Atualiza campos
            paciente.Nome = viewModel.Nome;
            paciente.Cpf = viewModel.Cpf;
            paciente.Rg = viewModel.Rg;
            paciente.Idade = viewModel.Idade;
            paciente.DataNascimento = viewModel.DataNascimento;
            paciente.DataSolicitacao = viewModel.DataSolicitacao;
            paciente.Exame = viewModel.Exame;
            paciente.Material = viewModel.Material;
            paciente.Protocolo = viewModel.Protocolo;
            paciente.Solicitante = viewModel.Solicitante;
            
            await db.SaveChangesAsync();

            return RedirectToAction("Add", new { clinicaId = paciente.ClinicaId });
        }

        // POST: Deletar paciente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var paciente = await db.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                db.Pacientes.Remove(paciente);
                await db.SaveChangesAsync();
                return RedirectToAction("Add", new { clinicaId = paciente.ClinicaId });
            }

            return RedirectToAction("Add");
        }
    }
