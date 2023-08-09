using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaboratoryProject.Models
{
    public class RequestVM
    {
        public Request Request { get; set; }

        public List<DateTime> AvilableDates { get;  set; }

        public SelectList CollagesSelectList { get; set; }

    }
}
