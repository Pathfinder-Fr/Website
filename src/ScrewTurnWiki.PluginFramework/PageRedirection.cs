namespace ScrewTurn.Wiki.PluginFramework
{
    /// <summary>
    /// Contains information about a redirection target page.
    /// </summary>
    public sealed class PageRedirection
    {
        /// <summary>
        /// Gets or sets the target page full name.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the URL framgment targeted by the redirection.
        /// </summary>
        public string Fragment { get; set; }
    }
}
