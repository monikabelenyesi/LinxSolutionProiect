using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LinxSolutionProiect.Startup))]
namespace LinxSolutionProiect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
