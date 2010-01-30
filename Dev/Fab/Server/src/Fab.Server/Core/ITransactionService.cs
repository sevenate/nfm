// <copyright file="ITransactionService.cs" company="HD">
// 	Copyright (c) 2010 HD. All rights reserved.
// </copyright>
// <author name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </author>
// <editor name="Andrew Levshoff">
// 	<email>alevshoff@hd.com</email>
// 	<date>2010-01-28</date>
// </editor>
// <summary>Transaction service.</summary>

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Fab.Server.Core
{
    /// <summary>
    /// Transaction service.
    /// </summary>
    [ServiceContract]
    public interface ITransactionService
    {
        /// <summary>
        /// Gets all available asset types (i.e. "currency names").
        /// </summary>
        /// <returns>Asset types presented in the system.</returns>
        [OperationContract]
        IList<AssetType> GetAllAssetTypes();

        /// <summary>
        /// Gets all available journal types (i.e. "Deposit", "Withdrawal", "Transfer" etc.).
        /// </summary>
        /// <returns>Journal types presented in the system.</returns>
        [OperationContract]
        IList<JournalType> GetAllJournalTypes();
    }
}