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
    public class ProductCategoriesController : Controller
    {
        private readonly Se104Context _context;

        public ProductCategoriesController(Se104Context context)
        {
            _context = context;
        }

        // GET: Admin/ProductCategories
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCategories = _context.TbProductCategories
                .AsNoTracking()
                .OrderBy(x => x.CatId);
            PagedList<TbProductCategory> models = new PagedList<TbProductCategory>(lsCategories.AsQueryable(), pageNumber, pageSize);

            return await Task.FromResult(View(models));
        }

        public IActionResult GetDanhMucCha(int danhMucGocId)
        {
            var danhMucChaList = _context.TbProductCategories
                .Where(x => x.RootId == danhMucGocId && x.Levels == 1)
                .OrderBy(x => x.CatName)
                .ToList();

            return Json(danhMucChaList);
        }


        // GET: Admin/ProductCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbProductCategories == null)
            {
                return NotFound();
            }

            var tbProductCategory = await _context.TbProductCategories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (tbProductCategory == null)
            {
                return NotFound();
            }

            return View(tbProductCategory);
        }

        // GET: Admin/ProductCategories/Create
        public IActionResult Create()
        {
            ViewData["DanhMucGoc"] = new SelectList(_context.TbProductCategories.Where(x => x.Levels == 0), "CatId", "CatName");
            ViewData["DanhMucCha"] = new SelectList(Enumerable.Empty<SelectListItem>(), "CatId", "CatName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CatId,CatName,Description,Ordering,Published,Image,Alias,CreatedDate,CreatedBy,DateModified,Title,AccountId,ParentId,RootId,Levels")] TbProductCategory tbProductCategory)
        {
            if (ModelState.IsValid)
            {
                if (tbProductCategory.RootId == 0 || tbProductCategory.RootId == null)
                {
                    tbProductCategory.Levels = 0;
                    tbProductCategory.ParentId = 0;
                }
                else
                {
                    if (tbProductCategory.ParentId == 0 || tbProductCategory.ParentId == null)
                    {
                        tbProductCategory.Levels = 1;
                    }
                    else
                    {
                        tbProductCategory.Levels = 2;

                    }
                }

                if (string.IsNullOrEmpty(tbProductCategory.Image)) tbProductCategory.Image = "/Uploads/default.jpg";


                tbProductCategory.Alias = Utilities.SEOUrl(tbProductCategory.CatName);

                _context.Add(tbProductCategory);
                tbProductCategory.CreatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tbProductCategory);
        }

        // GET: Admin/ProductCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TbProductCategories == null)
            {
                return NotFound();
            }

            var tbProductCategory = await _context.TbProductCategories.FindAsync(id);
            if (tbProductCategory == null)
            {
                return NotFound();
            }

            ViewData["DanhMucGoc"] = new SelectList(_context.TbProductCategories.Where(x => x.Levels == 0), "CatId", "CatName");
            ViewData["DanhMucCha"] = new SelectList(_context.TbProductCategories.Where(x => x.Levels == 1 && x.RootId == tbProductCategory.RootId), "CatId", "CatName");

            return View(tbProductCategory);
        }

        // POST: Admin/ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CatId,CatName,Description,Ordering,Published,Image,Alias,CreatedDate,CreatedBy,DateModified,Title,AccountId,ParentId,RootId,Levels")] TbProductCategory tbProductCategory)
        {
            if (id != tbProductCategory.CatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (tbProductCategory.RootId == 0 || tbProductCategory.RootId == null)
                    {
                        tbProductCategory.Levels = 0;
                        tbProductCategory.ParentId = 0;
                    }
                    else
                    {
                        if (tbProductCategory.ParentId == 0 || tbProductCategory.ParentId == null)
                        {
                            tbProductCategory.Levels = 1;
                        }
                        else
                        {
                            tbProductCategory.Levels = 2;

                        }
                    }
                    if (string.IsNullOrEmpty(tbProductCategory.Image)) tbProductCategory.Image = "default.jpg";

                    tbProductCategory.Alias = Utilities.SEOUrl(tbProductCategory.CatName);
                    tbProductCategory.DateModified = DateTime.Now;

                    _context.Update(tbProductCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProductCategoryExists(tbProductCategory.CatId))
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
            return View(tbProductCategory);
        }

        // GET: Admin/ProductCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TbProductCategories == null)
            {
                return NotFound();
            }

            var tbProductCategory = await _context.TbProductCategories
                .FirstOrDefaultAsync(m => m.CatId == id);
            if (tbProductCategory == null)
            {
                return NotFound();
            }

            return View(tbProductCategory);
        }

        // POST: Admin/ProductCategories/Delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TbProductCategories == null)
            {
                return Problem("Entity set 'Se104Context.TbProductCategories'  is null.");
            }
            var tbProductCategory = await _context.TbProductCategories.FindAsync(id);
            if (tbProductCategory != null)
            {
                _context.TbProductCategories.Remove(tbProductCategory);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbProductCategoryExists(int id)
        {
          return (_context.TbProductCategories?.Any(e => e.CatId == id)).GetValueOrDefault();
        }
    }
}
