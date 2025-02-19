﻿using Furion;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Extension;
using Furion.Extras.Admin.NET.Options;
using Furion.Extras.Admin.NET.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnceMi.AspNetCore.OSS;
using QMS.Application.System.EventSubscriber;
using Serilog;
using Yitter.IdGenerator;

namespace QMS.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBStyle(c => c.UseDefault());
            services.AddConfigurableOptions<RefreshTokenSettingOptions>();
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
            services.AddCorsAccessor();
            services.AddRemoteRequest();
            services.AddControllersWithViews().AddMvcFilter<RequestActionFilter>().AddNewtonsoftJson(options =>
            {
                // 首字母小写(驼峰样式)
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                // 时间格式化
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // 忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 忽略空值
                // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            }).AddInjectWithUnifyResult<XnRestfulResultProvider>();
            services.AddViewEngine();
            services.AddSignalR();


            // 注册EventBus服务
            services.AddEventBus(builder =>
            {
                // 注册 Log 日志订阅者
                builder.AddSubscriber<LogEventSubscriber>();
                builder.AddSubscriber<NoticeEventSubscriber>();
            });

            if (App.Configuration["Cache:CacheType"] == "RedisCache")
            {
                //框架原注册StackExchangeRedisCache服务
                //services.AddStackExchangeRedisCache(options =>
                //{
                //    options.Configuration = App.Configuration["Cache:RedisConnectionString"]; // redis连接配置
                //     options.InstanceName = App.Configuration["Cache:InstanceName"]; // 键名前缀
                // });

                services.UseCsRedis();
            }


            //// default minio
            //// 添加默认对象储存配置信息
            //services.AddOSSService(option =>
            //{
            //    option.Provider = OSSProvider.Minio;
            //    option.Endpoint = "oss.oncemi.com:9000";
            //    option.AccessKey = "Q*************9";
            //    option.SecretKey = "A**************************Q";
            //    option.IsEnableHttps = true;
            //    option.IsEnableCache = true;
            //});

            // aliyun oss
            // 添加名称为‘aliyunoss’的OSS对象储存配置信息
            services.AddOSSService("aliyunoss", option =>
            {
                option.Provider = OSSProvider.Aliyun;
                option.Endpoint = "oss-cn-hangzhou.aliyuncs.com";
                option.AccessKey = "L*******************U";
                option.SecretKey = "5*******************************T";
                option.IsEnableCache = true;
            });

            //// qcloud oss
            //// 从配置文件中加载节点为‘OSSProvider’的配置信息
            //services.AddOSSService("QCloud", "OSSProvider");

            //.net6下使用Npgsql数据库时使用以下2行配置
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);//启用遗留时间戳行为
            //AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);//禁用日期时间无限转换
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //  NGINX 反向代理获取真实IP
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // 添加状态码拦截中间件
            app.UseUnifyResultStatusCodes();

            app.UseHttpsRedirection(); // 强制https
            app.UseStaticFiles();

            // Serilog请求日志中间件---必须在 UseStaticFiles 和 UseRouting 之间
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hubs/chathub");

                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // 设置雪花Id的workerId，确保每个实例workerId都应不同
            var workerId = ushort.Parse(App.Configuration["SnowId:WorkerId"] ?? "1");
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = workerId });

            // 开启自启动定时任务
            App.GetService<ISysTimerService>().StartTimerJob();
        }
    }
}
