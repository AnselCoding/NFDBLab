using Microsoft.Owin;
using NSwag.AspNet.Owin;
using Owin;
using System.Web.Http;

// 指定了應用程序的 OwinStartup 類型。這意味著當應用程序啟動時，將自動執行這個 Startup 類
[assembly: OwinStartup(typeof(NFDBLab.Startup))]

namespace NFDBLab
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (System.Web.Hosting.HostingEnvironment.IsDevelopmentEnvironment)
            {
                ConfigureSwagger(app);
            }

        }

        private static void ConfigureSwagger(IAppBuilder app)
        {
            // 創建一個 HttpConfiguration 實例，用於設定 Web API 的設置和路由。
            var config = new HttpConfiguration();
            // 將 Swagger UI（用於生成和測試 API 文檔的工具）集成到應用程序中。
            app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
            {
                //針對RPC-Style WebAPI，指定路由包含Action名稱
                settings.GeneratorSettings.DefaultUrlTemplate =
                    "api/{controller}/{action}/{id?}";
                //可加入客製化調整邏輯
                settings.PostProcess = document =>
                {
                    document.Info.Title = "NFDBLab Web API";
                };
            });
            // 將配置好的 HttpConfiguration 應用到 OWIN 的中間件管道中，以便處理 HTTP 請求。
            app.UseWebApi(config);
            // 啟用使用屬性路由的 Web API 路由設置
            config.MapHttpAttributeRoutes();
            // 確保配置初始化完畢
            config.EnsureInitialized();
        }
    }
}