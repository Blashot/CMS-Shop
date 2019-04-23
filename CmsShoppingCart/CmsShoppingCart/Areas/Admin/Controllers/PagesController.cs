using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // declare list of PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                // init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //return view with list
            return View(pagesList);
        }


        //GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }


        //POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {

            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {


                // Declare slug
                string slug;
                // init pageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;
                // check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                // make sure title and slug are unique
                if (db.Pages.Any(x=> x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "that title or slug already exist.");
                    return View(model);
                }
                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                // save dto
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //set tempdata message
            TempData["SM"] = "You have added a new Page!";


            //redirect
            return RedirectToAction("AddPage");

            return View();
        }
    }
}