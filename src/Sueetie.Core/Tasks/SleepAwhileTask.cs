// -----------------------------------------------------------------------
// <copyright file="SleepAwhileTask.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core.Tasks
{
    using System.Threading;
    using System.Xml.Linq;

    public class SleepAwhileTask : ISueetieTask
    {
        public void Execute(XElement _xTaskElement)
        {
            Thread.Sleep(10000);
        }
    }
}