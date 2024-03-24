using System.Security.Claims;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using MyBlogger.Models;
using MyBlogger.Services;

namespace MyBlog.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly IEmailService _email;

    public AccountController(
        AppDbContext context,
        IWebHostEnvironment e,
        IEmailService em
        )
    {
        _context = context;
        _env = e;
        _email = em;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] Login data)
    {
        var userFromDb = _context.User
            .FirstOrDefault(x =>
                x.Username == data.Username
                && x.Password == data.Password);

        if (userFromDb == null)
        {
            @ViewBag.Error = "User not found";
            return View();
        }

        var claims = new List<Claim>(){
            new Claim(ClaimTypes.Name, data.Username),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var scheme = CookieAuthenticationDefaults.AuthenticationScheme;
        var identity = new ClaimsIdentity(claims, scheme);

        await HttpContext.SignInAsync(scheme, new ClaimsPrincipal(identity));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Login");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromForm] UserForm data,
        IFormFile foto
        )
    {

        if (!ModelState.IsValid)
        {
            return View(data);
        }

        var user = new User()
        {
            Username = data.Username,
            Name = data.Name,
            Email = data.Email,
            Telepon = data.Telepon,
            Alamat = data.Alamat,
            Role = data.Role,
            Password = data.Password,
            TanggalLahir = data.TanggalLahir,
        };

        if (foto != null)
        {
            var fileFolder = Path.Combine(_env.WebRootPath, "upload");

            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }

            var fullFile = Path.Combine(fileFolder, foto.FileName);

            using (var stream = System.IO.File.Create(fullFile))
            {
                await foto.CopyToAsync(stream);
            }

            user.Foto = foto.FileName;
        }
        _email.SendEmail(
            data.Email,
            "Registrasi Akun",
            "Selamat Registrasi Akun anda berhasil"
            );

        _context.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}