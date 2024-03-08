using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(Inventory360API_V2.Startup))]

namespace Inventory360API_V2
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            // enable cors origin request
            app.UseCors(CorsOptions.AllowAll);

            var myProvider = new MyAuthorizationServerProvider();
            // https://www.c-sharpcorner.com/UploadFile/ff2f08/angularjs-enable-owin-refresh-tokens-using-asp-net-web-api/
            // http://bitoftech.net/2014/07/16/enable-oauth-refresh-tokens-angularjs-app-using-asp-net-web-api-2-owin/
            var myRefreshTokenProvider = new MyAuthorizationServerRefreshTokenProvider();

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = myProvider,
                //RefreshTokenProvider = myRefreshTokenProvider
            };

            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}