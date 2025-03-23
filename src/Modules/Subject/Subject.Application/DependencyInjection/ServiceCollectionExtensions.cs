using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subject.Application.DependencyInjection
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventInteractors(this IServiceCollection services)
            => services;
        //.AddScoped<ISendEmailWhenCreditConsultationCreatedHandler, SendEmailWhenCreditConsultationCreatedHandler>();
    }
}
