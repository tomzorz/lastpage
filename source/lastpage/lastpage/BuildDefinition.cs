// ReSharper disable InconsistentNaming

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
        
        public static BuildDefinition GetDefault => new BuildDefinition
        {
            sourceFolder = "build",
            targetFolder = "live",
            postFolder = "post"
        };
    }
}
