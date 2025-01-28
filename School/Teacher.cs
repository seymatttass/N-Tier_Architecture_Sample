using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School.Entities
{
    public class Teacher : BaseEntity   
    {
        [ForeignKey("Person")]
        [Required]
        public Guid  PersonId { get; set; }
        public Person Person { get; set; }



    }
}
