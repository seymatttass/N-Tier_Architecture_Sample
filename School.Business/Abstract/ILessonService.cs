using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Core.Business.DTOs.Lesson;
using School.Business.Utils;


namespace School.Business.Abstract
{
    public interface ILessonService : ICrudEntityService<LessonGetDto, LessonDto, LessonDto>
    {

    }
}
