namespace Sueetie.Commerce
{
    using System;

    public enum PurchaseType
    {
        /// <summary>
        /// Non sp�cifi�.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Achetable.
        /// </summary>
        Commercial = 1,

        /// <summary>
        /// T�l�chargeable pour les membres.
        /// </summary>
        FreeRegistered = 2,

        FreeSubscribers = 3,

        /// <summary>
        /// T�l�chargeable pour tout le monde.
        /// </summary>
        FreeAll = 4,

        Contribution
    }
}

