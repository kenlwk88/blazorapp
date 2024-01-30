using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLA.CSSMS.Shared.Dtos.IMS
{
    public class DocumentsDto
    {
        public string RefNo { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string IsFileConverted { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string TiffPath { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdateAt { get; set; }
    }
}
