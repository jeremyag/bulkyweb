using Bulky.DataAccess.Data;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.RoleAdmin)]
public class UserController : Controller
{
    private readonly ApplicationDbContext _db;
    public UserController(ApplicationDbContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        return View();
    }

    #region API Calls
    [HttpGet]
    public IActionResult GetAll()
    {
        var userLists = _db.ApplicationUsers.Include(u => u.Company).ToList();

        var userRoles = _db.UserRoles.ToList();
        var roles = _db.Roles.ToList();

        foreach(var user in userLists)
        {
            if (user.Company == null)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id)?.RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId)?.Name;

                user.Company = new Company()
                {
                    Name = ""
                };
            }
        }
        return Json(new { data = userLists });
    }

    [HttpPost]
    public IActionResult LockUnlock([FromBody]string id)
    {
        var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        if(user == null)
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        if(user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
        {
            user.LockoutEnd = DateTime.Now;
        }else
        {
            user.LockoutEnd = DateTime.Now.AddYears(1000);
        }

        _db.SaveChanges();
        return Json(new { success = true, message = "Delete Successful" });
    }
    #endregion

}
