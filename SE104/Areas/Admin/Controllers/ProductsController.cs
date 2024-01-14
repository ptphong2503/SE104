using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using SE104.Helpers;
using SE104.Models;

namespace SE104.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly Se104Context _context;

        public ProductsController(Se104Context context)
        {
            _context = context;
        }

        // GET: Admin/TbProducts
        public async Task<IActionResult> Index(int? page, string search, int CatID)
        {


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;

            List<TbProduct> lsProducts = new List<TbProduct>();
            ViewData["DanhMuc"] = new SelectList(_context.TbProductCategories, "CatId", "CatName", CatID);

            if (CatID != 0)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    lsProducts = _context.TbProducts
                        .AsNoTracking()
                        .Where(x => x.CatId == CatID && x.ProductName.Contains(search))
                        .OrderByDescending(x => x.ProductId).ToList();
                }
                else
                {
                    lsProducts = _context.TbProducts
                        .AsNoTracking()
                        .Where(x => x.CatId == CatID)
                        .OrderByDescending(x => x.ProductId).ToList();
                }
            }
            else
            {
                lsProducts = _context.TbProducts
                    .AsNoTracking()
                   .Include(x => x.Cat)
                   .OrderByDescending(x => x.ProductId).ToList();
            }


            PagedList<TbProduct> models = new PagedList<TbProduct>(lsProducts.AsQueryable(), pageNumber, pageSize);



            return await Task.FromResult(View(models));
        }

        // GET: Admin/TbProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbProducts == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.Cat)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            tbProduct.ProductImage = _context.TbProductImages.Where(x => x.ProductId == id).ToList();

            var Size = _context.TbSizes;
            ViewBag.Size = Size.ToList();

            var Color = _context.TbColors.ToList();
            ViewBag.Color = Color;

            var lsSize = _context.TbAttributesPrices.Where(x => x.ProductId == id).Select(x => x.SizeId).ToList();
            ViewBag.lsSize = lsSize;

            var lsColor = _context.TbAttributesPrices.Where(x => x.ProductId == id).Select(x => x.ColorId).ToList();
            ViewBag.lsColor = lsColor;

            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // GET: Admin/TbProducts/Create
        public IActionResult Create()
        {
            var Size = _context.TbSizes;
            ViewBag.Size = Size.ToList();

            var Color = _context.TbColors.ToList();
            ViewBag.Color = Color;

            ViewData["DanhMuc"] = new SelectList(_context.TbProductCategories, "CatId", "CatName");
            return View();
        }

        // POST: Admin/TbProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Image,CreatedDate,CreatedBy,DateModified,BestSellers,IsNew,Active,Tags,Alias,UnitsInStock,Title,SalePrice")] TbProduct tbProduct, List<string> Images, List<int> rDefault, List<int> Size, List<int> Color)
        {
            if (ModelState.IsValid)
            {
                tbProduct.Alias = Utilities.SEOUrl(tbProduct.ProductName);
                tbProduct.CreatedDate = DateTime.Now;
                _context.Add(tbProduct);
                await _context.SaveChangesAsync();

                if (Size != null)
                {
                    foreach (var size in Size)
                    {
                        if (Color != null)
                        {
                            foreach (var color in Color)
                            {
                                tbProduct.TbAttributesPrices.Add(new TbAttributesPrice
                                {

                                    ProductId = tbProduct.ProductId,
                                    SizeId = size,
                                    ColorId = color,
                                    Active = true

                                });
                            }

                        }
                        else
                        {
                            tbProduct.TbAttributesPrices.Add(new TbAttributesPrice
                            {
                                ProductId = tbProduct.ProductId,
                                SizeId = size,
                                Active = true
                            });
                        }

                    }
                }

                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {
                            tbProduct.Image = Images[i];
                            tbProduct.ProductImage.Add(new TbProductImage
                            {
                                ProductId = tbProduct.ProductId,
                                Image = Images[i],
                                IsDefault = true
                            });
                        }
                        else
                        {
                            tbProduct.ProductImage.Add(new TbProductImage
                            {
                                ProductId = tbProduct.ProductId,
                                Image = Images[i],
                                IsDefault = false
                            });
                        }
                    }
                }

                
                if (tbProduct.Price.HasValue && tbProduct.Discount.HasValue)
                {
                    tbProduct.SalePrice = Convert.ToInt32(tbProduct.Price.Value - (tbProduct.Price.Value * tbProduct.Discount.Value / 100));
                }
                else
                {
                    tbProduct.SalePrice = tbProduct.Price;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DanhMuc"] = new SelectList(_context.TbProductCategories, "CatId", "CatName", tbProduct.CatId);

            return View(tbProduct);
        }

        // GET: Admin/TbProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbProducts == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts.FindAsync(id);

            tbProduct.ProductImage = _context.TbProductImages.Where(x => x.ProductId == id).ToList();
            var img = tbProduct.ProductImage;

            var Size = _context.TbSizes;
            ViewBag.Size = Size.ToList();

            var Color = _context.TbColors.ToList();
            ViewBag.Color = Color;

            var lsSize = _context.TbAttributesPrices.Where(x => x.ProductId == id).Select(x => x.SizeId).ToList();
            ViewBag.lsSize = lsSize;

            var lsColor = _context.TbAttributesPrices.Where(x => x.ProductId == id).Select(x => x.ColorId).ToList();
            ViewBag.lsColor = lsColor;

            if (tbProduct == null)
            {
                return NotFound();
            }

            ViewData["CatId"] = new SelectList(_context.TbProductCategories, "CatId", "CatId", tbProduct.CatId);

            return View(tbProduct);
        }

        // POST: Admin/TbProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CatId,Price,Discount,Image,CreatedDate,CreatedBy,DateModified,BestSellers,IsNew,Active,Tags,Alias,UnitsInStock,Title,SalePrice")] TbProduct tbProduct, List<string> Images, List<int> rDefault, List<string> AddedImage, List<int> Size, List<int> Color)
        {
            if (id != tbProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var lsAttribute = _context.TbAttributesPrices.Where(x => x.ProductId == id);
                    foreach (var item in lsAttribute)
                    {
                        _context.TbAttributesPrices.Remove(item);
                    }

                    if (Size != null)
                    {
                        foreach (var size in Size)
                        {
                            if (Color != null)
                            {
                                foreach (var color in Color)
                                {
                                    tbProduct.TbAttributesPrices.Add(new TbAttributesPrice
                                    {

                                        ProductId = tbProduct.ProductId,
                                        SizeId = size,
                                        ColorId = color,
                                        Active = true

                                    });
                                }

                            }
                            else
                            {
                                tbProduct.TbAttributesPrices.Add(new TbAttributesPrice
                                {
                                    ProductId = tbProduct.ProductId,
                                    SizeId = size,
                                    Active = true
                                });
                            }

                        }
                    }


                    var lsImage = _context.TbProductImages.Where(x => x.ProductId == id);
                    foreach (var item in lsImage)
                    {
                        _context.TbProductImages.Remove(item);
                    }
                    if (Images != null && Images.Count > 0)
                    {
                        for (int i = 0; i < Images.Count; i++)
                        {
                            if (i + 1 == rDefault[0])
                            {
                                tbProduct.Image = Images[i];
                                tbProduct.ProductImage.Add(new TbProductImage
                                {
                                    ProductId = tbProduct.ProductId,
                                    Image = Images[i],
                                    IsDefault = true
                                });
                            }
                            else
                            {
                                tbProduct.ProductImage.Add(new TbProductImage
                                {
                                    ProductId = tbProduct.ProductId,
                                    Image = Images[i],
                                    IsDefault = false
                                });
                            }
                        }
                    }

                    tbProduct.Alias = Utilities.SEOUrl(tbProduct.ProductName);
                    tbProduct.DateModified = DateTime.Now;
                    _context.Update(tbProduct);
                    if (tbProduct.Price.HasValue && tbProduct.Discount.HasValue)
                    {
                        tbProduct.SalePrice = Convert.ToInt32(tbProduct.Price.Value - (tbProduct.Price.Value * tbProduct.Discount.Value / 100));
                    }
                    else
                    {
                        tbProduct.SalePrice = tbProduct.Price;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProductExists(tbProduct.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.TbProductCategories, "CatId", "CatName", tbProduct.CatId);

            return View(tbProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbProducts == null)
            {
                return Problem("Entity set 'Se104Context.TbProducts'  is null.");
            }
            var tbProduct = await _context.TbProducts.FindAsync(id);
            var tbProductImages = _context.TbProductImages.Where(x => x.ProductId == id);

            if (tbProduct != null)
            {
                foreach (var image in tbProductImages)
                {
                    _context.TbProductImages.Remove(image);

                }

                _context.TbProducts.Remove(tbProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products", "/Admin/Products");
        }


        private bool TbProductExists(int id)
        {
          return _context.TbProducts.Any(e => e.ProductId == id);
        }
    }
}
