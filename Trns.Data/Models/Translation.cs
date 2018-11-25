using System;

namespace Trns.Data.Models
{
    public class Translation
    {     
        public int Id { get; set; }
        public string TransKey { get; set; }
        public string Text { get; set; }
        public string Spanish { get; set; }
        public string BlockedBy { get; set; }
        public DateTime? BlockedTime { get; set; }
        public string TransBy { get; set; }
        public DateTime? TransDate { get; set; }
        public string CheckedBy { get; set; }
        public DateTime? CheckedTime { get; set; }
        public string EditedBy { get; set; }
        public DateTime? EditedTime { get; set; }
        public string Comment { get; set; }
        public string Edition { get; set; }
    }
}
