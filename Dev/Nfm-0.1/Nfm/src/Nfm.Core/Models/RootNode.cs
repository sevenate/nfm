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
using System.Linq;

namespace Nfm.Core.Models
{
	/// <summary>
	/// Provide access to nodes tree.
	/// </summary>
	public class RootNode : INode
	{
		/// <summary>
		/// All registered root nodes (modules)
		/// </summary>
		private readonly List<INode> modules = new List<INode>();

		/// <summary>
		/// General nodes key separator in string path.
		/// </summary>
		public static readonly string Separator = @"\";

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

		#region Implementation of INode

		/// <summary>
		/// Gets node display name.
		/// </summary>
		public string DisplayName
		{
			get { return "Root"; }
		}

		/// <summary>
		/// Gets parent node.
		/// </summary>
		public INode Parent
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all registered root nodes (modules).
		/// </summary>
		public IEnumerable<INode> Childs
		{
			get { return modules; }
		}

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over node attributes.
		/// </summary>
		public IEnumerable<INodeAttribute> Attributes
		{
			get
			{
				// TODO: add support of INodeAttribute
				yield break;
			}
		}

		/// <summary>
		/// Gets or sets unique node identification key.
		/// </summary>
		public string Key
		{
			get { return @"\"; }
			set { throw new NotSupportedException("Root node key is constant."); }
		}

		#endregion

		/// <summary>
		/// Register new node in the system.
		/// </summary>
		/// <param name="node">New node element.</param>
		public void RegisterNode(INode node)
		{
			if (!modules.Contains(node))
			{
				modules.Add(node);
			}
		}

		/// <summary>
		/// Unregister node from the system.
		/// </summary>
		/// <param name="node">Registered node element.</param>
		public void UnregisterNode(INode node)
		{
			if (modules.Contains(node))
			{
				modules.Remove(node);
			}
		}

		/// <summary>
		/// Get specific node in the tree according to provided string path to it.
		/// </summary>
		/// <param name="pathToNode">Path to node.</param>
		/// <returns>Correspond <see cref="INode"/>.</returns>
		public INode GetNode(string pathToNode)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Get specific module by it unique key.
		/// </summary>
		/// <param name="key">Module unique key.</param>
		/// <returns>Specific module or null if not found.</returns>
		public INode GetModule(string key)
		{
			return
				modules.Where(node => node.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Single();
		}
	}
}