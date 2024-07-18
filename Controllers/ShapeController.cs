using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserIdentity.Identity;

namespace UserIdentity.Controllers
{
    public class ShapeController : Controller
    {
        public ShapeController() { }
        public ActionResult Index() {  return View(); }






        UserIdentity.Identity.IdentityDataContext db = new UserIdentity.Identity.IdentityDataContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = db.InputModels;
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] UserIdentity.Models.InputModel item)
        {
            var model = db.InputModels;
            if (ModelState.IsValid)
            {
                try
                {
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
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] UserIdentity.Models.InputModel item)
        {
            var model = db.InputModels;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 Id)
        {
            var model = db.InputModels;
            if (Id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id == Id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }
    }
}