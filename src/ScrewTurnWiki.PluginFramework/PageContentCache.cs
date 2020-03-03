using System.Collections.Generic;

namespace ScrewTurn.Wiki.PluginFramework
{
    /// <summary />
    public class PageContentCache
    {
        /// <summary />
        public PageContentCache(PageInfo pageInfo, string formattedContent, List<string> linkedPages)
        {
            PageInfo = pageInfo;
            FormattedContent = formattedContent;
            LinkedPages = linkedPages;
        }

        /// <summary />
        public PageInfo PageInfo { get; }

        /// <summary />
        public string FormattedContent { get; }

        /// <summary />
        public List<string> LinkedPages { get; }
    }
}
