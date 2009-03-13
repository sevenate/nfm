// <copyright file="INode.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-09</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-09</date>
// </editor>
// <summary>Represent regulary node element in tree structure.</summary>

using System.Collections.Generic;

namespace Nfm.Core.Models
{
	/// <summary>
	/// Represent regulary node element in tree structure.
	/// </summary>
	public interface INode
	{
		/// <summary>
		/// Gets node display name.
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// Gets parent node.
		/// </summary>
		INode Parent { get; }

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over all child nodes.
		/// </summary>
		IEnumerable<INode> Childs { get; }

		/// <summary>
		/// Gets the enumerator, which supports a simple iteratetion over node attributes.
		/// </summary>
		IEnumerable<INodeAttribute> Attributes { get; }
	}
}