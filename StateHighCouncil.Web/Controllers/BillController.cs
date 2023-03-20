using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
using StateHighCouncil.Web.Services;
using System.Web;
using System.Security.Claims;

namespace StateHighCouncil.Web.Controllers;

public class BillController : Controller
{
    private readonly DataContext _context;
    private readonly IAlertService _alertService;
    private readonly IBillService _service;

    public BillController(DataContext context,
        IAlertService alertService,
        IBillService billService)
    {
        _context = context;
        _alertService = alertService;
        _service = billService;
    }

    public async Task<IActionResult> Index()
    {
        var setting = (_context.SystemSettings).FirstOrDefault();

        var viewModel = await _service.GetBillsAsync(setting.Status, setting.Subject);

        ViewData["SessionMessage"] = _alertService.GetSessionMessage();
        
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(BillUpdateViewModel update)
    {
        var viewModel = await _service.UpdateBillAsync(update);
        ViewData["SessionMessage"] = _alertService.GetSessionMessage();
        //return View("Index");
        return View(viewModel);
    }
}
