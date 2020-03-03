// -----------------------------------------------------------------------
// <copyright file="SueetieTask.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Threading;
    using System.Timers;
    using System.Xml;
    using System.Xml.Linq;
    using Timer = System.Timers.Timer;

    public class SueetieTask
    {
        Timer timer;

        private ISueetieTask _itask;
        public string Name { get; set; }

        public Type TaskType { get; set; }
        public bool IsRunning { get; set; }
        public bool Enabled { get; set; }

        public DateTime LastRunStart { get; set; }
        public DateTime LastRunEnd { get; set; }
        public bool IsLastRunSuccessful { get; set; }
        public double Minutes { get; set; }

        public double Interval { get; set; }
        public bool Stopped { get; set; }

        public XmlNode ConfigurationNode { get; set; }
        public XElement XTaskElement { get; set; }

        public SueetieTask(double interval)
        {
            this.Interval = interval;
            this.Initialize();
        }

        public void Start()
        {
            this.Stopped = false;
            this.StartTask();
        }

        public void Stop()
        {
            this.Stopped = true;
        }

        private void Initialize()
        {
            this.Stopped = false;

            this.timer = new Timer(this.Interval);
            this.timer.Elapsed += this.timer_Elapsed;
            this.timer.Enabled = true;
        }

        private void StartTask()
        {
            if (!this.Stopped)
            {
                var thread = new Thread(this.ExecuteTask);
                thread.Start();
            }
        }

        internal ISueetieTask CreateTaskInstance()
        {
            if (this.Enabled && (this._itask == null))
            {
                if (this.TaskType != null)
                {
                    this._itask = Activator.CreateInstance(this.TaskType) as ISueetieTask;
                }
                this.Enabled = this._itask != null;
            }
            return this._itask;
        }

        internal void ExecuteTask()
        {
            this.IsRunning = true;
            var task = this.CreateTaskInstance();
            if (task != null)
            {
                this.LastRunStart = DateTime.Now;
                try
                {
                    task.Execute(this.XTaskElement);
                    this.IsLastRunSuccessful = true;
                }
                catch (Exception exception)
                {
                    this.IsLastRunSuccessful = false;
                    SueetieLogs.LogTaskException("Task Exception: " + exception.Message);
                }
                finally
                {
                    this.LastRunEnd = DateTime.Now;
                }
            }
            this.IsRunning = false;
        }


        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!this.IsRunning)
                this.StartTask();
        }
    }
}