//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

namespace Microsoft.PowerShell.EditorServices
{
    public class ExecutionRequestOptions
    {
        #region Properties

        public bool WriteOutputToHost { get; set; }

        public bool WriteErrorsToHost { get; set; }

        public bool AddToHistory { get; set; }

        #endregion

        #region Constructors

        public ExecutionRequestOptions()
        {
            this.WriteOutputToHost = true;
            this.WriteErrorsToHost = true;
            this.AddToHistory = false;
        }

        #endregion
    }
}
