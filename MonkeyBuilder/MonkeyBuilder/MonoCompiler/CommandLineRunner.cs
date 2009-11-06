//
// Authors:
// Jonathan Pobst (monkey@jpobst.com)
//
// Copyright (C) 2009 Jonathan Pobst 
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

using System;
using System.Diagnostics;
using System.Text;
using System.ComponentModel;

namespace MonkeyBuilder.MonoCompiler
{
	public static class CommandLineRunner
	{
		public static CommandLineResults ExecuteCommand (string command, string workingDirectory, params string[] arguments)
		{
			return ExecuteCommand (command, workingDirectory, string.Join (" ", arguments));
		}

		public static CommandLineResults ExecuteCommand (string command, string workingDirectory, string arguments)
		{
			try {
			// Build our ProcessStartInfo
			ProcessStartInfo psi = new ProcessStartInfo (command, arguments);

			if (!string.IsNullOrEmpty (workingDirectory))
				psi.WorkingDirectory = workingDirectory;

			// Set up output buffers
			StringBuilder std_output = new StringBuilder ();
			StringBuilder std_error = new StringBuilder ();

			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;

			psi.UseShellExecute = false;

			Process p = new Process ();
			p.StartInfo = psi;

			p.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) {
				std_output.AppendLine (e.Data);
			};

			p.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) {
				std_error.AppendLine (e.Data);
			};

			// Run our Process
			DateTime start = DateTime.Now;

			p.Start ();
			p.BeginOutputReadLine();
			p.BeginErrorReadLine();
			
			p.WaitForExit ();

			CommandLineResults results = new CommandLineResults ();

			results.ExecutionTime = DateTime.Now.Subtract (start);
			results.ExitCode = p.ExitCode;

			// Robocopy has "success" exitcodes 0-7  :(
			if (command.ToLowerInvariant () == "robocopy" && p.ExitCode < 8)
				results.ExitCode = 0;

			results.Output = std_output.ToString ();
			results.ErrorOutput = std_error.ToString ();
			
			return results;
			} catch (Win32Exception ex) {
				Console.WriteLine ("Cannot find file:");
				Console.WriteLine ("Command: {0}", command);
				
				throw;
			}
		}
	}
}
