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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using MonkeyBuilder.Properties;
using MonkeyBuilder.MonoCompiler;

namespace MonkeyBuilder
{
	class Program
	{
		static int Main (string[] args)
		{
			// Build mono runtime
			Console.WriteLine ("Building the mono runtime..");
			
			string msbuild = @"C:\Windows\Microsoft.NET\Framework\v3.5\msbuild.exe";
			string solution = Utilities.CombinePaths (Environment.CurrentDirectory, "mono", "msvc", "mono.sln");
			string[] msbuild_args = new string[] { "/m", "\"" + solution + "\"", "/p:Configuration=Release_eglib" };
			
			CommandLineResults results = CommandLineRunner.ExecuteCommand (msbuild, null, string.Join (" ", msbuild_args));
			
			if (results.ExitCode != 0) {
				Console.WriteLine ("Error compiling mono runtime:");
				Console.WriteLine (results.Output);
				return 1;
			}

			Console.WriteLine ("Runtime successfully built.");
			
			// Build managed libraries/tools
			Console.WriteLine ("Building the managed libraries..");

			MonoCompiler.MonoCompiler mc = new MonkeyBuilder.MonoCompiler.MonoCompiler ();
			
			string config_file = Utilities.CombinePaths (Environment.CurrentDirectory, "win32.xml");

			StepResults compile_results = mc.Compile ("unknown", config_file);

			if (compile_results.ExitCode != 0) {
				Console.WriteLine ("Error compiling managed libraries:");
				Console.WriteLine (compile_results.Log);
				return 1;
			}

			Console.WriteLine ("Managed libraries successfully built.");

			return 0;
		}
	}
}
