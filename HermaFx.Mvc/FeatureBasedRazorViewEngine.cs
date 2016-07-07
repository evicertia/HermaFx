using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HermaFx.Mvc
{
	public class FeatureBasedRazorViewEngine : RazorViewEngine
	{
		//holds all of the actual paths to the required files
		private static ConcurrentDictionary<string, string> _ResolvedFilePaths = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public FeatureBasedRazorViewEngine()
		{
			// {0} ActionName; {1} ControllerName; {2} AreaName

			FileExtensions = new[]
			{
				"cshtml"
			};

			ViewLocationFormats = new[]
			{
				"~/Features/{1}/{1}{0}.cshtml",
				"~/Features/Shared/{0}.cshtml"
			};

			MasterLocationFormats = ViewLocationFormats;

			PartialViewLocationFormats = new[]
			{
				"~/Features/{1}/{1}{0}.cshtml",
				"~/Features/Shared/{0}.cshtml"
			};

			AreaViewLocationFormats = new[]
			{
				"~/Areas/{2}/{1}/{1}{0}.cshtml",
				"~/Areas/{2}/Shared/{0}.cshtml" // Replacement for "Views/Shared"
			};

			AreaMasterLocationFormats = AreaViewLocationFormats;

			AreaPartialViewLocationFormats = new[]
			{
				"~/Areas/{2}/{1}/_{1}{0}.cshtml",
				"~/Areas/{2}/Shared/{0}.cshtml" // Replacement for "Views/Shared"
			};
		}

		#region Get Root Directory
		private string _AppRootPath = null;

		private string GetRootDirectory()
		{
			lock (this)
			{
				if (_AppRootPath == null)
				{
					_AppRootPath = HttpContext.Current.Server.MapPath("~/");
				}

				return _AppRootPath;
			}
		}
		#endregion

		#region Internal Methods

		// searches for a matching file name in the current directory
		private string LookupFile(DirectoryInfo folder, string name)
		{
			//try and find a matching file, regardless of case
			var match = folder.EnumerateFiles().FirstOrDefault(file => file.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			return match is FileInfo ? match.Name : null;
		}

		// searches for a folder in the current directory and steps up a level
		private string LookupDirectory(ref DirectoryInfo folder, string name)
		{
			folder = folder.EnumerateDirectories().FirstOrDefault(dir => dir.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
			return folder is DirectoryInfo ? folder.Name : null;
		}

		// determines (and caches) the actual path for a file
		private string ResolveActualFilePath(string virtualPath)
		{
			// get the root folder to work from
			var folder = new DirectoryInfo(GetRootDirectory());
			var segments = virtualPath.Split(new char[] { '/' });

			// start stepping up the folders to replace with the correct cased folder name
			for (int i = 0; i < segments.Length; i++)
			{
				var part = segments[i];
				var last = i == segments.Length - 1;

				// ignore the root
				if (part.Equals("~")) continue;

				// process the file name if this is the last segment
				else if (last) part = LookupFile(folder, part);

				// step up the directory for another part
				else part = this.LookupDirectory(ref folder, part);

				// if no matches were found, just return null
				if (part == null || folder == null) return null;

				// update the segment with the correct name
				segments[i] = part;
			}

			return string.Join("/", segments);
		}

		private string GetActualFilePath(string virtualPath)
		{
			// check if this has already been matched before
			if (_ResolvedFilePaths.ContainsKey(virtualPath))
				return _ResolvedFilePaths[virtualPath];

			var result = ResolveActualFilePath(virtualPath);
			if (result != null)
			{
				_ResolvedFilePaths.TryAdd(virtualPath, result);
			}

			return result ?? virtualPath;
		}

		#endregion

		#region IViewEngine Overrides

		/// <summary>
		/// Creates the view.
		/// </summary>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="viewPath">The view path.</param>
		/// <param name="masterPath">The master path.</param>
		/// <returns></returns>
		protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
		{
			return base.CreateView(controllerContext, GetActualFilePath(viewPath), GetActualFilePath(masterPath));
		}

		/// <summary>
		/// Creates the partial view.
		/// </summary>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="partialPath">The partial path.</param>
		/// <returns></returns>
		protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
		{
			return base.CreatePartialView(controllerContext, GetActualFilePath(partialPath));
		}

		/// <summary>
		/// Files the exists.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="virtualPath">The virtual path.</param>
		/// <returns></returns>
		protected override bool FileExists(ControllerContext context, string virtualPath)
		{
			return base.FileExists(context, GetActualFilePath(virtualPath));
		}

		#endregion
	}
}