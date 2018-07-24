using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountBook.Common;
using AccountBook.Model;
using AccountBook.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AccountBook.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IServiceCollection _services;
        public IConfiguration Configuration { get; }      
        public void ConfigureServices(IServiceCollection services)
        {
            //利用ASP.NET Core的依赖注入容器系统，通过请求获取IHttpContextAccessor接口，我们拥有模拟使用HttpContext.Current这样API的可能性。但是因为IHttpContextAccessor接口默认不是由依赖注入进行实例管理的。我们先要将它注册到ServiceCollection中：
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            string sqlConnectionStr = Configuration.GetConnectionString("SqlServerConnection");
            ConnectionFactory.ConnectionStr = sqlConnectionStr;
            services.AddDbContext<AppliactionDbContext>(options => options.UseSqlServer(sqlConnectionStr, o => o.UseRowNumberForPaging()));
            //注入
           services.AddScoped<UserInfoRepository>();

            #region cookie使用
            //注册Cookie认证服务
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=> {
                
                options.LoginPath = "/Login/Index";//设置登录页
            });

            //services.AddAuthentication("MyCookieAuthenticationScheme").AddCookie("MyCookieAuthenticationScheme", options =>
            //{
            //    // 当用户尝试访问资源但没有通过任何授权策略时，这是请求会重定向的相对路径资源。
            //    options.AccessDeniedPath= "/Account/Forbidden/";
            //    options.LoginPath = "/Login/Index";//设置登录页
            //});
            #endregion
            #region session使用
            //在新建Asp.net core应用程序后，要使用session中间件，在startup.cs中需执行三个步骤：
            //1.使用实现了IDistributedcache接口的服务来启用内存缓存。（例如使用内存缓存）
            services.AddDistributedMemoryCache();
            services.AddSession(
                options => {
                    //  options.Cookie.Name = "";
                    options.IdleTimeout = TimeSpan.FromMinutes(20);
                    //设置HttpOnly标志以防止客户端JavaScript中访问Cookie
                    options.Cookie.HttpOnly = true;
                });
            #endregion
            services.AddMvc()
                  .AddJsonOptions(options =>
                  {
                      //忽略循环引用
                      options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                      //不使用驼峰样式的key
                      options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                      //设置时间格式
                      options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                  }
                );
            _services = services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
          //  app.UseDefaultFiles();
            // 启用session           
             app.UseSession();
            #region 启用cookie
            //1、启用cookie会话 =>在Startup.cs文件中的Configure方法中的调用UseAuthentication方法会添加设置HttpContext.User属性的 AuthenticationMiddleware 中间件。
            //2、//注意app.UseAuthentication方法一定要放在下面的app.UseMvc方法前面，否者后面就算调用HttpContext.SignInAsync进行用户登录后，使用
            //HttpContext.User还是会显示用户没有登录，并且HttpContext.User.Claims读取不到登录用户的任何信息。
            //这说明Asp.Net OWIN框架中MiddleWare的调用顺序会对系统功能产生很大的影响，各个MiddleWare的调用顺序一定不能反
            app.UseAuthentication();
            #endregion
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
}
