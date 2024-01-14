using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SE104.Models;
using SE104.ModelsView;

namespace SE104.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Se104Context _context;

        public ProductsController(Se104Context context)
        {
            _context = context;
        }


        
        [HttpGet]
        public IActionResult search(int? page, int id, string query)
        {
            var lsProducts = _context.TbProducts.AsNoTracking().OrderByDescending(x => x.CreatedDate);
            var categories = _context.TbProductCategories.Where(x => x.Levels == 2).OrderBy(x => Guid.NewGuid()).Take(4);

            var popProducts = _context.TbProducts.AsNoTracking().Where(x => x.BestSellers == true).OrderBy(x => Guid.NewGuid()).Take(3);

            if (query != null)
            {
                lsProducts = _context.TbProducts.AsNoTracking()
                            .Where(x => x.ProductName.Contains(query))
                            .OrderByDescending(x => x.CreatedDate);
            }
            HttpContext.Session.SetString("query", query);

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 12;
            PagedList<TbProduct> models = new PagedList<TbProduct>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.categories = categories;
            ViewBag.popProducts = popProducts;

            return View(models);
        }

        [Route("danhmuc/{AliasCat}-{CatId}", Name = ("ListProduct"))]
        public IActionResult Index(string AliasCat, int? page, int id)
        {
            var category = _context.TbProductCategories.AsNoTracking().SingleOrDefault(x => x.Alias == AliasCat);
            /*var lsProducts = _context.TbProducts.AsNoTracking().Where(x=>x.BestSellers == true).OrderBy(x => Guid.NewGuid()).Take(3);*/
            var lsProducts = _context.TbProducts.AsNoTracking()
                        .Where(x => x.CatId == category.CatId)
                        .OrderByDescending(x => x.CreatedDate);
            var categories = _context.TbProductCategories.Where(x => x.ParentId == category.ParentId).Take(4);


            if (category.Levels==1)
            {
                lsProducts = _context.TbProducts.AsNoTracking()
                        .Where(x => x.Cat.ParentId == category.CatId)
                        .OrderByDescending(x => x.CreatedDate);

                categories = _context.TbProductCategories.Where(x => x.ParentId == category.CatId).Take(4);
            }
            
            

            HttpContext.Session.SetString("AliasCat", AliasCat);

            var popProducts = _context.TbProducts.AsNoTracking().Where(x => x.BestSellers == true).OrderBy(x => Guid.NewGuid()).Take(3);


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 12;
            PagedList<TbProduct> models = new PagedList<TbProduct>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.category = category;
            ViewBag.categories = categories;
            ViewBag.popProducts = popProducts;

            return View(models);
        }

        public IActionResult SortProduct(int Sort, string query)
        {
            var AliasCat = HttpContext.Session.GetString("AliasCat");
            query = HttpContext.Session.GetString("query");

            ProductsView model = new ProductsView();

            var category = _context.TbProductCategories.AsNoTracking().SingleOrDefault(x => x.Alias == AliasCat);
            var lsProducts = _context.TbProducts.Where(x => x.CatId == category.CatId).AsNoTracking().OrderByDescending(x => x.CreatedDate);

               

            if (query != null)
            {
                if (Sort == 1)
                {
                    lsProducts = _context.TbProducts.Where(x => x.ProductName.Contains(query))
                        .OrderBy(x => x.Price);
                }
                else if (Sort == 2)
                {

                    lsProducts = _context.TbProducts.Where(x => x.ProductName.Contains(query))
                        .OrderByDescending(x => x.Price);
                }
                else
                {
                    lsProducts = _context.TbProducts.Where(x => x.ProductName.Contains(query))
                        .OrderByDescending(x => x.CreatedDate);
                }
            }
            else
            {
                if (category.Levels == 1)
                {
                    lsProducts = _context.TbProducts.Where(x => x.Cat.ParentId == category.CatId).AsNoTracking().OrderByDescending(x => x.CreatedDate);
                }

                if (Sort == 1)
                {
                    lsProducts = _context.TbProducts.Where(x=> x.CatId == category.CatId).OrderBy(x => x.Price);
                }
                else if (Sort == 2)
                {

                    lsProducts = _context.TbProducts.Where(x => x.CatId == category.CatId).OrderByDescending(x => x.Price);
                }
                else
                {
                    lsProducts = _context.TbProducts.Where(x => x.CatId == category.CatId).OrderByDescending(x => x.CreatedDate);
                }
            }
            var models = lsProducts.ToList();

            

            return PartialView("SortProduct", models);

        }




        [Route("San-pham/{Alias}/{id}", Name = ("ProductDetails"))]
        public IActionResult Details(int id, int SizeId, int ColorId)
        {
            try
            {
                var lsImage = _context.TbProductImages.Where(x => x.ProductId == id).ToList();
                ViewBag.lsImage = lsImage;

                var lsSize = _context.TbAttributesPrices
                            .Where(x => x.ProductId == id)
                            .Select(x => x.SizeId)
                            .ToList();

                var lsSizeID = _context.TbSizes
                                .Where(x => lsSize.Contains(x.SizeId))
                                .ToList();

                ViewData["lsSize"] = new SelectList(lsSizeID, "SizeId", "SizeValue", SizeId);

                var lsColor = _context.TbAttributesPrices
                                .Where(x => x.ProductId == id)
                                .Select(x => x.ColorId)
                                .ToList();

                var lsColorID = _context.TbColors.Where(x => lsColor.Contains(x.ColorId)).ToList();

                ViewData["lsColor"] = new SelectList(lsColorID, "ColorId", "ColorName", ColorId);


                var product = _context.TbProducts.Include(x => x.Cat).FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    return RedirectToAction("Index");
                }
                var lsProduct = _context.TbProducts
                    .AsNoTracking()
                    .Where(x => x.CatId == product.CatId && x.ProductId != id && x.Active == true)
                    .Take(6)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();
                ViewBag.SanPham = lsProduct;
                return View(product);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }


        }




    }
}
