using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuotationAppv1.Startup))]
namespace QuotationAppv1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
