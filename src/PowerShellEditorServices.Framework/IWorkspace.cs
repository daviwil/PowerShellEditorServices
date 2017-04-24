using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerShellEditorServices.Framework
{
    public interface IWorkspace
    {
        IFileBuffer OpenFile(string filePath);

        CloseFile(IFileBuffer fileBuffer);
    }

    public enum WorkspaceFileChangeType
    {
        Opened,
        Changed,
        Saving,
        Saved,
        Closing,
        Closed
    }

    public class WorkspaceFileChangedEventArgs
    {
        public WorkspaceFileChangeType ChangeType { get; private set; }

        public bool Cancel { get; private set; }

        public IFileBuffer File { get; private set; }
    }
}
