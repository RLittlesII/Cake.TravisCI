using System;
using Cake.Common.Build.TravisCI;
using Cake.Core;
using Cake.TravisCI.Upload;

namespace Cake.TravisCI
{
	public static class TravisCIUploadAliases
	{
        /// <summary>
        /// Uploads artifacts to TravisCI.
        ///  <example>
        ///   <code>
        ///     var settings = new TravisCIUploadSettings
        ///         {
        ///             Key = "AKIT339AFIY655O3Q9DZ",
        ///             Secret = "",
        ///             Bucket = ""
        ///         };
        ///     TravisCIUpload(settings);
        ///   </code>
        ///  </example>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="travisCIUploadSettings">The travis ci upload settings.</param>
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
	    /// <param name="configurator">The configurator.</param>
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
