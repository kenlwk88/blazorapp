using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLA.CSSMS.Shared.Dtos.IMS
{
    public class IMSSubmissionsDto
    {
        public string RefNo { get; set; } = string.Empty;
        public string PolicyNo { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public string AgentCode { get; set; } = string.Empty;
        public string CopyStatus { get; set; } = string.Empty;
        public string DecyptStatus { get; set; } = string.Empty;
        public string EAIStatus { get; set; } = string.Empty;
        public string ConvertStatus { get; set; } = string.Empty;
        public string ExportStatus { get; set; } = string.Empty;
        public string CopyErrorMsg { get; set; } = string.Empty;
        public string DecyptErrorMsg { get; set; } = string.Empty;
        public string EAIErrorMsg { get; set; } = string.Empty;
        public string ConvertErrorMsg { get; set; } = string.Empty;
        public string ExportErrorMsg { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public Errors Errors { get; set; } = new();
    }
    public class Errors 
    {
        public List<string> Messages { get; set; } = new List<string>();
    }
}
