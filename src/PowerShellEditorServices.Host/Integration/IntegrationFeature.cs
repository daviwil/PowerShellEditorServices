//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Components;
using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol;
using Microsoft.PowerShell.EditorServices.Protocol.LanguageServer;
using Microsoft.PowerShell.EditorServices.Utility;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using PS = System.Management.Automation;
using Servers = Microsoft.PowerShell.EditorServices.Protocol.Server;

namespace Microsoft.PowerShell.EditorServices.Symbols
{
    public class CommandParameter
    {
        public string Name { get; set; }

        public JToken Value { get; set; }
    }

    public class Command
    {
        public string CommandText { get; set; }

        public CommandParameter[] Parameters { get; set; }
    }

    public class PSCommand
    {
        public Command[] Commands { get; set; }
    }

    public class InvokePSCommandRequest
    {
        public static readonly
            RequestType<PSCommand, InvokePSCommandResponse, object, object> Type =
            RequestType<PSCommand, InvokePSCommandResponse, object, object>.Create("powerShell/invokePSCommand");
    }

    public class InvokePSCommandResponse
    {
        public JArray Output { get; set; }

        public string[] Errors { get; set; }
    }

    internal class IntegrationFeature :
        FeatureComponentBase<IDocumentSymbolProvider>,
        IDocumentSymbols
    {
        private EditorSession editorSession;

        public IntegrationFeature(
            EditorSession editorSession,
            IMessageHandlers messageHandlers,
            ILogger logger)
                : base(logger)
        {
            this.editorSession = editorSession;

            messageHandlers.SetRequestHandler(
                InvokePSCommandRequest.Type,
                this.HandleInvokePSCommandRequest);
        }

        private async Task HandleInvokePSCommandRequest(
            PSCommand PSCommandParam,
            RequestContext<InvokePSCommandResponse> requestContext)
        {
            PS.PSCommand psCommand = new PS.PSCommand();

            foreach (Command command in PSCommandParam.Commands)
            {
                psCommand.AddCommand(command.CommandText);

                foreach (CommandParameter parameter in command.Parameters)
                {
                    if (parameter.Name != null)
                    {
                        psCommand.AddParameter(parameter.Name, parameter.Value?.ToString());
                    }
                    else
                    {
                        psCommand.AddArgument(parameter.Name);
                    }
                }
            }

            var results =
                await this.editorSession.PowerShellContext.ExecuteCommand<PS.PSObject>(
                    psCommand,
                    false,
                    true);

            await requestContext.SendResult(
                new InvokePSCommandResponse
                {
                    Output =
                        JArray.FromObject(
                            results
                                .Select(r => JObject.FromObject(r))
                                .ToArray())
                });
        }

        public static IntegrationFeature Create(
            IComponentRegistry components,
            EditorSession editorSession)
        {
            return
                new IntegrationFeature(
                    editorSession,
                    components.Get<IMessageHandlers>(),
                    components.Get<ILogger>());
        }

        public IEnumerable<SymbolReference> ProvideDocumentSymbols(
            ScriptFile scriptFile)
        {
            return
                this.InvokeProviders(p => p.ProvideDocumentSymbols(scriptFile))
                    .SelectMany(r => r);
        }

        protected async Task HandleDocumentSymbolRequest(
            DocumentSymbolParams documentSymbolParams,
            RequestContext<SymbolInformation[]> requestContext)
        {
            ScriptFile scriptFile =
                editorSession.Workspace.GetFile(
                    documentSymbolParams.TextDocument.Uri);

            IEnumerable<SymbolReference> foundSymbols =
                this.ProvideDocumentSymbols(scriptFile);

            SymbolInformation[] symbols = null;

            string containerName = Path.GetFileNameWithoutExtension(scriptFile.FilePath);

            if (foundSymbols != null)
            {
                symbols =
                    foundSymbols
                        .Select(r =>
                            {
                                return new SymbolInformation
                                {
                                    ContainerName = containerName,
                                    Kind = Servers.LanguageServer.GetSymbolKind(r.SymbolType),
                                    Location = new Location
                                    {
                                        Uri = Servers.LanguageServer.GetFileUri(r.FilePath),
                                        Range = Servers.LanguageServer.GetRangeFromScriptRegion(r.ScriptRegion)
                                    },
                                    Name = Servers.LanguageServer.GetDecoratedSymbolName(r)
                                };
                            })
                        .ToArray();
            }
            else
            {
                symbols = new SymbolInformation[0];
            }

            await requestContext.SendResult(symbols);
        }
    }
}
