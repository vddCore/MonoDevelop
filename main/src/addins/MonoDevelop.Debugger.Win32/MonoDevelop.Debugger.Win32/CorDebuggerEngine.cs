using System;
using System.Collections.Generic;
using System.IO;
using Mono.Debugging.Client;
using MonoDevelop.Core;
using MonoDevelop.Core.Execution;

namespace MonoDevelop.Debugger.Win32
{
	class CorDebuggerEngine: IDebuggerEngine
	{
		#region IDebuggerEngine Members

		public bool CanDebugCommand (ExecutionCommand command)
		{
			DotNetExecutionCommand cmd = command as DotNetExecutionCommand;
			if (cmd != null)
				return (cmd.TargetRuntime == null || cmd.TargetRuntime.RuntimeId == "MS.NET");
			return false;
		}

		public DebuggerStartInfo CreateDebuggerStartInfo (ExecutionCommand command)
		{
			DotNetExecutionCommand cmd = command as DotNetExecutionCommand;
			if (cmd != null) {
				DebuggerStartInfo startInfo = new DebuggerStartInfo ();
				startInfo.Command = cmd.Command;
				startInfo.Arguments = cmd.Arguments;
				startInfo.WorkingDirectory = cmd.WorkingDirectory;
				if (cmd.EnvironmentVariables.Count > 0) {
					foreach (KeyValuePair<string, string> val in cmd.EnvironmentVariables)
						startInfo.EnvironmentVariables[val.Key] = val.Value;
				}
				return startInfo;
			}

			throw new NotSupportedException ();
		}

		public DebuggerSession CreateSession ( )
		{
			return new CorDebuggerSession ();
		}

		public ProcessInfo[] GetAttachableProcesses ( )
		{
			return new ProcessInfo[0];
		}

		#endregion
	}
}
