// 
// AssemblyInfoWriter.cs
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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Application
{
	static class AssemblyInfoWriter
	{
		static void Main (string[] args)
		{
			if (args.Length != 1|| !File.Exists (args[0])) {
				Console.WriteLine ("Usage: AssemblyInfoWriter inputfile");
				Environment.Exit (1);
			}
			
			string outFile = Path.Combine (Path.GetDirectoryName (args[0]), "AssemblyInfo.cs");
			XElement el = XDocument.Load (args[0]).Element ("Addin");
			if (el == null) {
				Console.WriteLine ("Error: missing Addin element in addin file '{0}'.", args[0]);
				Environment.Exit (1);
			}
			
			var maps = new Dictionary<string,string> () {
				{"name", "AssemblyTitle"},
				{"description", "AssemblyDescription"},
				{"version", "AssemblyVersion"},
				{"copyright", "AssemblyCopyright"}
			};
			
			using (TextWriter writer = new StreamWriter (outFile)) {
				writer.WriteLine ("// Autogenerated from {0}", Path.GetFileName (args[0]));
				writer.WriteLine ();
				writer.WriteLine ("using System.Reflection;");
				writer.WriteLine ();
				writer.WriteLine ("[assembly: AssemblyProduct (\"MonoDevelop\")]");
				
				foreach (KeyValuePair<string, string> map in maps) {
					XAttribute att = el.Attribute (map.Key);
					if (att == null || String.IsNullOrEmpty (att.Value))
						Console.WriteLine ("Warning: missing {0} in addin file '{1}'.", map.Key, args[0]);
					else
						writer.WriteLine ("[assembly: {0} (\"{1}\")]", map.Value, att.Value);
				}
			}
		}
	}
}
