using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLA.CSSMS.Shared.Models
{
    public class IMSSubmissionFilter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public string RefNo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PolicyNo { get; set; } = string.Empty;
    }
}
