// -----------------------------------------------------------------------
// <copyright file="SueetieClassifieds.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    public static class SueetieClassifieds
    {
        public static int AddPhoto(ClassifiedsPhoto classifiedsPhoto)
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.AddPhoto(classifiedsPhoto);
        }

        public static bool HasClassifiedsCategories()
        {
            var provider = SueetieDataProvider.LoadProvider();
            return provider.HasClassifiedsCategories();
        }
    }
}