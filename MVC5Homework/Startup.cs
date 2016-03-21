using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC5Homework.Startup))]
namespace MVC5Homework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
