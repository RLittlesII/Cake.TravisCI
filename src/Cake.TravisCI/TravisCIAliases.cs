using System;
using Cake.Common.Build.TravisCI;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.TravisCI
{
    [CakeAliasCategory("TravisCI")]
    public static class TravisCIAliases
    {
        /// <summary>
        /// Lists or deletes TravisCI repository cache.
        ///  <example>
        ///   <code>
        ///     var settings = new TravisCISettings
        ///         {
        ///             Token = "AKIT339AFIY655O3Q9DZ",
        ///             Org = true,
        ///             Branch = "develop"
        ///         };
        ///     TravisCICache(settings);
        ///   </code>
        ///  </example>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The TravisCI settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Cache")]
        public static void TravisCICache(this ICakeContext context, TravisCISettings settings)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new TravisCIRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Cache(settings ?? new TravisCISettings());
        }

        /// <summary>
        /// Lists or deletes TravisCI repository cache.
        ///  <example>
	    ///   <code>
	    ///     TravisCICache(settings =>
	    ///         {
	    ///             settings.Token = "AKIT339AFIY655O3Q9DZ";
	    ///             settings.Org = true;
	    ///             settings.Branch = "develop";
	    ///         });
	    ///   </code>
	    ///  </example>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configurator">The action that creates the TravisCI settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Cache")]
        public static void TravisCICache(this ICakeContext context, Action<TravisCISettings> configurator)
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            var settings = new TravisCISettings();
            configurator(settings);
            context.TravisCICache(settings);
        }

        /// <summary>
        /// Uploads artifacts to TravisCI.
        ///  <example>
        ///   <code>
        ///     var settings = new TravisCIUploadSettings
        ///         {
        ///             Key = "AKIT339AFIY655O3Q9DZ",
        ///             Secret = "48TmqyraUyJ7Efpegi6Lfd10yUskAMB0G2TtRCX1",
        ///             Bucket = "cake-bucket"
        ///         };
        ///     TravisCIUpload(settings);
        ///   </code>
        ///  </example>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="travisCIUploadSettings">The TravisCI upload settings.</param>
		[CakeMethodAlias]
        [CakeAliasCategory("Upload")]
        public static void TravisCIUpload(this ICakeContext context,
			TravisCIUploadSettings travisCIUploadSettings)
		{
		    if (context == null)
		    {
		        throw new ArgumentNullException(nameof(context));
		    }

		    var runner =
		        new TravisCIUploadRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
		    runner.Upload(travisCIUploadSettings ?? new TravisCIUploadSettings());
		}

	    /// <summary>
	    /// Uploads artifacts to TravisCI.
	    ///  <example>
	    ///   <code>
	    ///     TravisCIUpload(settings =>
	    ///         {
	    ///             settings.Key = "AKIT339AFIY655O3Q9DZ";
	    ///             settings.Secret = "48TmqyraUyJ7Efpegi6Lfd10yUskAMB0G2TtRCX1";
	    ///             settings.Bucket = "cake-bucket";
	    ///         });
	    ///   </code>
	    ///  </example>
	    /// </summary>
	    /// <param name="context">The context.</param>
	    /// <param name="configurator">The action that creates the TravisCI upload settings.</param>
		[CakeMethodAlias]
        [CakeAliasCategory("Upload")]
	    public static void TravisCIUpload(this ICakeContext context, Action<TravisCIUploadSettings> configurator)
	    {
	        if (configurator == null)
	        {
	            throw new ArgumentNullException(nameof(configurator));
	        }

	        var settings = new TravisCIUploadSettings();

	        configurator(settings);

	        context?.TravisCIUpload(settings);
	    }
    }
}