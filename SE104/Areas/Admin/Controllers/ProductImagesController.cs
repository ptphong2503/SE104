using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using SE104.Models;

namespace SE104.Areas.Admin.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly Se104Context _context;

        public ProductImagesController(Se104Context context)
        {
            _context = context;
        }

        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = _context.TbProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
        {
            _context.TbProductImages.Add(new TbProductImage
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            _context.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _context.TbProductImages.Find(id);
            _context.TbProductImages.Remove(item);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}
