using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dal.Identity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNet.Identity;
using dal.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using UserIdentity.Attributes;

namespace UserIdentity.Controllers
{
    
    public class ShapeController : MyController
    {
        
        public ActionResult Index() { return View(); }
        public ShapeController() { }

        IdentityDataContext db = new IdentityDataContext();
        private readonly UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var userId = new Guid(User.Identity.GetUserId());
            var roles = UserManager.GetRoles(User.Identity.GetUserId());

            IQueryable<InputModel> model;

            if (roles.Contains("Hesaplayici") || roles.Contains("Girdici"))
            {
                // Hesaplayici veya Girdici rolünde olan kullanıcılar için filtreleme
                var hesaplayiciGirdiciUsers = db.Users.ToList()
                    .Where(u => UserManager.IsInRole(u.Id, "Hesaplayici") || UserManager.IsInRole(u.Id, "Girdici"))
                    .Select(u => u.Id)
                    .ToList();

                model = db.InputModels
                    .Where(i => hesaplayiciGirdiciUsers.Contains(i.UserId.ToString()));
            }
            else
            {
                // Diğer kullanıcılar sadece kendi kayıtlarını görsün
                model = db.InputModels.Where(i => i.UserId == userId);
            }

            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }



        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.InputModel item)
        {
            var model = db.InputModels;
            if (ModelState.IsValid)
            {
                try
                {
                    item.HesaplandiMi = false;
                    item.CreatedDate = DateTime.Now;
                    item.UserId = new Guid(User.Identity.GetUserId());
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var userId = new Guid(User.Identity.GetUserId());
            var model2 = db.InputModels.Where(i => i.UserId == userId);
            return GridViewPartial();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.InputModel item)
        {
            var model = db.InputModels;
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
                    if (modelItem != null)
                    {
                        modelItem.ShapeType = item.ShapeType;
                        modelItem.Yukseklik = item.Yukseklik;
                        modelItem.Yaricap = item.Yaricap;
                        modelItem.Kenar = item.Kenar;
                        modelItem.KısaKenar = item.KısaKenar;
                        modelItem.UzunKenar = item.UzunKenar;
                        modelItem.HesaplandiMi = item.HesaplandiMi;
                        modelItem.CreatedDate = DateTime.Now;

                        db.SaveChanges();
                        isSuccess = true;
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = "Please, correct all errors.";
            }
            return Json(new { success = isSuccess });
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.InputModel item)
        {
            var model = db.InputModels;
            if (item.Id >= 0)
            {
                try
                {
                    var item1 = model.FirstOrDefault(it => it.Id == item.Id);
                    if (item1 != null)
                        model.Remove(item1);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            var userId = new Guid(User.Identity.GetUserId());
            var model2 = db.InputModels.Where(i => i.UserId == userId);
            return GridViewPartial();
        }


        dal.Identity.IdentityDataContext db1 = new dal.Identity.IdentityDataContext();

       
        [ValidateInput(false)]
        public ActionResult GridViewPartialResults()
        {
            var userId = new Guid(User.Identity.GetUserId());
            var roles = UserManager.GetRoles(User.Identity.GetUserId());

            IQueryable<ResultModel> model;

            if (roles.Contains("Hesaplayici") || roles.Contains("Girdici"))
            {
                // Hesaplayici veya Girdici rolünde olan kullanıcılar için filtreleme
                var hesaplayiciGirdiciUsers = db.Users.ToList()
                    .Where(u => UserManager.IsInRole(u.Id, "Hesaplayici") || UserManager.IsInRole(u.Id, "Girdici"))
                    .Select(u => u.Id)
                    .ToList();

                model = db.ResultModels
                    .Where(i => hesaplayiciGirdiciUsers.Contains(i.UserId.ToString()));
            }
            else
            {
                // Diğer kullanıcılar sadece kendi kayıtlarını görsün
                model = db.ResultModels.Where(i => i.UserId == userId);
            }

            return PartialView("~/Views/Shape/_GridViewPartialResults.cshtml", model.ToList());
        }

        private static readonly HttpClient httpClient = new HttpClient();
#if DEBUG
        string _targetApiUrl = ConfigurationManager.AppSettings["FirstApiUrlLocal"];
#else
  string _targetApiUrl = ConfigurationManager.AppSettings["FirstApiUrlLive"];
#endif

        [Unauthorize("Girdici,Member")]
        public async Task<ActionResult> CalculateAndFetchResults()
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(_targetApiUrl, null);

                if (response.IsSuccessStatusCode)
                {
                    
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = response.ReasonPhrase });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


    }
}