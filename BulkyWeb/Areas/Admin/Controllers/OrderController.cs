using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.RoleAdmin)]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        return View();
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var orderHeaders = _unitOfWork.OrderHeaders.GetAll(includeProperties: "ApplicationUser").ToList();
        return Json(new {data=orderHeaders});
    }
    #endregion
}
