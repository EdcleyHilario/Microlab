using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microlab.web.Data;
using System.Linq;
using System.Threading.Tasks;

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
        bool isAdmin = false;

        if (User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            // Busca a clínica do usuário logado
            var clinica = await _context.Clinicas
                .Where(c => c.UsuarioId == userId)
                .FirstOrDefaultAsync();

            if (clinica != null)
            {
                clinicaId = clinica.ClinicaId.ToString();
            }

            // Verifica se o usuário tem role "Admin"
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            }
        }

        // Passa os dados para a view do componente
        ViewBag.ClinicaID = clinicaId;
        ViewBag.IsAdmin = isAdmin;

        return View(); // Views/Shared/Components/LoginPartial/Default.cshtml
    }
}
