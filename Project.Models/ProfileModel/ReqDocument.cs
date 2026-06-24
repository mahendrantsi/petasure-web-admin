using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.ProfileModel
{

    public class RequireDocs
    {
        public long DocumentID { get; set; }
        public string DocTypeName { get; set; }
        public string DocDescription { get; set; }
        public bool IsRequire { get; set; }
        public IFormFile DocFile { get; set; }
        public string Path { get; set; }
    }

    public class DocSaveModel
    {
        public IFormFile File { get; set; }
        public long DocumentID { get; set; }
        public long UserID { get; set; }
        public long CreatedBy { get; set; }
        public EnumFileType FileType { get; set; } = EnumFileType.Profile;
        public string FileName { get; set; }
        public string Name { get; set; }
    }
}
