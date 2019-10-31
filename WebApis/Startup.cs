using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApis.BAL;
using WebApis.BOL;
using WebApis.DAL;
using WebApis.elastic;

namespace WebApis
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddOptions();
            services.Configure<AppConfig>(Configuration);
      
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddSingleton<ISportDetails, sport>();
            services.AddSingleton<Isportz, sportzBal>();
            services.AddSingleton<ISearchDataFilter, GetMatchDetails>();
            services.AddSingleton<ESInterface, EsLayer>();
            services.AddSingleton<ICon, Connection>();
            services.AddSingleton<ICricket, Cricket>();
            services.AddSingleton<ICricketS2, CricketS2>();
            services.AddSingleton<IKabaddi, Kabaddi>();
            services.AddSingleton<IAddUpdateIndex, AddUpdateElasticIndex>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(Options => Options.WithOrigins("http://localhost:57271/").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseMvc();
            
        }
    }
}
