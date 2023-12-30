using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sales_Dash_Board.Models
{
    public class Document
    {
        public partial class DocModify
        {
            public int VersionId { get; set; }
            public string LabName { get; set; }
            public string Address { get; set; }
            public string TextFilePath { get; set; }
        }
    }
}
