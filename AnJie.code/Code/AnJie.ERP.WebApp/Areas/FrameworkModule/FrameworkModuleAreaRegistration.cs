using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.FrameworkModule
{
    public class FrameworkModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FrameworkModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "AnJie.ERP.WebApp.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
