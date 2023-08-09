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
        public async Task<IActionResult> Index(string searchSelected, string searchLabel)
        {
            // Filter data (search from college and student status database)
            if (_context.Request == null)
            {
                return NotFound("Request Is Null");
            }
            var searchelement = from c in _context.Request select c;
            if (!string.IsNullOrEmpty(searchLabel))
            {
                if (searchSelected == "College")
                {
                    searchelement = searchelement.Where(s => s.College.Contains(searchLabel));
                }
                else if (searchSelected == "Student Status")
                {
                    searchelement = searchelement.Where(s => s.StudentsStatus.Contains(searchLabel));
                }
            }
            return View(await searchelement.ToListAsync());
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
        public List<DateTime>? GetAvilableDates()
        {
            var managment = _context.Management.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (managment is null)
            {
               
                return null;
            }
            var limitDays = managment.Value;

            var dateTo = DateTime.Now.AddDays(30);

            List<DateTime> avilableDates = new List<DateTime>();
            for (var date = DateTime.Now; date <= dateTo; date = date.AddDays(1))
            {
                if (date.DayOfWeek.ToString() == "Friday" || date.DayOfWeek.ToString() == "Saturday")
                {
                    continue;
                }
                var requestCount = _context.Request.Where(x => x.TestDate.Date == date.Date).Count();
                if (requestCount >= limitDays)
                {
                    continue;
                }
                avilableDates.Add(date);
            }
            return avilableDates;
        }
        // GET: Requests/Create
        public IActionResult Create()
        {
            RequestVM requestVM = new RequestVM();
            var colleges = _context.College.ToList();
            requestVM.CollagesSelectList = new SelectList(colleges, "Name", "Name");
            var avilableDates = GetAvilableDates();
            if (avilableDates is null)
            {
                requestVM.ErrorMessage = "You need to set the limit in mangment page";
            }
            else
            {
                requestVM.AvilableDates = avilableDates;    
            }

            return View(requestVM);

        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,NationalOrResidenceId,UniversityNumber,StudentsStatus,College,FirstNameEnglish," +
            "FatherNameEnglish,GrandFatherNameEnglish,FamilyNameEnglish,FirstNameArabic,FatherNameArabic," +
            "GrandFatherNameArabic,FamilyNameArabic,Email,PhoneNo,BirthDate,MedicalFileNo,TestDate")] Request request,
            IFormFile nationalOrResidenceIdFile, IFormFile copyOfStudentId)
        {
            RequestVM requestVM = new RequestVM();
            requestVM.Request = request;
            var colleges = _context.College.ToList();
            requestVM.CollagesSelectList = new SelectList(colleges, "Name", "Name");
            var management = _context.Management.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (management is null)
            {
                requestVM.ErrorMessage = "You need to set the limit in mangment page";
                return View(requestVM);
            }

            var avilableDates= GetAvilableDates();  
            if(avilableDates is not null) 
            {
                requestVM.AvilableDates= avilableDates;
            }

            var limitDays = management.Value;
            var requestsCount = _context.Request.Where(X => X.TestDate == request.TestDate).Count();
            if (requestsCount >= limitDays)
            {

                requestVM.ErrorMessage = "Sorry,The limit of Requests for this Day is Reached";
                return View(requestVM);
            }

            // Uploaded files by use GUID

            var nationalOrResidenceIdFileName = Guid.NewGuid().ToString() + ".jpg";
            var copyOfStudentIdName = Guid.NewGuid().ToString() + ".jpg";

            var residenceIdFullPath = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", nationalOrResidenceIdFileName);
            var studentIdFullPath = System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", copyOfStudentIdName);

            using (var stream = new System.IO.FileStream(residenceIdFullPath, System.IO.FileMode.Create))
            {
                await nationalOrResidenceIdFile.CopyToAsync(stream);
            }
            using (var stream = new System.IO.FileStream(studentIdFullPath, System.IO.FileMode.Create))
            {
                await copyOfStudentId.CopyToAsync(stream);
            }
            request.NationalOrResidenceIdFile = nationalOrResidenceIdFileName;
            request.CopyOfStudentId = copyOfStudentIdName;

            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("Message");
            }
            return View(requestVM);
        }

        public IActionResult Message()
        {
            return View();
        }


        // Export Data to Excel 
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

        private bool RequestExists(int id)
        {
          return (_context.Request?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
