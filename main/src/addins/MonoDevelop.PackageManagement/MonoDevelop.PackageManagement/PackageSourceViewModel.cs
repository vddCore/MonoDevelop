// 
// PackageSourceViewModel.cs
// 
// Author:
//   Matt Ward <ward.matt@gmail.com>
// 
// Copyright (C) 2013 Matthew Ward
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
using NuGet;

namespace ICSharpCode.PackageManagement
{
	public class PackageSourceViewModel : ViewModelBase<PackageSourceViewModel>
	{
		RegisteredPackageSource packageSource;
		
		public PackageSourceViewModel(PackageSource packageSource)
		{
			this.packageSource = new RegisteredPackageSource(packageSource);
			IsValid = true;
			ValidationFailureMessage = "";
		}
		
		public PackageSource GetPackageSource()
		{
			return packageSource.ToPackageSource();
		}
		
		public string Name {
			get { return packageSource.Name; }
			set { packageSource.Name = value; }
		}
		
		public string SourceUrl {
			get { return packageSource.Source; }
			set { packageSource.Source = value; }
		}
		
		public bool IsEnabled {
			get { return packageSource.IsEnabled; }
			set { packageSource.IsEnabled = value; }
		}

		public string UserName {
			get { return packageSource.UserName; }
			set { packageSource.UserName = value; }
		}

		public string Password {
			get { return packageSource.Password; }
			set { packageSource.Password = value; }
		}

		public bool HasPassword ()
		{
			return !String.IsNullOrEmpty (Password);
		}

		public bool IsValid { get; set; }
		public string ValidationFailureMessage { get; set; }
	}
}
