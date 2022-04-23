﻿// *****************************************************
// Copyright 2007, Charlie Poole
//
// Licensed under the Open Software License version 3.0
// *****************************************************

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("NUnitLiteTests")]
[assembly: AssemblyDescription("Tests of the NUnitLite testing framework")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NUnitLite")]
[assembly: AssemblyCopyright("Copyright ©  2007, Charlie Poole")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e7a2d0a1-69b5-40a6-bbfa-4c2e77335d8d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.0")]
#if !PocketPC && !WindowsCE && !NETCF
[assembly: AssemblyFileVersion("1.0.0.0")]
#endif

// Under Silverlight, it's only possible to reflect
// over members that would be accessible normally.
#if SILVERLIGHT
[assembly: InternalsVisibleTo("nunitlite")]
#endif