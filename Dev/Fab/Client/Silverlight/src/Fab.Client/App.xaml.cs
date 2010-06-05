using System.Globalization;
using System.Net;
using System.Net.Browser;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
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
			// with SOAP Faults xml information returned from WCF service in case of server side error.
			// Details: http://blogs.msdn.com/carlosfigueira/archive/2009/08/15/fault-support-in-silverlight-3.aspx
			// Note: should always return "true", unless the prefix has been previously registered.
			WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
			Startup += (sender, args) =>
			           	{
			           		// Ensure the current culture passed into bindings 
			           		// is the OS culture. By default, WPF uses en-US 
			           		// as the culture, regardless of the system settings.
//			           		FrameworkElement.LanguageProperty.OverrideMetadata(
//			           			typeof (FrameworkElement),
//			           			new FrameworkPropertyMetadata(
//			           				XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

							
			           		var culture = CultureInfo.CurrentCulture;
			           		var cultureUI = CultureInfo.CurrentUICulture;
			           		var lp = FrameworkElement.LanguageProperty;
							FrameworkElement.Language = new XmlLanguage();
//			           		Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentCulture;
//			           		var l = XmlLanguage.IetfLanguageTag;
//			           		var a = lp.GetMetadata(this);
			           	};
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