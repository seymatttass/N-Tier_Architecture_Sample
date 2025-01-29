using Microsoft.EntityFrameworkCore;
using School.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess
{
    public class SchoolDbContex  :  DbContext
    {

        public SchoolDbContex()
        {
        }

        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentScore> StudentScores { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=SEYMA\\SQLEXPRESS03; Database=SchoolDb; Integrated Security=True; TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // İlişkileri ve konfigürasyonları burada tanımlayabiliriz.
        }




    }
}
