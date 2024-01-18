using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpicyXWebsite.Areas.Admin.ViewModel;
using SpicyXWebsite.DAL;
using SpicyXWebsite.Models;

namespace SpicyXWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _context.Settings.ToListAsync();
            return View(settings);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSettingVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            bool isExist = await _context.Settings.AnyAsync(s => s.Key == vm.Key);
            if (isExist)
            {
                ModelState.AddModelError("Key", "This Key is already exist");
                return View(vm);
            }
            Setting setting = new()
            {
                Key = vm.Key,
                Value = vm.Value
            };
            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Setting setting = await _context.Settings.FirstOrDefaultAsync(i => i.Id == id);
            if (setting is null) return NotFound();

            UpdateSettingVm vm = new()
            {
                Key = setting.Key,
                Value = setting.Value
            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSettingVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _context.Settings.AnyAsync(s => s.Key == vm.Key && s.Id != id))
            {
                ModelState.AddModelError("Name", "This Setting is already exist");
                return View(vm);
            }
            Setting existed= await _context.Settings.FirstOrDefaultAsync(s=>s.Id== id);
            existed.Key=vm.Key;
            existed.Value=vm.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Setting setting = await _context.Settings.FirstOrDefaultAsync(i => i.Id == id);
            if (setting is null) return NotFound();

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
