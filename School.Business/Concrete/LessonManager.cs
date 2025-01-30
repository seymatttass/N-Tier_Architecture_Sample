using AutoMapper;
using Core.Business.DTOs.Lesson;
using School.Business.Abstract;
using School.Business.Utils;
using School.DataAccess;
using School.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Business.Concrete
{
    public class LessonManager : CrudEntityManager<Lesson, LessonGetDto, LessonDto, LessonDto>, ILessonService
    {
        public LessonManager(IUnitOfWorks unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
