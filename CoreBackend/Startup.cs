using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace CoreBackend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc() // 注册MVC到Container
                .AddMvcOptions(options => //asp.net core 默认只实现了json.
                                          // 为了也能够支持返回XML格式，
                                          // 可以在ConfigureServices里面修改Mvc的配置来添加xml格式:
            {
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });
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
                app.UseExceptionHandler(); //在正式环境中, 我们遇到exception的时候, 需要捕获并把它记录(log)下来
                                           //这时候我们应该使用这个middleware: Exception Handler Middleware, 我们可以这样调用它:
                                           // UseExceptionHandler是可以传参数的, 但暂时先这样, 我们在app.Run方法里抛一个异常
                                           // 然后运行程序, 在Chrome里按F12就会发现有一个(或若干个, 多少次请求, 就有多少个错误)500错误.
                                           // 用来创建 Web Api的middleware:
            }
            app.UseStatusCodePages(); // !!!status code middleware
            app.UseMvc();// ConfigureServices里面写完services.AddMvc()后，在这里也要写上这条来告诉程序使用MVC

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("API源，非礼勿视!");
            });
        }
    }
}
