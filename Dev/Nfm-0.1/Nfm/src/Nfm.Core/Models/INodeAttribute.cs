// <copyright file="INodeAttribute.cs" company="HD">
// 	Copyright (c) 2009 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2009-01-10</date>
// </editor>
// <summary>Represent regulary attribute of the node.</summary>

namespace Nfm.Core
{
	/// <summary>
	/// Represent regulary attribute of the node.
	/// </summary>
	public interface INodeAttribute
	{
		/// <summary>
		/// Gets attribute name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets attribute type.
		/// </summary>
		NodeAttributeValueType ValueType { get; }

		/// <summary>
		/// Gets attribute value.
		/// </summary>
		object Value { get; }
	}
}