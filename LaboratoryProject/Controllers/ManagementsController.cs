using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaboratoryProject.Data;
using LaboratoryProject.Models;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Localization;
using System.Runtime.InteropServices;

namespace LaboratoryProject.Controllers
{
    public class ManagementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagementsController(ApplicationDbContext context)
        {
            _context = context;
        }





        // GET

        public IActionResult Edit(int? id)

        {

            var limitationCountResult = _context.Management.Where(x => x.Name == "limitationDays").FirstOrDefault();

            return View(limitationCountResult == null ? 0 : limitationCountResult.Value);

        }

        // POST

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int limitationDays)

        {

            var limitationDaysObject = _context.Management.Where(x => x.Name == "limitationDays").FirstOrDefault();

            if (limitationDaysObject == null)

            {

                limitationDaysObject = new Management();

                limitationDaysObject.Name = "limitationDays";

                limitationDaysObject.Value = limitationDays;

                _context.Management.Add(limitationDaysObject);

            }

            else

            {

                limitationDaysObject.Value = limitationDays;

            }

            _context.SaveChanges();

            //return RedirectToAction(nameof(Index),"Requests" );

            return View(limitationDays);

        }


        private bool ManagementExists(int id)
        {
            return (_context.Management?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
