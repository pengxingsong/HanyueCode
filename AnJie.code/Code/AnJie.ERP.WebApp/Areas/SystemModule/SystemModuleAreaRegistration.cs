using System.Web.Mvc;

namespace AnJie.ERP.WebApp.Areas.SystemModule
{
    public class SystemModuleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemModule";
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
