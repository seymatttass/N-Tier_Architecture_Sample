using Core.Business.DTOs.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Business.DTOs.Student;
using School.Business.Utils;


namespace School.Business.Abstract
{
    public interface IStudentService : ICrudEntityService<StudentGetDto, StudentCreateDto, StudentUpdateDto>
    {

    }
}
