using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microlab.web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microlab.web.Data;
using System.Linq;

public class LoginPartialViewComponent : ViewComponent
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public LoginPartialViewComponent(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        string clinicaId = null;

        if (User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Busca a clínica do usuário logado
            var clinica = await _context.Clinicas
                .Where(c => c.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (clinica != null)
            {
                clinicaId = clinica.ClinicaId.ToString(); // Converte para string se necessário
            }
        }

        // Define o ViewBag para a view do componente
        ViewBag.ClinicaID = clinicaId;

        return View(); // Views/Shared/Components/LoginPartial/Default.cshtml
    }
}
