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

        public async Task<IActionResult> Index(string? subject, string status)
        {
            status = "New";

            var viewModel = _service.GetBills(subject, status);

            if (string.IsNullOrWhiteSpace(status))
            {
                status = "All";
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                subject = "All";
            }

            ViewData["SessionMessage"] = _alertService.GetSessionMessage();
            ViewData["StatusValue"] = status;
            ViewData["SubjectValue"] = subject;
            ViewData["ShouldRemoveDiv"] = (status == "New" && status == "Updated");

            return View(viewModel);
        }
    }
}
