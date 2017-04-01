//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Utility;
using System;
using System.Management.Automation;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices
{
    public class ExecutionRequest
    {
        #region Private Fields

        private TaskCompletionSource<ExecutionResult> executionResultTask =
            new TaskCompletionSource<ExecutionResult>();

        #endregion

        #region Properties

        public PSCommand Command { get; private set; }

        public ExecutionRequestOptions Options { get; private set; }

        public ExecutionRequestState State { get; private set; }

        #endregion

        #region Constructors

        public ExecutionRequest(
            PSCommand command,
            ExecutionRequestOptions executionOptions)
        {
            Validate.IsNotNull(nameof(executionOptions), executionOptions);

            this.Command = command;
            this.Options = executionOptions;
        }

        #endregion

        #region Public Methods

        public void SetResult(ExecutionResult executionResult)
        {
            this.executionResultTask.TrySetResult(executionResult);
            this.OnExecutionStateChanged(ExecutionRequestState.Completed);
        }

        public void SetAborted()
        {
            this.OnExecutionStateChanged(ExecutionRequestState.Aborted);
        }

        public void SetFailed(Exception e)
        {
            this.OnExecutionStateChanged(ExecutionRequestState.Failed);
        }

        public Task<ExecutionResult> GetResultAsync()
        {
            return Task.FromResult<ExecutionResult>(null);
        }

        public ExecutionResult WaitForResult()
        {
            return null;
        }

        #endregion

        #region Events

        public event EventHandler<ExecutionRequestState> ExecutionStateChanged;

        protected void OnExecutionStateChanged(ExecutionRequestState executionState)
        {
            this.State = executionState;
            this.ExecutionStateChanged?.Invoke(this, executionState);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
