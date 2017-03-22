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
            ExecutionResult executionResult = new ExecutionResult(executionRequest);

            using (var nestedPipeline = currentRunspace.CreateNestedPipeline())
            {
                foreach (var command in executionRequest.Command.Commands)
                {
                    nestedPipeline.Commands.Add(command);
                }

                executionResult.SetOutput(nestedPipeline.Invoke());
            }

            // Write the output to the host if necessary
            if (executionRequest.Options.WriteOutputToHost)
            {
                foreach (var line in executionResult.Result)
                {
                    powerShellContext.WriteOutput(line.ToString(), true);
                }
            }

            return executionResult;
        }
    }
}

