namespace WebBff.Extensions
{
    public static class SystemsManagerExtensions
    {
        public static void ConfigureSystemsManager(this WebApplicationBuilder builder)
        {
            builder.Configuration
                .AddSystemsManager($"/common")
                .AddSystemsManager($"/gallilearn-webbff");
        }
    }
}
