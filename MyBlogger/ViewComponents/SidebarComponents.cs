using Microsoft.AspNetCore.Mvc;
using MyBlogger.Models;

namespace MyBlogger.ViewComponents
{
    public class SidebarViewComponent:ViewComponent
    {
        public readonly AppDbContext _context;

        public SidebarViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            List<Menu>menus=_context.Menu.ToList();
            /*List<Menu> menus = new List<Menu>();
            menus.Add(new Menu
            {
                Id = 1,
                Name = "Home",
                Link = "/",
                Icon = "fa fa-home"
            });

            menus.Add(new Menu
            {
                Id = 2,
                Name = "Post",
                Link = "/post",
                Icon = "fa fa-file"
            });

            menus.Add(new Menu
            {
                Id = 3,
                Name = "User",
                Link = "/user",
                Icon = "fa fa-user"
            });*/
            return View(menus);
        }
    }
}
