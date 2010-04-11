// <copyright file="ITestItem.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-03-31</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-03-31</date>
// </editor>
// <summary>Test item interface.</summary>

using System.Collections.Generic;
using Caliburn.PresentationFramework.RoutedMessaging;

namespace Fab.Client.Models
{
	/// <summary>
	/// Screen part service.
	/// </summary>
	public interface IPart
	{
		string DisplayName { get; }

		IEnumerable<IResult> Enter();
	}
}