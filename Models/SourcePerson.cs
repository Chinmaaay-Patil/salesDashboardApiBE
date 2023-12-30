using System;
using System.Collections.Generic;

namespace Sales_Dash_Board.Models
{
    public partial class SourcePerson
    {
        public int Spid { get; set; }
        public string SourcePersonName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public int? SourceId { get; set; }
    }
}
