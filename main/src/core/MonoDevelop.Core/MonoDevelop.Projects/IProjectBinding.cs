//
// IProjectBinding.cs
//
// Author:
//   Lluis Sanchez Gual

//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
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
using System.Collections;
using System.Xml;

namespace MonoDevelop.Projects
{
	public interface IProjectBinding
	{
		/// <remarks>
		/// Returns the project type name
		/// </remarks>
		string Name { get; }
		
		/// <remarks>
		/// Creates a Project out of the given ProjetCreateInformation object.
		/// Each project binding must provide a representation of the project
		/// it 'controls'.
		/// </remarks>
		Project CreateProject (ProjectCreateInformation info, XmlElement projectOptions);
		
		/// <remarks>
		/// Creates a Project for a single source file. If the file is not
		/// valid for this project type, it must return null.
		/// </remarks>
		Project CreateSingleFileProject (string sourceFile);

		bool CanCreateSingleFileProject (string sourceFile);
	}
}
