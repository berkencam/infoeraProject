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

namespace UserIdentity.Controllers
{
    public class ShapeController : Controller
    {
        public ActionResult Index() { return View(); }
        public ShapeController() { }

        IdentityDataContext db = new IdentityDataContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {


            var model = db.InputModels;
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
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.InputModel item)
        {
            var model = db.InputModels;
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
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
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
            return PartialView("~/Views/Shape/_GridViewPartial.cshtml", model.ToList());
        }

        dal.Identity.IdentityDataContext db1 = new dal.Identity.IdentityDataContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartialResults()
        {
            var model = db1.ResultModels;
            return PartialView("~/Views/Shape/_GridViewPartialResults.cshtml", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialResultsAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.ResultModel item)
        {
            var model = db1.ResultModels;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Shape/_GridViewPartialResults.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialResultsUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] dal.Models.ResultModel item)
        {
            var model = db1.ResultModels;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db1.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Shape/_GridViewPartialResults.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialResultsDelete(System.Int32 Id)
        {
            var model = db1.ResultModels;
            if (Id >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id == Id);
                    if (item != null)
                        model.Remove(item);
                    db1.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Shape/_GridViewPartialResults.cshtml", model.ToList());
        }
        private readonly string _firstApiUrl = ConfigurationManager.AppSettings["FirstApiUrl"];


    }
}