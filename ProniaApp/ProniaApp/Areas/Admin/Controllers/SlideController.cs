using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Areas.Admin.ViewModels.Slide;
using ProniaApp.DAL;
using ProniaApp.Models;
using ProniaApp.Utilities.Enums;
using ProniaApp.Utilities.Extensions;

namespace ProniaApp.Areas.Admin.Controllers;

[Area("Admin")]
public class SlideController : Controller

{
    public readonly AppDbContext _context;
    public readonly IWebHostEnvironment _env;
    public SlideController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public async Task<ActionResult> Index()
    {
        List<Slide> slides = await _context.Sliders.ToListAsync();
        return View(slides);
    }
    public ActionResult Test()
    {
        string result = Guid.NewGuid().ToString();
        return Content(result);
    }
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateSlideVM createSlideVM)
    {


        if (!ModelState.IsValid)
        {
            return View();
        }
        // slide.Photo.ValidateType("image/")
        if (!createSlideVM.Photo.ValidateType("image/"))
        {
            ModelState.AddModelError("Photo", "Siz uygun formatta file elave etmirsiz");
            return View();
        }
        // file, fileSizeType, size 
        // slide.Pthoto, kb,mg,gb, 5

        if (createSlideVM.Photo.ValidateSize(FileSize.MB, 200))
        {
            ModelState.AddModelError("Photo", "Siz duzgun hecmde file elave etmirsiz");
            return View();
        }


        Slide slide = new Slide()
        {
            Title = createSlideVM.Title,
            Description = createSlideVM.Description,
            Discount = createSlideVM.Discount,
            Order = createSlideVM.Order,
            Image = await createSlideVM.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images")
        };



        // return Content(slide.Photo.FileName + " " + slide.Photo.ContentType + " " + slide.Photo.Length);
        await _context.Sliders.AddAsync(slide);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null || id < 1)
        {
            return BadRequest();
        }

        Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slide == null)
        {
            return NotFound();
        }

        // System.IO.File.Delete(Path.Combine(_env.WebRootPath,"assets","images","website-images",slide.Image));

        slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
        _context.Sliders.Remove(slide);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }



    public async Task<ActionResult> Update(int? id)
    {
        if (id == null || id < 1)
        {
            return BadRequest();
        }

        Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slide == null)
        {
            return NotFound();
        }

        UpdateSlideVM updateSlideVM = new UpdateSlideVM()
        {
            Title = slide.Title,
            Description = slide.Description,
            Discount = slide.Discount,
            Order = slide.Order,
            Image = slide.Image,
        };
        return View(updateSlideVM);
    }

    [HttpPost]
    public async Task<ActionResult> Update(int? id, UpdateSlideVM updateSlideVM)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSlideVM);
        }

        Slide slide = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

        if (updateSlideVM.Photo is not null)
        {
            if (!updateSlideVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(updateSlideVM.Photo), "invalid type");
                return View(updateSlideVM);
            }

            if (updateSlideVM.Photo.ValidateSize(FileSize.KB, 20))
            {
                ModelState.AddModelError(nameof(updateSlideVM.Photo), "invalid size");
                return View(updateSlideVM);
            }

            string filename = await updateSlideVM.Photo.CreateFile(_env.WebRootPath, "assets", "images", "website-images");
            slide.Image.DeleteFile(_env.WebRootPath, "assets", "images", "website-images");
            slide.Image = filename;
        }


        slide.Title = updateSlideVM.Title;
        slide.Description = updateSlideVM.Description;
        slide.Discount = updateSlideVM.Discount;
        slide.Order = updateSlideVM.Order;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
