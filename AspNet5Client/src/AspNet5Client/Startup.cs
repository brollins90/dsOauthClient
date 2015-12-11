namespace AspNet5Client
{
    using Microsoft.AspNet.Authentication.Cookies;
    using Microsoft.AspNet.Builder;
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System.IdentityModel.Tokens.Jwt;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
            services.AddMvc();
            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug(LogLevel.Debug);
            loggerFactory.AddConsole(LogLevel.Debug);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseIISPlatformHandler();

            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;
                options.AuthenticationScheme = "Cookies";
            });

            app.UseOAuthAuthentication(options =>
            {
                options.AutomaticChallenge = true;
                options.AuthenticationScheme = "DSAUTH";
                options.DisplayName = "DSAUTH";
                options.SignInScheme = "Cookies";

                options.ClientId = "clientid1";
                options.ClientSecret = "xoxoxo";
                options.CallbackPath = new PathString("/login/oauthcallback");

                //options.AuthorizationEndpoint = "http://ds.transvec.com/oauth/authorize";
                //options.TokenEndpoint = "http://ds.transvec.com/oauth/token";
                options.AuthorizationEndpoint = "http://localhost:8080/oauth/authorize";
                options.TokenEndpoint = "http://localhost:8080/oauth/token";
                options.Scope.Add("email");
            });

            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}