using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpicyXWebsite.Areas.Admin.ViewModel;
using SpicyXWebsite.DAL;
using SpicyXWebsite.Migrations;
using SpicyXWebsite.Models;
using SpicyXWebsite.Utilities.Extension;

namespace SpicyXWebsite.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class MenuItemController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _env;

        public MenuItemController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
		{
			List<MenuItem> menuItems = await _context.MenuItems.ToListAsync();
			return View(menuItems);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateMenuItemVm vm)
		{
			if(!ModelState.IsValid)
			{
				return View(vm);
			}
			bool isExist=await _context.MenuItems.AnyAsync(mi=>mi.Name==vm.Name);
			if (isExist)
			{
				ModelState.AddModelError("Name", "This Meal is already exist");
				return View(vm);
			}

			if (!vm.Photo.ValidateSize(3))
			{
				ModelState.AddModelError("Photo", "Image size is invalid");
				return View(vm);
			}
            if (!vm.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "Image type is invalid");
                return View(vm);
            }
			string fileName = await vm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "menu");

			MenuItem menuItem = new()
			{
				Name = vm.Name,
				ImageUrl = fileName,
				Description = vm.Description,
				Price = vm.Price,
			};
			await _context.MenuItems.AddAsync(menuItem);
			await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0) return BadRequest();
			MenuItem mi= await _context.MenuItems.FirstOrDefaultAsync(i=>i.Id==id);
			if (mi is null) return NotFound();

			UpdateMenuItemVm vm = new()
			{
				Name = mi.Name,
				ImageUrl = mi.ImageUrl,
				Price = mi.Price,
				Description = mi.Description
			};
			return View(vm);
		}
		[HttpPost]
		public async Task<IActionResult> Update(UpdateMenuItemVm vm,int id)
        {

            MenuItem existed = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.Id == id);
            if (!ModelState.IsValid)
            {
                vm.ImageUrl = existed.ImageUrl;
                return View(vm);
            }
			bool isExist = await _context.MenuItems.AnyAsync(mi => mi.Name == vm.Name && mi.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This Meal is already exist");
				vm.ImageUrl = existed.ImageUrl;
                return View(vm);
            }
			if(vm.Photo is not null)
			{
                if (!vm.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "Image type is invalid");
                    return View(vm);
                }

                if (!vm.Photo.ValidateSize(3))
                {
                    ModelState.AddModelError("Photo", "Image size is invalid");
                    return View(vm);
                }
                existed.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img", "menu");
                string fileName = await vm.Photo.CreateFile(_env.WebRootPath, "assets", "img", "menu");

                existed.ImageUrl = fileName;
            }

			existed.Description = vm.Description;
			existed.Name = vm.Name;
			existed.Price = vm.Price;

			await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
	}
}
