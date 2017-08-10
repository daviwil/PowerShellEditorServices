//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices.VSCode.Tasks
{
    public enum TaskType
    {
        None,

        Clean,

        Build,

        Rebuild,

        Test
    }

    public class TaskEntry
    {
        /// <summary>
        /// Gets or sets the task's type, used for grouping
        /// similar tasks in the editor.
        /// </summary>
        public TaskType Type { get; set; }

        /// <summary>
        /// Gets or sets the task name that is displayed to the
        /// user.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Gets or sets the name of the task's source, usually
        /// the name of the module which provides the task.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the line of PowerShell code that will
        /// be executed when this task is invoked.
        /// </summary>
        public string Command { get; set; }
   }

    /// <summary>
    /// Defines the contract for a task provider.
    /// </summary>
    public interface ITaskProvider : IFeatureProvider
    {
        Task<TaskDetails> ProvideTasks();
    }
}
