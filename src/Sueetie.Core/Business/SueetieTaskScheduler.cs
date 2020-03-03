// -----------------------------------------------------------------------
// <copyright file="SueetieTaskScheduler.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Linq;

    public class SueetieTaskScheduler
    {
        private XDocument TaskConfig;
        private static readonly SueetieTaskScheduler taskScheduler;
        private static List<SueetieTask> _tasks = new List<SueetieTask>();

        static SueetieTaskScheduler()
        {
            taskScheduler = new SueetieTaskScheduler();
        }

        private SueetieTaskScheduler()
        {
        }

        public static SueetieTaskScheduler Instance()
        {
            return taskScheduler;
        }

        public SueetieTaskScheduler(XDocument _taskConfig)
        {
            this.TaskConfig = _taskConfig;
            this.Initialize();
        }

        public void StartTasks()
        {
            foreach (var task in _tasks)
            {
                if (!task.IsRunning)
                    task.Start();
            }
        }

        public void StopTasks()
        {
            foreach (var task in _tasks)
            {
                task.Stop();
            }
        }

        public IList<SueetieTask> Tasks
        {
            get { return new ReadOnlyCollection<SueetieTask>(_tasks); }
        }

        private void Initialize()
        {
            _tasks = new List<SueetieTask>();

            var tasknodes = from tasknode in this.TaskConfig.Descendants("Task")
                select new
                {
                    Minutes = (double)tasknode.Attribute("minutes"),
                    Name = (string)tasknode.Attribute("name"),
                    TaskType = Type.GetType((string)tasknode.Attribute("type"), true),
                    Enabled = (bool)tasknode.Attribute("enabled"),
                    XTaskElement = tasknode
                };

            foreach (var _tasknode in tasknodes)
            {
                try
                {
                    var sueetieTask = new SueetieTask(_tasknode.Minutes * 60000);
                    sueetieTask.Minutes = _tasknode.Minutes;
                    sueetieTask.Name = _tasknode.Name;
                    sueetieTask.TaskType = _tasknode.TaskType;
                    sueetieTask.Enabled = _tasknode.Enabled;
                    sueetieTask.XTaskElement = _tasknode.XTaskElement;
                    _tasks.Add(sueetieTask);
                }
                catch (Exception exception)
                {
                    SueetieLogs.LogTaskException("Task Exception: " + exception.Message);
                }
            }
        }
    }
}