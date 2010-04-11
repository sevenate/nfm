using System.Net;
using System.Net.Browser;
using System.Reflection;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.PresentationFramework.Configuration;
using Caliburn.PresentationFramework.Views;
using Caliburn.ShellFramework;
using Caliburn.ShellFramework.History;
using Caliburn.Unity;
using Fab.Client.Models;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace Fab.Client
{
	public partial class App
	{
		private IUnityContainer container;

		public App()
		{
			InitializeComponent();

			// Required for support HTTP response code 500 (Internal Server Error)
			// with SOAP Faults xml informarmation returned from WCF service in case of server side error.
			// Details: http://blogs.msdn.com/carlosfigueira/archive/2009/08/15/fault-support-in-silverlight-3.aspx
			// Note: should always return "true", unless the prefix has been previously registered.
			WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
		}

		protected override IServiceLocator CreateContainer()
		{
			container = new UnityContainer();
			return new UnityAdapter(container);
		}

		protected override Assembly[] SelectAssemblies()
		{
			return new[] {Assembly.GetExecutingAssembly()};
		}

		protected override object CreateRootModel()
		{
			return Container.GetInstance<IShell>();
		}

		protected override void ConfigurePresentationFramework(PresentationFrameworkConfiguration module)
		{
			module.With.ShellFramework()
				.ConfigureDeepLinking<DeepLinkStateManager, DefaultHistoryCoordinator>()
				.RedirectViewNamespace("Fab.Client.Shell.Views");

			module.RegisterAllScreensWithSubjects(true)
				.Using(x => x.ViewLocator<DefaultViewLocator>())
				.Configured(x => x.AddNamespaceAlias("Fab.Client.Models", "Fab.Client.Shell.Views"));
		}
	}
}