using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HermaFx.MvcDemo
{
	/// <summary>
	/// A generic website-level exception.
	/// </summary>
	/// <seealso cref="System.Exception" />
	[global::System.Serializable]
	public class WebSiteException : Exception
	{
		public const string DEFAULT_TITLE = "Error";
		//
		// For guidelines regarding the creation of new exception types, see
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
		// and
		//    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
		//

		public string Title { get; set; }

		public WebSiteException() { }
		public WebSiteException(string message) : this(DEFAULT_TITLE, message) { }
		public WebSiteException(string message, Exception inner) : this(DEFAULT_TITLE, message, inner) { }
		public WebSiteException(string title, string message) : this(title, message, null) { }
		public WebSiteException(string title, string message, Exception inner) : base(message, inner) { Title = title; }

		protected WebSiteException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}