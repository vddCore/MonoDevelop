//
// ReinstallPackageAction.cs
//
// Author:
//       Matt Ward <matt.ward@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (http://xamarin.com)
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
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.PackageManagement.NodeBuilders;
using MonoDevelop.Projects;
using ICSharpCode.PackageManagement;
using NuGet;

namespace MonoDevelop.PackageManagement
{
	public class ReinstallPackageAction : ProcessPackageAction
	{
		IFileRemover fileRemover;

		public ReinstallPackageAction (
			IPackageManagementProject project,
			IPackageManagementEvents packageManagementEvents)
			: this (project, packageManagementEvents, new FileRemover ())
		{
		}

		public ReinstallPackageAction (
			IPackageManagementProject project,
			IPackageManagementEvents packageManagementEvents,
			IFileRemover fileRemover)
			: base (project, packageManagementEvents)
		{
			this.fileRemover = fileRemover;
		}

		protected override string StartingMessageFormat {
			get { return "Retargeting {0}..." + Environment.NewLine; }
		}

		protected override void ExecuteCore ()
		{
			using (IDisposable referenceMaintainer = CreateLocalCopyReferenceMaintainer ()) {
				using (IDisposable monitor = CreateFileMonitor (fileRemover)) {
					UninstallPackage ();
				}
				InstallPackage ();
			}
		}

		void UninstallPackage ()
		{
			UninstallPackageAction action = Project.CreateUninstallPackageAction ();
			action.Package = Package;
			action.ForceRemove = true;
			action.Execute ();
		}

		void InstallPackage ()
		{
			InstallPackageAction action = Project.CreateInstallPackageAction ();
			action.Package = Package;
			action.OpenReadMeText = false;
			action.PreserveLocalCopyReferences = false;
			action.LicensesMustBeAccepted = false;
			action.Execute ();
		}
	}
}

