//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Utility;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices
{
    public class ExecutionResult
    {
        #region Private Fields

        private TaskCompletionSource<IEnumerable<PSObject>> resultTask =
            new TaskCompletionSource<IEnumerable<PSObject>>();

        #endregion

        #region Public Properties

        public ExecutionRequest Request { get; private set; }

        public ExecutionRequestState State { get; internal set; }

        public IEnumerable<PSObject> Result { get; private set; }

        public Task<IEnumerable<PSObject>> ResultAsync { get { return this.resultTask.Task;  } }

        public DebuggerResumeAction? DebuggerAction { get; internal set; }

        public StringBuilder Errors { get; internal set; }

        #endregion

        #region Constructors

        public ExecutionResult(ExecutionRequest executionRequest)
        {
            this.Request = executionRequest;
            this.Errors = new StringBuilder();
        }

        #endregion

        #region Public Methods

        public IEnumerable<TResult> GetResultOfType<TResult>()
        {
            return this.CastOutput<TResult>(this.Result);
        }

        public async Task<IEnumerable<TResult>> GetResultOfTypeAsync<TResult>()
        {
            var output = await this.ResultAsync;

            return this.CastOutput<TResult>(output);
        }

        internal void SetOutput(IEnumerable<PSObject> output)
        {
            // TODO: Resolve task
            this.resultTask.TrySetResult(output);
            this.Result = output;
        }

        #endregion

        #region Events


        #endregion

        #region Private Methods

        private IEnumerable<TResult> CastOutput<TResult>(IEnumerable<PSObject> output)
        {
            if (typeof(TResult) != typeof(PSObject))
            {
                return
                    output
                        .Select(pso => pso.BaseObject)
                        .Cast<TResult>();
            }

            return output.Cast<TResult>();
        }

        #endregion
    }
}
