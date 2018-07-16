﻿using AspNetCore.Mvc.Fragments.Datasource;
using AspNetCore.Mvc.Fragments.Demo.FragmentSource1.Services;
using AspNetCore.Mvc.Fragments.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddMvc()
                .AddFragments(fragmentOptions => fragmentOptions.AddDatasource(new FragmentAssamblyDatasource(typeof(Startup).Assembly)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseDeveloperExceptionPage()
                .UseStaticFiles()
                .UseFragments()
                .UseMvc(builder => builder.MapFragmentRoute());
        }
    }
}