
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using panel.Repository;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.RepoExtension
{
    public static class RepoExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<ILoginRepo, LoginRepo>();
            services.AddScoped<IPropertyTabRepo, PropertyTabRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
        }
    }
}
