// -----------------------------------------------------------------------
// <copyright file="SiteLogCategoryType.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    public enum SiteLogCategoryType
    {
        GenericMessage = 100,
        DebugInfo = 101,
        EmailException = 102,
        GeneralException = 103,
        TasksException = 104,
        AppStartStop = 105,
        GeneralAppEvent = 106,
        SearchException = 107,
        TasksMessage = 109,
        MarketplaceMessage = 201,
        MarketplaceException = 202,
        AddonPackMessage = 301,
        AddonPackException = 302,
        AnalyticsMessage = 401,
        AnalyticsException = 402
    }
}