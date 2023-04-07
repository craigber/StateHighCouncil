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

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Bills == null)
        {
            return NotFound();
        }

        var bill = await _context.Bills.FindAsync(id);
        if (bill == null)
        {
            return NotFound();
        }
        var viewModel = new BillEditViewModel
        {
            Id = bill.Id,
            PlusMinus = bill.PlusMinus,
            Commentary = bill.Commentary,
            IsTracked = bill.IsTracked,
            Status = bill.Status
        };
        return View(viewModel);
    }

    // POST: House/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,PlusMinus,Commentary,IsTracked,Status")] BillEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        try
        {
            var current = _context.Bills
                .Where(b => b.Id == viewModel.Id).FirstOrDefault();

            current.PlusMinus = viewModel.PlusMinus;
            current.Commentary = viewModel.Commentary;
            current.IsTracked = viewModel.IsTracked;
            current.Status = viewModel.Status;

            _context.Update(current);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            //if (!LegislatorExists(legislator.Id))
            //{
            //    return NotFound();
            //}
            //else
            //{
                throw;
            //}
        }
        return RedirectToAction(nameof(Index));
        //}
        return View(legislator);
    }
}
