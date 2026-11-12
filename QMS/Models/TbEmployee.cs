using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Models
{
    [Table("tb_employee")]
    public class TbEmployee
    {
        [Key]
        [Column("no_peg")]
        public int NoPeg { get; set; }

        [Column("emp_name")]
        public string EmpName { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("reset_pswd")]
        public string ResetPswd { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("jabatan")]
        public string? Jabatan { get; set; }

        [Column("fungsi")]
        public string? Fungsi { get; set; }

        [Column("telpon")]
        public string? Telpon { get; set; }
    }
}
