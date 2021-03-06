//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol;

namespace Microsoft.PowerShell.EditorServices.Protocol.DebugAdapter
{
    public class TerminatedEvent
    {
        public static readonly
            EventType<TerminatedEvent> Type =
            EventType<TerminatedEvent>.Create("terminated");

        public bool Restart { get; set; }
    }
}

