// -----------------------------------------------------------------------
// <copyright file="ISueetieTask.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Xml.Linq;

    public interface ISueetieTask
    {
        void Execute(XElement XTaskElement);
    }
}