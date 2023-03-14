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

namespace StateHighCouncil.Web.Controllers
{
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

        public async Task<IActionResult> Index(string? subject)
        {   
            ViewData["SessionMessage"] = _alertService.GetSessionMessage();

            var viewModel = _service.GetBills(subject);
            if(viewModel == null || !viewModel.Any())
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
