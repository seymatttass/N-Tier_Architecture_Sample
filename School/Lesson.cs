using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;


namespace School.Entities
{
    public class Lesson : BaseEntity
    {
        [MaxLength(48)]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<StudentScore> StudentScores { get; set; }



    }
}
