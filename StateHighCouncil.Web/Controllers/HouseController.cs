using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using StateHighCouncil.Web.Data;
using StateHighCouncil.Web.Models;
using StateHighCouncil.Web.Services;

namespace StateHighCouncil.Web.Controllers
{
    public class HouseController : Controller
    {
        private readonly DataContext _context;
        private readonly ILegislatorService _service;
        private readonly IAlertService _alertService;

        public HouseController(DataContext context,
            ILegislatorService service,
            IAlertService alertService)
        {
            _context = context;
            _service = service;
            _alertService = alertService;
        }

          public async Task<IActionResult> Index()
        {
            ViewData["SessionMessage"] = _alertService.GetSessionMessage();
            ViewData["Title"] = "House of Representatives";
            var viewModel = await _service.GetLegislatorsListAsync("H");
            return viewModel != null && viewModel.Any() ?
                        View(viewModel) :
                        Problem("Entity set 'DataContext.Legislators'  is null.");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0 )
            {
                return NotFound();
            }

            var viewModel = await _service.GetLegislatorAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            ViewData["SessionMessage"] = _alertService.GetSessionMessage();
            ViewData["Title"] = "Rep. " + viewModel.Name;
            return View(viewModel);
        }

        // GET: House/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Legislators == null)
            {
                return NotFound();
            }

            var legislator = await _context.Legislators.FindAsync(id);
            if (legislator == null)
            {
                return NotFound();
            }
            return View(legislator);
        }

        // POST: House/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StateId,ImageUrl,House,Party,Position,District,ServiceStart,Profession,ProfessionalAffiliations,Education,RecognitionsAndHonors,Counties,Address,Email,Cell,WorkPhone,LegislationUrl,DemographicUrl,Religion,Gender,Status,TwitterUrl,FacebookUrl,HomePhone,Bio,Fax,InstagramUrl,WhenAdded,WhenLastUpdated")] Legislator legislator)
        {
            if (id != legislator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(legislator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LegislatorExists(legislator.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(legislator);
        }

        private bool LegislatorExists(int id)
        {
            return (_context.Legislators?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}
