using System;
using System.Collections.Generic;
using System.Text;
// ReSharper disable InconsistentNaming

namespace lastpage.Features.Articles
{
    public class ArticleConfiguration
    {
        /// <summary>
        /// Folder for articles relative to the build definition
        /// </summary>
        public string articlesFolder { get; set; }

        /// <summary>
        /// Name of the partial that contains the latest articles
        /// </summary>
        public string latestPartialName { get; set; }

        /// <summary>
        /// Name of the partial that contains the article archive
        /// </summary>
        public string archivePartialName { get; set; }

        /// <summary>
        /// Name of the partial that contains all the tags
        /// </summary>
        public string tagListingPartialName { get; set; }

        /// <summary>
        /// Path template for the tag collection pages
        /// </summary>
        /// <remarks>Add %HERE% mark replaceable part</remarks>
        public string tagArticlesPathTemplate { get; set; }

        /// <summary>
        /// Page template for the tag collection pages
        /// </summary>
        /// <remarks>See docs for template contents definition</remarks>
        public string tagArticlesPageTemplate { get; set; }
    }
}
