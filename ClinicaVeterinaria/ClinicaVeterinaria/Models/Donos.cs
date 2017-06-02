using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClinicaVeterinaria.Models{

    public class Donos{

        // vai representar os dados da tabela dos DONOS

        // criar o construtor desta classe
        // e carregar a lista de Animais
        public Donos(){
            ListaDeAnimais = new HashSet<Animais>();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//A PK não será AutoNumber
        public int DonoID { get; set; }

        [Required(ErrorMessage ="o {0} é de Preenchimento Obrigatório")]
        [Display(Name ="Nome do Dono do Animal")]
        //[RegularExpression("[A-ZÍÂÓ][a-záéíóúàèìòùâêîôûãõäëïöüç']+ ((-| )(de|da|do|dos| )?[A-ZÍÂÓ][a-záéíóúàèìòùâêîôûãõäëïöüç']+)*", 
        //    ErrorMessage ="No {0} só são aceites letras. Cada nome começa, obrigatoriamente, por uma maiúscula")]
        public string Nome { set; get; }


        [Required]
        [RegularExpression("[0-9]{9}",
            ErrorMessage = "Escreva APENAS 9 caracteres numéricos!")]
        public string NIF { get; set; }

        //***************************************************************************************************
        //       Criar um atributo para ligar este
        //      atributo à BD de autenticação
        //***************************************************************************************************
        public string UserName { get; set; } // corresponde ao LOGN
        //***************************************************************************************************


        // especificar que um DONO tem muitos ANIMAIS
        public virtual ICollection<Animais> ListaDeAnimais { get; set; }
    }
}