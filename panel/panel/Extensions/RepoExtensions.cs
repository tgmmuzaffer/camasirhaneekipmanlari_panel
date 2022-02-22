
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using panel.Repository;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Extensions
{
    public static class RepoExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IRoleRepo, RoleRepo>();
            services.AddScoped<ILoginRepo, LoginRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            //services.AddScoped<IPropertyDescRepo, PropertyDescRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            //services.AddScoped<IProductPropertyRepo, ProductPropertyRepo>();
            services.AddScoped<IFileUpload, FileUplaod>();
            services.AddScoped<IContactRepo, ContactRepo>();
            services.AddScoped<ITagRepo, TagRepo>();
            services.AddScoped<IBlogRepo, BlogRepo>();
            services.AddScoped<ISliderRepo, SliderRepo>();
            services.AddScoped<IReferanceRepo, ReferanceRepo>();
            services.AddScoped<ISubCategoryRepo, SubCategoryRepo>();
            services.AddScoped<IFeatureRepo, FeatureRepo >();
            services.AddScoped<IFeatureDescriptionRepo, FeatureDescriptionRepo>();
            services.AddScoped<IPr_Fe_RelRepo, Pr_Fe_RelRepo>();
            services.AddScoped<IFe_FeDesc_RelRepo, Fe_FeDesc_RelRepo>();
            services.AddScoped<IFe_SubCat_RelRepo, Fe_SubCat_RelRepo>();
            services.AddScoped<ILogRepo,LogRepo>();
        }
    }
}
