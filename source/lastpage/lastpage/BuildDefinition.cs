// ReSharper disable InconsistentNaming

using lastpage.Features.Articles;

namespace lastpage
{
    /// <summary>
    /// Build definition
    /// </summary>
    public class BuildDefinition
    {
        /// <summary>
        /// Source folder relative to the build definition
        /// </summary>
        /// <remarks>Defaults to "build"</remarks>
        public string sourceFolder { get; set; }

        /// <summary>
        /// Target folder relative to the build definition
        /// </summary>
        /// <remarks>Defaults to "live"</remarks>
        public string targetFolder { get; set; }

        /// <summary>
        /// Source for static post-build content folder relative to the build definition
        /// </summary>
        /// <remarks>Defaults to "post"</remarks>
        public string postFolder { get; set; }

        /// <summary>
        /// Articles configuration object, add to enable feature
        /// </summary>
        public ArticleConfiguration articles { get; set; }

        /// <summary>
        /// Platform adapter to generate platform specific code, add to enable feature
        /// </summary>
        public string platformAdapter { get; set; }

        public static BuildDefinition GetDefault => new BuildDefinition
        {
            sourceFolder = "build",
            targetFolder = "live",
            postFolder = "post"
        };
    }
}
