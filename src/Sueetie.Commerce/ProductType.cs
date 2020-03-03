namespace Sueetie.Commerce
{
    using System;

    public enum ProductType
    {
        NA = 0,

        /// <summary>
        /// Téléchargement fichier.
        /// </summary>
        ElectronicDownload,

        /// <summary>
        /// Licence produit.
        /// </summary>
        ProductLicense,

        /// <summary>
        /// Abonnement.
        /// </summary>
        Subscription,

        /// <summary>
        /// Produit physique.
        /// </summary>
        PhysicalContent,

        /// <summary>
        /// Package sueetie.
        /// </summary>
        SueetiePackage,

        /// <summary>
        /// Logiciel sans licence.
        /// </summary>
        NonLicensedSoftware,

        /// <summary>
        /// Service.
        /// </summary>
        Service
    }
}

