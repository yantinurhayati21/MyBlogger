using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlogger.Models;
namespace MyBlogger.Controllers;

public class UserController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public UserController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _context.User.ToListAsync();
        return View(data);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var data = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

        var indoCulture = CultureInfo.GetCultureInfo("id-ID");

        data.FormatTanggalLahir = data.TanggalLahir.ToString("d MMMM yyyy", indoCulture);

        var fullpath = Path.Combine(_env.WebRootPath, "upload", data.Foto);
        ViewBag.Foto = fullpath;
        return View(data);
    }

    public async Task<IActionResult> DownloadFoto(int id)
    {
        var data = await _context.User.FirstOrDefaultAsync(x => x.Id == id);

        if (!string.IsNullOrEmpty(data.Foto))
        {
            var fullpath = Path.Combine(_env.WebRootPath, "upload", data.Foto);
            var filebyte = System.IO.File.ReadAllBytes(fullpath);

            return File(filebyte, "application/octet-stream", data.Foto);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }
}