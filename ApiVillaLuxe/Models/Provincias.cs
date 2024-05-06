using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiVillaLuxeApiVillaLuxe.Models
{
    [Table("Provincias")]
    public class Provincias
    {
        [Key]

        [Column("idprovincia")]
        public int IdProvincia { get; set; }

        [Column("nombre")]
        public string Provincia { get; set; }
    }
}
