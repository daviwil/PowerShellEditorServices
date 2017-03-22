//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Utility;
using System.Management.Automation;

namespace Microsoft.PowerShell.EditorServices
{
    public class ExecutionRequest
    {
        #region Properties

        public PSCommand Command { get; private set; }

        public ExecutionRequestOptions Options { get; private set; }

        #endregion

        #region Constructors

        internal ExecutionRequest(
            PSCommand command,
            ExecutionRequestOptions executionOptions)
        {
            Validate.IsNotNull(nameof(executionOptions), executionOptions);

            this.Command = command;
            this.Options = executionOptions;
        }

        #endregion
    }
}
