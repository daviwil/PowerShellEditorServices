//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.EditorServices.Session
{
    internal class PowerShell4Operations : IVersionSpecificOperations
    {
        public void ConfigureDebugger(Runspace runspace)
        {
#if !PowerShellv3
            if (runspace.Debugger != null)
            {
                runspace.Debugger.SetDebugMode(DebugModes.LocalScript | DebugModes.RemoteScript);
            }
#endif
        }

        public virtual void PauseDebugger(Runspace runspace)
        {
            // The debugger cannot be paused in PowerShell v4.
            throw new NotSupportedException("Debugger cannot be paused in PowerShell v4");
        }

        public ExecutionResult ExecuteCommandInDebugger(
            PowerShellContext powerShellContext,
            Runspace currentRunspace,
            ExecutionRequest executionRequest)
        {
            ExecutionResult executionResult = new ExecutionResult(executionRequest);
            PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();

#if !PowerShellv3
            if (executionRequest.Options.WriteOutputToHost)
            {
                outputCollection.DataAdded +=
                    (obj, e) =>
                    {
                        for (int i = e.Index; i < outputCollection.Count; i++)
                        {
                            powerShellContext.WriteOutput(
                                outputCollection[i].ToString(),
                                true);
                        }
                    };
            }

            DebuggerCommandResults commandResults =
                currentRunspace.Debugger.ProcessCommand(
                    executionRequest.Command,
                    outputCollection);
#endif

            executionResult.SetOutput(outputCollection);
            executionResult.DebuggerAction = commandResults.ResumeAction;

            return executionResult;
        }
    }
}

