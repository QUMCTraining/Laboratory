using LaboratoryProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using LaboratoryProject.Models;

namespace LaboratoryProject.Controllers
{
    public class ManagmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var limitationCountResult = _context.Mangement.Where(x => x.Name == "limitationDays").FirstOrDefault();


            return View(limitationCountResult == null ? 0 : limitationCountResult.Value);
        }

        // POST: Managements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Edit(int limitationDays)
        {
            var limitationDaysObject = _context.Mangement.Where(x => x.Name == "limitationDays").FirstOrDefault();

            if (limitationDaysObject == null)
            {
                limitationDaysObject = new Managment();
                limitationDaysObject.Name = "limitationDays";
                limitationDaysObject.Value = limitationDays;
                _context.Add(limitationDaysObject);
            }
            else
            {
                limitationDaysObject.Value = limitationDays;
            }
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index),"Requests");
            return View(limitationDays);
        }
    }
}
