using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected RedirectToActionResult Redirecionar(string action, string? controller = null, object? routeValues = null) => RedirectToAction(action, controller, routeValues);
    }
}
