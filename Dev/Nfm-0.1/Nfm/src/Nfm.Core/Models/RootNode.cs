// <copyright file="RootNode.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-21</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-04-21</date>
// </editor>
// <summary>Provide access to nodes tree.</summary>

using System;
using System.Collections.Generic;
using Nfm.Core.ViewModels;

namespace Nfm.Core.Models
{
	/// <summary>
	/// Provide access to nodes tree.
	/// </summary>
	public class RootNode
	{
		/// <summary>
		/// General nodes key separator in string path.
		/// </summary>
		public static readonly char Separator = '\\';

		/// <summary>
		/// All registered root modules and they default view models.
		/// </summary>
		private readonly IDictionary<IRootModule, IDefaultModuleViewModel> childs =
			new Dictionary<IRootModule, IDefaultModuleViewModel>();

		#region Singleton

		/// <summary>
		/// Singleton instance.
		/// </summary>
		private static RootNode inst;

		/// <summary>
		/// Prevents a default instance of the <see cref="RootNode"/> class from being created.
		/// </summary>
		private RootNode()
		{
		}

		/// <summary>
		/// Gets singleton instance.
		/// </summary>
		public static RootNode Inst
		{
			get
			{
				if (inst == null)
				{
					inst = new RootNode();
				}

				return inst;
			}
		}

		#endregion Singleton

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all registered root modules.
		/// </summary>
		public IEnumerable<KeyValuePair<IRootModule, IDefaultModuleViewModel>> Childs
		{
			get { return childs; }
		}

		/// <summary>
		/// Gets unique node identification key.
		/// </summary>
		public string Key
		{
			get { return @"\"; }
		}

		/// <summary>
		/// Register new root module in the system.
		/// </summary>
		/// <param name="node">New root module element.</param>
		/// <param name="defaultVM">Default view model, used to display root module.</param>
		public void RegisterNode(IRootModule node, IDefaultModuleViewModel defaultVM)
		{
			if (!childs.Keys.Contains(node))
			{
				childs.Add(node, defaultVM);
			}
		}

		/// <summary>
		/// Unregister node from the system.
		/// </summary>
		/// <param name="node">Registered node element.</param>
		public void UnregisterNode(IRootModule node)
		{
			if (childs.Keys.Contains(node))
			{
				childs.Remove(node);
			}
		}

		/// <summary>
		/// Get default node view model in the tree according to provided absolute string path to it.
		/// </summary>
		/// <param name="pathToNode">Path to node.</param>
		/// <returns>Correspond <see cref="IViewModel"/> if found or null otherwise.</returns>
		public IViewModel GetNode(string pathToNode)
		{
			string moduleKey = pathToNode.Split(Separator)[1];
			string pathInModule = pathToNode.Substring(pathToNode.IndexOf(Separator, 1) + 1);

			foreach (var module in childs)
			{
				if (module.Key.Key.Equals(moduleKey, StringComparison.InvariantCultureIgnoreCase))
				{
					return module.Value.GetChildViewModel(pathInModule);
				}
			}

			throw new NotSupportedException("Specified path to node is not valid or corresponding module is not registered.");
		}
	}
}