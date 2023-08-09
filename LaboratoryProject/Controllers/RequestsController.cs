using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaboratoryProject.Data;
using LaboratoryProject.Models;
using System.Data;
using ClosedXML.Excel;

namespace LaboratoryProject.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Requests
        public async Task<IActionResult> Index(string searchSelected, string searchString)
        {
            if (_context.Request == null)
            {
                return NotFound("Request Is Null");
            }
            var searchelement = from c in _context.Request select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                if (searchSelected == "College")
                {
                    searchelement = searchelement.Where(s => s.College.Contains(searchString));
                }
                else if (searchString == "Status")
                {
                    searchelement = searchelement.Where(s => s.StudentsStatus.Contains(searchString));
                }
            }
            return View(await searchelement.ToListAsync());
            //return _context.Request != null ? 
            //              View(await _context.Request.ToListAsync()) :
            //              Problem("Entity set 'ApplicationDbContext.Request'  is null.");
        }
        [HttpGet]
        public async Task<FileResult> ExportInExcel()
        {
            var Requests = await _context.Request.ToListAsync();
            var FileName = "Requests.xlsx";
            return GenerateExcel(FileName, Requests);
        }
        private FileResult GenerateExcel(string FileName, IEnumerable<Request> requests)
        {
            DataTable dataTable = new DataTable("Requests");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("NationalOrResidenceId"),
                new DataColumn("UniversityNumber"),
                new DataColumn("StudentsStatus"),
                new DataColumn("College"),
                new DataColumn("FirstNameEnglish"),
                new DataColumn("FatherNameEnglish"),
                new DataColumn("GrandFatherNameEnglish"),
                new DataColumn("FamilyNameEnglish"),
                new DataColumn("FirstNameArabic"),
                new DataColumn("FatherNameArabic"),
                new DataColumn("GrandFatherNameArabic"),
                new DataColumn("FamilyNameArabic"),
                new DataColumn("Email"),
                new DataColumn("PhoneNo"),
                new DataColumn("BirthDate"),
                new DataColumn("MedicalFileNo"),
                new DataColumn("TestDate"),
                new DataColumn("NationalOrResidenceIdFile"),
                new DataColumn("CopyOfStudentId")
            });
            foreach (var Request in requests)
            {
                dataTable.Rows.Add(
                    Request.Id,
                    Request.NationalOrResidenceId,
                    Request.UniversityNumber,
                    Request.StudentsStatus,
                    Request.College,
                    Request.FirstNameEnglish,
                    Request.FatherNameEnglish,
                    Request.GrandFatherNameEnglish,
                    Request.FamilyNameEnglish,
                    Request.FirstNameArabic,
                    Request.FatherNameArabic,
                    Request.GrandFatherNameArabic,
                    Request.FamilyNameArabic,
                    Request.Email,
                    Request.PhoneNo,
                    Request.BirthDate,
                    Request.MedicalFileNo,
                    Request.TestDate,
                    Request.NationalOrResidenceIdFile,
                    Request.CopyOfStudentId
                    );
            }
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);  
                    return File(stream.ToArray(),
                         "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                }
            }
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            //var managment = _context.Mangement.Where(x => x.Name == "limitationDays").FirstOrDefault();
            //if (managment is null)
            //{
            //    ViewBag.ErrorMessage = "You need to set the limit in mangment page";
            //    return View();
            //}
            //var limitDays = managment.Value;

            //var dateTo = DateTime.Now.AddDays(30);

            //List<DateTime> avilableDates = new List<DateTime>();
            //for (var date = DateTime.Now; date <= dateTo; date = date.AddDays(1))
            //{
            //    if (date.DayOfWeek.ToString() == "Friday" || date.DayOfWeek.ToString() == "Saturday")
            //    {
            //        continue;
            //    }
            //    var requestCount = _context.Request.Where(x => x.TestDate.Date == date.Date).Count();
            //    if (requestCount >= limitDays)
            //    {
            //        continue;
            //    }
            //    avilableDates.Add(date);
            //}
            return View();

        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NationalOrResidenceId,UniversityNumber,StudentsStatus,College,FirstNameEnglish,FatherNameEnglish,GrandFatherNameEnglish,FamilyNameEnglish,FirstNameArabic,FatherNameArabic,GrandFatherNameArabic,FamilyNameArabic,Email,PhoneNo,BirthDate,MedicalFileNo,TestDate,NationalOrResidenceIdFile,CopyOfStudentId")] Request request)
        {
            //var management = _context.Mangement.Where(x => x.Name == "limitationDays").FirstOrDefault();
            //if (management is null)
            //{
            //    ViewBag.ErrorMessage = "You need to set limit in management page";
            //    return View(vmStudentandCollages);
            //}
            //var limitDays = management.Value;
            //var requestsCount = _context.Request.Where(X => X.TestDate == request.TestDate).Count();
            //if (requestsCount >= limitDays)
            //{
            //    ViewBag.ErrorMessage = "Sorry,The limit of Requests for this Day is Reached";
            //    return View(vmStudentandCollages);
            //}

            //if (ModelState.IsValid)
            //{
            //    _context.Add(request);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return View();
        }

        // GET: Requests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NationalOrResidenceId,UniversityNumber,StudentsStatus,College,FirstNameEnglish,FatherNameEnglish,GrandFatherNameEnglish,FamilyNameEnglish,FirstNameArabic,FatherNameArabic,GrandFatherNameArabic,FamilyNameArabic,Email,PhoneNo,BirthDate,MedicalFileNo,TestDate,NationalOrResidenceIdFile,CopyOfStudentId")] Request request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequestExists(request.Id))
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
            return View(request);
        }

        // GET: Requests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Request == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .FirstOrDefaultAsync(m => m.Id == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Request == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Request'  is null.");
            }
            var request = await _context.Request.FindAsync(id);
            if (request != null)
            {
                _context.Request.Remove(request);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequestExists(int id)
        {
          return (_context.Request?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
