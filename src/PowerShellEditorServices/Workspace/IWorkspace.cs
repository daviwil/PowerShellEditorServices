//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

namespace Microsoft.PowerShell.EditorServices
{
    public interface IWorkspace
    {
        IFileBuffer OpenFile(string filePath);
    }

    public enum WorkspaceFileChangeType
    {
        Opened = 0,
        Changed = 1,
        Saving = 2,
        Saved = 3,
        Closing = 4,
        Closed = 5
    }

    public class WorkspaceFileChangedEventArgs
    {
        public WorkspaceFileChangeType ChangeType { get; private set; }

        public IFileBuffer File { get; private set; }
    }
}
