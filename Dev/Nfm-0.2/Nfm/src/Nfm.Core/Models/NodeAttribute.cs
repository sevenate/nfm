// <copyright file="NodeAttribute.cs" company="HD">
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
// <summary>Node attribute.</summary>

namespace Nfm.Core
{
	/// <summary>
	/// Node attribute.
	/// </summary>
	public class NodeAttribute : INodeAttribute
	{
		#region Implementation of INodeAttribute

		/// <summary>
		/// Gets attribute name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets attribute type.
		/// </summary>
		public NodeAttributeValueType ValueType { get; set; }

		/// <summary>
		/// Gets or sets attribute value.
		/// </summary>
		public object Value { get; set; }

		#endregion
	}
}