// 
// Gettext.cs
// 
// Author:
//   Michael Hutchinson <mhutchinson@novell.com>
// 
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.IO;

using Mono.Unix;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace MonoDevelop.Core
{
	public static class GettextCatalog
	{
		static Thread mainThread;

		[DllImport ("kernel32.dll", SetLastError = true)]
		static extern int SetThreadUILanguage (int LangId);

		const int LOCALE_CUSTOM_UNSPECIFIED = 4096;

		static GettextCatalog ()
		{
			mainThread = Thread.CurrentThread;

			//variable can be used to override where Gettext looks for the catalogues
			string catalog = Environment.GetEnvironmentVariable ("MONODEVELOP_LOCALE_PATH");

			// Set the user defined language
			string lang = PropertyService.Get ("MonoDevelop.Ide.UserInterfaceLanguage", "");
			if (!string.IsNullOrEmpty (lang)) {
				if (Platform.IsWindows) {
					lang = lang.Replace("_", "-");
					CultureInfo ci = CultureInfo.GetCultureInfo(lang);
					if (ci.IsNeutralCulture) {
						// We need a non-neutral culture
						foreach (CultureInfo c in CultureInfo.GetCultures (CultureTypes.AllCultures & ~CultureTypes.NeutralCultures))
							if (c.Parent != null && c.Parent.Name == ci.Name && c.LCID != LOCALE_CUSTOM_UNSPECIFIED) {
								ci = c;
								break;
							}
					}
					if (!ci.IsNeutralCulture) {
						SetThreadUILanguage (ci.LCID);
						mainThread.CurrentUICulture = ci;
					}
				}
				else
					Environment.SetEnvironmentVariable ("LANGUAGE", lang);
			}
			
			if (string.IsNullOrEmpty (catalog) || !Directory.Exists (catalog)) {
				string location = System.Reflection.Assembly.GetExecutingAssembly ().Location;
				location = Path.GetDirectoryName (location);
				if (Platform.IsWindows) {
					// On windows, load the catalog from a child dir
					catalog = Path.Combine (location, "locale");
				}
				else {
					// MD is located at $prefix/lib/monodevelop/bin
					// adding "../../.." should give us $prefix
					string prefix = Path.Combine (Path.Combine (Path.Combine (location, ".."), ".."), "..");
					//normalise it
					prefix = Path.GetFullPath (prefix);
					//catalogue is installed to "$prefix/share/locale" by default
					catalog = Path.Combine (Path.Combine (prefix, "share"), "locale");
				}
			}
			try {
				Catalog.Init ("monodevelop", catalog);
			}
			catch (Exception ex) {
				Console.WriteLine (ex);
			}
		}

		public static CultureInfo UICulture {
			get { return mainThread.CurrentUICulture; }
		}
		
		#region GetString

		static string GetStringInternal (string phrase)
		{
			try {
				return Catalog.GetString (phrase);
			} catch (Exception e) {
				LoggingService.LogError ("Failed to localize string", e);
				return phrase;
			}
		}
		
		public static string GetString (string phrase)
		{
			return GetStringInternal (phrase);
		}
		
		public static string GetString (string phrase, object arg0)
		{
			return string.Format (GetStringInternal (phrase), arg0);
		}
		
		public static string GetString (string phrase, object arg0, object arg1)
		{
			return string.Format (GetStringInternal (phrase), arg0, arg1);
		}
		
		public static string GetString (string phrase, object arg0, object arg1, object arg2)
		{
			return string.Format (GetStringInternal (phrase), arg0, arg1, arg2);
		}
		
		public static string GetString (string phrase, params object[] args)
		{
			return string.Format (GetStringInternal (phrase), args);
		}
		
		#endregion
		
		#region GetPluralString

		static string GetPluralStringInternal (string singular, string plural, int number)
		{
			try {
				return Catalog.GetPluralString (singular, plural, number);
			} catch (Exception e) {
				LoggingService.LogError ("Failed to localize string", e);
				return number == 0 ? singular : plural;
			}
		}
		
		public static string GetPluralString (string singular, string plural, int number)
		{
			return GetPluralStringInternal (singular, plural, number);
		}
		
		public static string GetPluralString (string singular, string plural, int number, object arg0)
		{
			return string.Format (GetPluralStringInternal (singular, plural, number), arg0);
		}
		
		public static string GetPluralString (string singular, string plural, int number, object arg0, object arg1)
		{
			return string.Format (GetPluralStringInternal (singular, plural, number), arg0, arg1);
		}
		
		public static string GetPluralString (string singular, string plural, int number, 
			object arg0, object arg1, object arg2)
		{
			return string.Format (GetPluralStringInternal (singular, plural, number), arg0, arg1, arg2);
		}
		
		public static string GetPluralString (string singular, string plural, int number, params object[] args)
		{
			return string.Format (GetPluralStringInternal (singular, plural, number), args);
		}
		#endregion
	}
}
