using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NFDBLab
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // 啟用 Cors 跨網域呼叫，需要安裝套件 Microsoft.AspNet.WebApi.Cors。
            //config.EnableCors();

            // 傳輸資料將僅使用 JSON 格式 (移除XML格式)
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // 傳輸資料開頭改小寫
            // config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // 使用 JSON.NET 序列化器，可以更好地處理複雜的型別，例如多態、匿名型別等，並且可以進行更多自定義的序列化和反序列化處理。
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;


            // Web API 路由

            // 啟用使用屬性路由來定義 Web API 控制器的路由。
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
