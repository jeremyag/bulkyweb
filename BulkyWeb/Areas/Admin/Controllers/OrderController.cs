using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;

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
    public IActionResult GetAll(string status)
    {
        IEnumerable<OrderHeader> orderHeaders = _unitOfWork.OrderHeaders.GetAll(includeProperties: "ApplicationUser").ToList();

        switch (status)
        {
            case "pending":
               orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusPending);
                break;
            case "inprocess":
                orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                break;
            case "completed":
                orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                break;
            case "approved":
                orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                break;
        }
        return Json(new {data=orderHeaders});
    }
    #endregion
}
