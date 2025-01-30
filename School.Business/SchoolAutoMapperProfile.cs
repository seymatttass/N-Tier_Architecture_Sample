using AutoMapper;
using Core.Business.DTOs.Lesson;
using Core.Business.DTOs.Student;
using School.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Business
{
    public class SchoolAutoMapperProfile : Profile
    {
        public SchoolAutoMapperProfile()
        {
            CreateMap<StudentCreateDto, Student>();  
            CreateMap<Student, StudentGetDto>();     
            CreateMap<StudentCreateDto, Person>();
            CreateMap<StudentUpdateDto, Person>();
            CreateMap<StudentCreateDto, StudentGetDto>();
            CreateMap<Person, Student>();    
            CreateMap<Person, StudentGetDto>();     

            CreateMap<LessonDto, Lesson>();   //lessondto yu lessona çeviriyorum
            CreateMap<Lesson, LessonGetDto>();  //lessonu lessongetdto ya çeviriyorum.
                                                // birbiri arasında geçişli olsun istiyorum ki tam tersi işlemini de yapabileyim.
        }
    }
}

