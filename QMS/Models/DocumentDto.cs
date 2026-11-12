namespace QMS.Core.Models
{
    public class DocumentDto
    {
        public int UploadId { get; set; }
        public int MenuId { get; set; }
        public string Creator { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string FileDir { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Category { get; set; } = "Dokumen Mutu";
        public string Submenu { get; set; } = "Umum";
    }
}
