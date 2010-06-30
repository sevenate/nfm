using Fab.Client.Models;
using Fab.Client.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Caliburn.Core.Configuration;
using Caliburn.Core.IoC;

namespace Fab.Client.Shell
{
	public class Configuration : ModuleBase
	{
		public override IEnumerable<IComponentRegistration> GetComponents()
		{
			yield return Singleton<IShell, ShellViewModel>();

			var moduleTypes = from type in Assembly.GetExecutingAssembly().GetExportedTypes()
							  where typeof(IPart).IsAssignableFrom(type)
									&& !type.IsAbstract && !type.IsInterface
							  select type;

			foreach (var type in moduleTypes)
			{
				yield return Singleton(typeof(IPart), type, type.FullName);
			}
		}

		private new static IComponentRegistration Singleton(Type service, Type implementation, string name)
		{
			return new Singleton
			{
				Service = service,
				Implementation = implementation,
				Name = name
			};
		}
	}
}