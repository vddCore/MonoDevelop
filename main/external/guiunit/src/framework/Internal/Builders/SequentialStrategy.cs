﻿// ***********************************************************************
// Copyright (c) 2008 Charlie Poole
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
// ***********************************************************************

using System;
using System.Collections;
#if CLR_2_0 || CLR_4_0
using System.Collections.Generic;
#endif
using System.Reflection;
using NUnit.Framework.Api;
using NUnit.Framework.Extensibility;
using NUnit.Framework.Internal;

namespace NUnit.Framework.Builders
{
    /// <summary>
    /// SequentialStrategy creates test cases by using all of the
    /// parameter data sources in parallel, substituting <c>null</c>
    /// when any of them run out of data.
    /// </summary>
    public class SequentialStrategy : CombiningStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SequentialStrategy"/> class.
        /// </summary>
        /// <param name="sources">The sources.</param>
        public SequentialStrategy(IEnumerable[] sources) : base(sources) { }

        /// <summary>
        /// Gets the test cases generated by the CombiningStrategy.
        /// </summary>
        /// <returns>The test cases.</returns>
#if CLR_2_0 || CLR_4_0
        public override IEnumerable<ITestCaseData> GetTestCases()
        {
            List<ITestCaseData> testCases = new List<ITestCaseData>();
#else
        public override IEnumerable GetTestCases()
        {
            ArrayList testCases = new ArrayList();
#endif

            for (; ; )
            {
                bool gotData = false;
                object[] testdata = new object[Sources.Length];

                for (int i = 0; i < Sources.Length; i++)
                    if (Enumerators[i].MoveNext())
                    {
                        testdata[i] = Enumerators[i].Current;
                        gotData = true;
                    }
                    else
                        testdata[i] = null;

                if (!gotData)
                    break;

                ParameterSet parms = new ParameterSet();
                parms.Arguments = testdata;
                testCases.Add(parms);
            }

            return testCases;
        }
    }
}
