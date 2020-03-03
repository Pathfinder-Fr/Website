// -----------------------------------------------------------------------
// <copyright file="WriteSqlRecordTask.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core.Tasks
{
    using System.Xml.Linq;

    public class WriteSqlRecordTask : ISueetieTask
    {
        public void Execute(XElement _xTaskElement)
        {
            var hour = (int)_xTaskElement.Attribute("hour");
            SueetieCommon.TestTaskEntry();
        }
    }
}