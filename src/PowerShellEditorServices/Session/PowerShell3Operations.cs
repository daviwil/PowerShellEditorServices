//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.EditorServices.Session
{
    internal class PowerShell3Operations : IVersionSpecificOperations
    {
        public void ConfigureDebugger(Runspace runspace)
        {
            // The debugger has no SetDebugMode in PowerShell v3.
        }

        public void PauseDebugger(Runspace runspace)
        {
            // The debugger cannot be paused in PowerShell v3.
            throw new NotSupportedException("Debugger cannot be paused in PowerShell v3");
        }

        public ExecutionResult ExecuteCommandInDebugger(
            PowerShellContext powerShellContext,
            Runspace currentRunspace,
            ExecutionRequest executionRequest)
        {
            Collection<PSObject> output = null;

            using (var nestedPipeline = currentRunspace.CreateNestedPipeline())
            {
                foreach (var command in executionRequest.Command.Commands)
                {
                    nestedPipeline.Commands.Add(command);
                }

                output = nestedPipeline.Invoke();
            }

            // Write the output to the host if necessary
            if (executionRequest.Options.WriteOutputToHost && output != null)
            {
                foreach (var line in output)
                {
                    powerShellContext.WriteOutput(line.ToString(), true);
                }
            }

            executionRequest.Result.SetOutput(output);

            return executionResult;
        }
    }
}

