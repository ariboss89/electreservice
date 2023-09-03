using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectreService.Models
{
    public class subkriteria
    {
        public int Id { get; set; }
        public int id_kriteria { get; set; }
        public string nama_kriteria { get; set; }
        public string pilihan { get; set; }
        public int nilai { get; set; }
    }
}