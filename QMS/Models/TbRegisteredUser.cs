using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Models
{
    [Table("tb_registered_user")]
    public class TbRegisteredUser
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("full_name")]
        public string FullName { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("no_peg")]
        public int NoPeg { get; set; }

        [Column("jabatan")]
        public string? Jabatan { get; set; }

        [Column("fungsi")]
        public string? Fungsi { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("role")]
        public string Role { get; set; } = "Pegawai";

        [Column("registered_at")]
        public string RegisteredAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        [Column("photo_url")]
        public string? PhotoUrl { get; set; }
    }
}
