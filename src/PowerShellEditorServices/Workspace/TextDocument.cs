
using System;

namespace Microsoft.PowerShell.EditorServices
{
    public enum FileType
    {
        Untitled,
        InMemory,
        FilePath
    }

    public class FileUri
    {
        public string Uri { get; private set; }

        public bool IsUntitled { get; private set; }
    }

    public class TextDocument
    {
        public bool IsSaved { get; private set; }

        public void Save()
        {

        }

        public void Close()
        {

        }

        public void ApplyChange(FileChange fileChange)
        {

        }

    }

    /*
    
    - By default, all actions go to editor: save, close, edit. 
    - Actions take place in the editor, get sent back to server
    - When server receives message, raises event
    - Who needs to be aware?

    */

    public class EditorWorkspace : IWorkspace
    {
        public void CloseFile(IWorkspaceFile fileBuffer)
        {
            throw new NotImplementedException();
        }

        public IFileBuffer OpenFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }


    public interface IWorkspace2
    {

    }

    /*
    
    Sequence 2:
    - PowerShell code makes edit to a file: $context.CurrentFile.InsertText("Boo!", 1, 1)
    - CurrentFile is an EditorFile which implements IWorkspaceFile
    - EditorFile.InsertText sends a request to the editor to change file contents
    - Editor sends a request to change file contents in the server
    - Local in-memory buffer for this file must be updated

    What if we have a set of interfaces plus a file wrapper class that
    abstracts the usage?

    - FileBuffer takes IFileOperations and FileDetails (local, remote, in-memory)
    - Workspace implementation knows how to create correct FileBuffer based on context
    - FileBuffer is now where you do edits

    */

    public interface IFileOperations
    {
        void ApplyChange(FileChange fileChange);

        void Save();

        void Close();
    }

    internal class EditorFile : IWorkspaceFile
    {
        public void ApplyChange(FileChange fileChange)
        {
            // Send request
        }

        public void Close()
        {
            // Send request
        }

        public void Save()
        {
            // Send request
        }
    }

    internal class LocalFile : IWorkspaceFile
    {
        public void ApplyChange(FileChange fileChange)
        {
            // Edit buffer directly
        }

        public void Close()
        {
            // Do nothing, really
        }

        public void Save()
        {
            // Save the file contents to local machine
        }
    }

    internal class RemoteFile : IWorkspaceFile
    {
        public void ApplyChange(FileChange fileChange)
        {
            // Edit buffer directly
        }

        public void Close()
        {
            // Do nothing, really.  Clean up anything?  What about notifying editor?
        }

        public void Save()
        {
            // Save the file contents to local machine
        }
    }
}
