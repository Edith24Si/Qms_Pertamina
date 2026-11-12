using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Models
{
    [Table("tb_gugus")]
    public class TbGugus
    {
        [Key]
        [Column("id_gugus")]
        public int IdGugus { get; set; }

        [Column("creator")]
        public string Creator { get; set; } = string.Empty;

        [Column("category_gugus")]
        public string CategoryGugus { get; set; } = string.Empty;

        [Column("nama_gugus")]
        public string NamaGugus { get; set; } = string.Empty;

        [Column("title")]
        public string Title { get; set; } = string.Empty;

        [Column("telp")]
        public string Telp { get; set; } = string.Empty;

        [Column("acc")]
        public string Acc { get; set; } = "N"; // 'Y' or 'N'
    }
}
