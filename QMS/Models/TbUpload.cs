using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Models
{
    [Table("tb_upload")]
    public class TbUpload
    {
        [Key]
        [Column("upload_id")]
        public int UploadId { get; set; }

        [Column("menu_id")]
        public int MenuId { get; set; }

        [Column("creator")]
        public string Creator { get; set; } = string.Empty;

        [Column("file_name")]
        public string FileName { get; set; } = string.Empty;

        [Column("file_dir")]
        public string FileDir { get; set; } = string.Empty;

        [Column("size")]
        public string Size { get; set; } = string.Empty;

        [Column("type")]
        public string Type { get; set; } = string.Empty;
    }
}