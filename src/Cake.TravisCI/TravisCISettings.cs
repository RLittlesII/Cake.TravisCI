using Cake.Core.Tooling;

namespace Cake.TravisCI
{
    /// <inheritdoc />
    /// <summary>
    /// <see cref="ToolSettings"/> for the TravisCI client.
    /// </summary>
    /// <seealso cref="T:Cake.Core.Tooling.ToolSettings" />
    public class TravisCISettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to rescue exceptions.
        /// </summary>
        public bool NoExplode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check it travis client is up to date.
        /// </summary>
        public bool SkipVersionCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check if auto-completion is set up.
        /// </summary>
        public bool SkipCompletionCheck { get; set; }

        /// <summary>
        /// Gets or sets the Travis API server to talk to.
        /// </summary>
        public string ApiEndpointUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to verify SSL certificate of API endpoint.
        /// </summary>
        public bool Insecure { get; set; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show API requests.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the enterprise setup.
        /// </summary>
        public string Enterprise { get; set; }

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        public string Repository { get; set; }

        /// <summary>
        /// Gets or sets the repository and remebers the value for the current directory.
        /// </summary>
        public string StoreRepository { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to delete the listed caches.
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// Gets or sets the list of caches on the specified branch.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the list of caches that match the criteria.
        /// </summary>
        public string Match { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ask user for delete confirmation.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to be interactive and colorful.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the pro api.
        /// </summary>
        public bool Pro {get;set;}

        /// <summary>
        /// Gets or sets a value indicating whether to use the org api.
        /// </summary>
        public bool Org {get;set;}
    }
}