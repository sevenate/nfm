using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fab.Client.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.RouteExistingFiles = false;

			routes.MapRoute(
				"Default",                                              // Route name
				"{action}",                                             // URL with parameters
				new { controller = "Home", action = "Silverlight" }     // Parameter defaults
			);

//			routes.MapRoute(
//				"Default",                                              // Route name
//				"{controller}/{action}/{id}",                           // URL with parameters
//				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
//			);

		}

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);
		}
	}
}