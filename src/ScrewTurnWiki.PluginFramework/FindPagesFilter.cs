using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrewTurn.Wiki.PluginFramework
{
    /// <summary />
    public class FindPagesFilter
    {
        /// <summary />
        public NamespaceInfo Namespace { get; set; }

        /// <summary />
        public string Name { get; set; }

        /// <summary />
        public int PageIndex { get; set; }

        /// <summary />
        public int PageSize { get; set; }

        /// <summary />
        public IEnumerable<PageInfo> ApplyOn(IEnumerable<PageInfo> list)
        {
            list = list.Where(p => NameTools.AreNamespaceEquals(NameTools.GetNamespace(p.FullName), Namespace));

            if (!string.IsNullOrEmpty(Name))
            {
                list = list.Where(p => MatchPattern(NameTools.GetLocalName(p.FullName), Name));
            }

            if (PageIndex > -1 && PageSize > -1)
            {
                list = list
                    .Skip(PageIndex*PageSize)
                    .Take(PageSize);
            }

            return list;
        }

        /// <summary />
        public static bool MatchPattern(string value, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return string.IsNullOrEmpty(value);
            }

            if ((pattern.Length == 1 && pattern[0] == '*') || pattern.Length == 2 && pattern == "**")
            {
                return true;
            }

            var starstWith = pattern[pattern.Length - 1] == '*';
            var endsWith = pattern[0] == '*';

            if (starstWith && endsWith)
            {
                return value.IndexOf(pattern.Substring(1, pattern.Length - 2)) != -1;
            }
            if (starstWith)
            {
                return value.StartsWith(pattern.Substring(0, pattern.Length - 1), StringComparison.OrdinalIgnoreCase);
            }
            if (endsWith)
            {
                return value.EndsWith(pattern.Substring(1), StringComparison.OrdinalIgnoreCase);
            }
            return value.Equals(pattern, StringComparison.OrdinalIgnoreCase);
        }
    }
}