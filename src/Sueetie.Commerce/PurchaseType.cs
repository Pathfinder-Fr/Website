namespace Sueetie.Commerce
{
    using System;

    public enum PurchaseType
    {
        /// <summary>
        /// Non spécifié.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Achetable.
        /// </summary>
        Commercial = 1,

        /// <summary>
        /// Téléchargeable pour les membres.
        /// </summary>
        FreeRegistered = 2,

        FreeSubscribers = 3,

        /// <summary>
        /// Téléchargeable pour tout le monde.
        /// </summary>
        FreeAll = 4,

        Contribution
    }
}

