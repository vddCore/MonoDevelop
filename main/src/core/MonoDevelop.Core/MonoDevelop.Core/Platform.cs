// 
// Platform.cs
//  
// Author:
//       Michael Hutchinson <mhutch@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc. (http://xamarin.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Runtime.InteropServices;
using System.IO;

namespace MonoDevelop.Core
{
	public static class Platform
	{
		public readonly static bool IsWindows;
		public readonly static bool IsLinux;

		public static Version OSVersion { get; private set; }

		static Platform ()
		{
			IsWindows = Path.DirectorySeparatorChar == '\\';
			IsLinux = !IsWindows;
			OSVersion = Environment.OSVersion.Version;

			// needed to make sure various p/invokes work
			if (Platform.IsWindows) {
				InitWindowsNativeLibs ();
			}
		}

		public static void Initialize ()
		{
			//no-op, triggers static ctor
		}

		[DllImport ("libc")]
		static extern int uname (IntPtr buf);

		[DllImport ("libc")]
		extern static IntPtr dlopen (string name, int mode);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetDllDirectory (string lpPathName);

		static void InitWindowsNativeLibs ()
		{
			string location = null;
			using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Xamarin\GtkSharp\InstallFolder")) {
				if (key != null) {
					location = key.GetValue (null) as string;
				}
			}
			if (location == null || !File.Exists (Path.Combine (location, "bin", "libgtk-win32-2.0-0.dll"))) {
				LoggingService.LogError ("Did not find registered GTK# installation");
				return;
			}
			var path = Path.Combine (location, @"bin");
			try {
				if (SetDllDirectory (path)) {
					return;
				}
			} catch (EntryPointNotFoundException) {
			}
			LoggingService.LogError ("Unable to set GTK# dll directory");
		}
	}
}