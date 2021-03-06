//
// LocalVariableValueBatch.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc. (www.xamarin.com)
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

using Mono.Debugger.Soft;

namespace Mono.Debugging.Soft
{
	public class LocalVariableBatch
	{
		readonly LocalVariable[] variables;
		readonly StackFrame frame;
		Value[] values;

		public LocalVariableBatch (StackFrame frame, LocalVariable[] variables)
		{
			this.variables = variables;
			this.frame = frame;
		}

		public Value GetValue (LocalVariable variable)
		{
			if (variable == null)
				throw new ArgumentNullException ("variable");

			if (values == null)
				values = frame.GetValues (variables);

			for (int i = 0; i < variables.Length; i++) {
				if (variable == variables[i])
					return values[i];
			}

			throw new ArgumentOutOfRangeException ("variable");
		}
	}
}
