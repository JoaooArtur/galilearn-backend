using Core.Application.Services;
using Student.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Application.Services
{
    public interface IStudentApplicationService : IApplicationService<StudentDbContext> { }
}
