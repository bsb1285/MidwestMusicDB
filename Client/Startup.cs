using System;
using System.Net.Http;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace MidwestMusicDB.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            
        }
        
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
            
        }
    }
}
