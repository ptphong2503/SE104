using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SE104.Models;

namespace SE104.Controllers
{
    [ViewComponent(Name = "_Header")]
    public class _HeaderViewComponent : ViewComponent
    {
        private readonly Se104Context _context;
        public _HeaderViewComponent(Se104Context context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var list = _context.TbProductCategories
                       .OrderBy(x => x.Ordering)
                       .Where(x => x.Published == true).ToList();

            ViewBag.list = list;


            return View("_Header") ;
        }
    }
}
