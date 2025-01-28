using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace School.Entities
{
    public class Person : BaseEntity
    {
        [Required]
        [MaxLength(128)]
        public string FullName { get; set; }

        [MaxLength(11)]
        [Required]
        public string IdentityNumber { get; set; }

        [Required]
        public DateTime BornDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
