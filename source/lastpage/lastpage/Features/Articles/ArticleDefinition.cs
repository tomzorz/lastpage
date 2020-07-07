using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace lastpage.Features.Articles
{
    /// <summary>
    /// Article definition sidecar for .md files
    /// </summary>
    public class ArticleDefinition
    {
        /// <summary>
        /// Title of the article
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Date for article publication
        /// </summary>
        public DateTimeOffset publishedDate { get; set; }

        /// <summary>
        /// Date for article last updated
        /// </summary>
        public DateTimeOffset updatedDate { get; set; }

        /// <summary>
        /// Tags of the article
        /// </summary>
        public List<string> tags { get; set; }

        /// <summary>
        /// Urls of the article
        /// </summary>
        /// <remarks>The first one will be the main one, at least one URL is expected</remarks>
        public List<string> urls { get; set; }
    }
}
