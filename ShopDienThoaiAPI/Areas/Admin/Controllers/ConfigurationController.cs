using Models.DAO;
using Models.EF;
using ShopDienThoaiAPI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ShopDienThoaiAPI.Areas.Admin.Controllers
{
    public class ConfigurationController : Controller
    {
        [HttpPost]
        public ActionResult CreateConfigurationPartial()
        {
            return PartialView("CreateConfigurationPartial");
        }

        [HttpPost]
        public async Task<JsonResult> CreateConfiguration(CONFIGURATION model)
        {
            if (ModelState.IsValid)
            {
                var config = new CONFIGURATION
                {
                    ProductID = model.ProductID,
                    OSName = model.OSName,
                    OSVersion = model.OSVersion,
                    SizeDisplay = model.SizeDisplay,
                    FrontCamera = model.FrontCamera,
                    RearCamera = model.RearCamera,
                    Cpu = model.Cpu,
                    Ram = model.Ram,
                    Rom = model.Rom,
                    SimStatus = model.SimStatus,
                    Battery = model.Battery
                };
                int result = await new ConfigurationDAO().CreateConfiguration(config);
                if (result == 0)
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);

                return Json(new { Success = true, id = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}