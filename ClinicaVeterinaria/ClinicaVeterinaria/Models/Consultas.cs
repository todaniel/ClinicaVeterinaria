using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.Models
{
    public class Consultas{

        [Key] //força o atributo a ser PK //poderia ser com nome da classe juntando ID ou apenas ID
        public int ConsultaID { get; set; }

        [Column(TypeName = "date")] //só regista 'datas', não 'horas'
        public DateTime DataConsulta { get; set; }


        [ForeignKey("Veterinario")]
        public int VeterinarioFK { get; set; }
        public virtual Veterinarios Veterinario { get; set; }


        [ForeignKey("Animal")]
        public int AnimalFK { get; set; }
        public virtual Animais Animal { get; set; }
    }
}