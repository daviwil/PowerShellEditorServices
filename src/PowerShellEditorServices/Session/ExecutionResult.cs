//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

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

        public IEnumerable<PSObject> Output { get; private set; }

        public DebuggerResumeAction? DebuggerAction { get; internal set; }

        public StringBuilder Errors { get; internal set; }

        #endregion

        #region Constructors

        internal ExecutionResult()
        {
            this.Errors = new StringBuilder();
        }

        #endregion

        #region Public Methods

        public IEnumerable<TResult> GetOutputOfType<TResult>()
        {
            return this.CastOutput<TResult>(this.Output);
        }

        public async Task<IEnumerable<TResult>> GetOutputOfTypeAsync<TResult>()
        {
            var output = await this.resultTask.Task;

            return this.CastOutput<TResult>(output);
        }

        internal void SetOutput(IEnumerable<PSObject> output)
        {
            this.resultTask.TrySetResult(output);
            this.Output = output;
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
