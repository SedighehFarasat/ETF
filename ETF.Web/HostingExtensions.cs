using System.Net.Http.Headers;

namespace EtfAnalyzer.Web;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient(name: "CapitalMarketDataWebApi",
            configureClient: options =>
            {
                options.BaseAddress = new Uri("http://localhost:5000");
                options.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue(mediaType: "application/json", quality: 1.0));
            });

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        return app;
    }
}
