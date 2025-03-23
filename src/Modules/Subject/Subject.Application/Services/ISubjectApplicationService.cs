using Core.Application.Services;
using Subject.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subject.Application.Services
{
    public interface ISubjectApplicationService : IApplicationService<SubjectDbContext> { }
}
