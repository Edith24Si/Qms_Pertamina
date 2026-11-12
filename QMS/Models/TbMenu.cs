using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMS.Core.Models
{
    [Table("tb_menu")]
    public class TbMenu
    {
        [Key]
        [Column("menu_id")]
        public int MenuId { get; set; }

        [Column("menu")]
        public string Menu { get; set; } = string.Empty;

        [Column("submenu")]
        public string Submenu { get; set; } = string.Empty;
    }
}
