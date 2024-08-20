using dal.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web.Mvc;

namespace UserIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;

        public RoleAdminController()
        {
            var roleStore = new RoleStore<IdentityRole>(new IdentityDataContext());
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        // GET: RoleAdmin
        public ActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                if (!roleManager.RoleExists(roleName))
                {
                    var role = new IdentityRole(roleName);
                    roleManager.Create(role);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Role already exists");
                }
            }

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string roleId)
        {
            var role = roleManager.FindById(roleId);
            if (role != null)
            {
                roleManager.Delete(role);
            }

            return RedirectToAction("Index");
        }
    }
}
