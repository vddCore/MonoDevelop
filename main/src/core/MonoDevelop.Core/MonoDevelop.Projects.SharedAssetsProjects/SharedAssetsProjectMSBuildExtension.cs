﻿//
// SharedAssetsProjectMSBuildExtension.cs
//
// Author:
//       Lluis Sanchez Gual <lluis@xamarin.com>
//
// Copyright (c) 2014 Xamarin, Inc (http://www.xamarin.com)
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
using System.Linq;
using System.IO;
using MonoDevelop.Core;
using System.Collections.Generic;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.SharedAssetsProjects
{
	class SharedAssetsProjectMSBuildExtension: MSBuildExtension
	{
		public override void LoadProject (IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject msproject)
		{
			base.LoadProject (monitor, item, msproject);

			var dnp = item as DotNetProject;
			if (dnp == null)
				return;

			// Convert .projitems imports into project references

			foreach (var sp in msproject.Imports.Where (im => im.Label == "Shared" && im.Project.EndsWith (".projitems"))) {
				var projitemsFile = sp.Project;
				if (!string.IsNullOrEmpty (projitemsFile)) {
					projitemsFile = MSBuildProjectService.FromMSBuildPath (item.ItemDirectory, projitemsFile);
					projitemsFile = Path.Combine (Path.GetDirectoryName (msproject.FileName), projitemsFile);
					if (File.Exists (projitemsFile)) {
						MSBuildSerializer iser = Handler.CreateSerializer ();
						iser.SerializationContext.BaseFile = projitemsFile;
						iser.SerializationContext.ProgressMonitor = monitor;
						MSBuildProject p = new MSBuildProject ();
						p.Load (projitemsFile);
						Handler.LoadProjectItems (p, iser, ProjectItemFlags.Hidden | ProjectItemFlags.DontPersist);
						var r = new ProjectReference (ReferenceType.Project, Path.GetFileNameWithoutExtension (projitemsFile));
						r.Flags = ProjectItemFlags.DontPersist;
						r.SetItemsProjectPath (projitemsFile);
						dnp.References.Add (r);
					}
				}
			}
		}

		public override void SaveProject (IProgressMonitor monitor, SolutionEntityItem item, MSBuildProject project)
		{
			base.SaveProject (monitor, item, project);
			var dnp = item as DotNetProject;
			if (dnp == null)
				return;
			HashSet<string> validProjitems = new HashSet<string> ();
			foreach (var r in dnp.References.Where (rp => rp.ReferenceType == ReferenceType.Project)) {
				var ip = r.GetItemsProjectPath ();
				if (!string.IsNullOrEmpty (ip)) {
					ip = MSBuildProjectService.ToMSBuildPath (item.ItemDirectory, ip);
					validProjitems.Add (ip);
					if (!project.Imports.Any (im => im.Project == ip)) {
						var im = project.AddNewImport (ip, project.Imports.FirstOrDefault (i => i.Label != "Shared"));
						im.Label = "Shared";
						im.Condition = "Exists('" + ip + "')";
					}
				}
			}
			foreach (var im in project.Imports) {
				if (im.Label == "Shared" && im.Project.EndsWith (".projitems") && !(validProjitems.Contains (im.Project)))
					project.RemoveImport (im.Project);
			}
		}
	}
}

