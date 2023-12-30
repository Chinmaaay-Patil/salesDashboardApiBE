using System;
using System.Collections.Generic;

namespace Sales_Dash_Board.Models
{
    public partial class SalesTrack
    {
        public int Id { get; set; }
        public string LabName { get; set; }
        public string OwnerName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? Createddate { get; set; }
        public int? SourceId { get; set; }
        public int? VersionId { get; set; }
        public decimal? ProjectedAmount { get; set; }
        public int? SalesPersonId { get; set; }
        public int? StateId { get; set; }
        public string Requirement { get; set; }
        public string Comment { get; set; }
        public DateTime? Followupdate { get; set; }
        public string Attachments { get; set; }
        public string VesionName { get; set; }
        public string SalesPersonName { get; set; }
        public string StateName { get; set; }
    }
}
