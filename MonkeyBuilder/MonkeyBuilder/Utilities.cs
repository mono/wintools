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
using System.IO;
using System.Net;
using MonkeyBuilder.Properties;

namespace MonkeyBuilder
{
	public static class Utilities
	{
		public static string ReplaceArgs (string input, string revision)
		{
			input = input.Replace ("%revision%", revision.ToString ());
			input = input.Replace ("{revision}", revision.ToString ());

			input = input.Replace ("{installedmono}", CombinePaths (Settings.Default.installedmono, "bin", "mono.exe"));

			//string sourcedir = Settings.Default.sourcedir;
			string sourcedir = Environment.CurrentDirectory;
			string destdir = CombinePaths (sourcedir, "build");
			string monobuilddir = CombinePaths (sourcedir, "mono", "msvc", "Win32_Release_eglib", "bin");

			input = input.Replace ("{monobuilddir}", monobuilddir);
			input = input.Replace ("{destdir}", destdir);
			input = input.Replace ("{sourcedir}", sourcedir);

			return input;
		}

		public static StepResults RemoveDirectory (BuildStep step)
		{
			DateTime start = DateTime.Now;

			StepResults sr = new StepResults ();
			sr.Command = Path.GetFileName (step.Command);

			sr.Log += string.Format ("RemoveDirectory: {0}\n", step.Arguments);

			try {
				Directory.Delete (step.Arguments, true);
				sr.Log += string.Format ("Success.");
				sr.ExitCode = 0;
			} catch (Exception ex) {
				sr.Log += string.Format ("Failed:\n{0}\n", ex.ToString ());
				sr.ExitCode = 1;
			}

			sr.ExecutionTime = DateTime.Now.Subtract (start);

			return sr;
		}

		public static StepResults MoveFile (BuildStep step)
		{
			DateTime start = DateTime.Now;

			StepResults sr = new StepResults ();
			sr.Command = Path.GetFileName (step.Command);

			string source;
			string dest;

			try {
				source = step.Arguments.Split (' ')[0];
				dest = step.Arguments.Split (' ')[1];

			} catch (Exception) {
				sr.Log += string.Format ("MoveFile: Cannot determine source and destination.\nMust be of form: <source> <dest>\nFound: {0}\n", step.Arguments);
				sr.ExecutionTime = DateTime.Now.Subtract (start);
				sr.ExitCode = 1;
				return sr;
			}
			
			sr.Log += string.Format ("MoveFile: {0} => {1}\n", source, dest);

			try {
				File.Move (source, dest);
				sr.Log += string.Format ("Success.");
				sr.ExitCode = 0;
			} catch (Exception ex) {
				sr.Log += string.Format ("Failed:\n{0}\n", ex.ToString ());
				sr.ExitCode = 1;
			}

			sr.ExecutionTime = DateTime.Now.Subtract (start);

			return sr;
		}

		public static StepResults CopyFile (BuildStep step)
		{
			DateTime start = DateTime.Now;

			StepResults sr = new StepResults ();
			sr.Command = Path.GetFileName (step.Command);

			string source;
			string dest;

			try {
				source = step.Arguments.Split (' ')[0];
				dest = step.Arguments.Split (' ')[1];

			} catch (Exception) {
				sr.Log += string.Format ("CopyFile: Cannot determine source and destination.\nMust be of form: <source> <dest>\nFound: {0}\n", step.Arguments);
				sr.ExecutionTime = DateTime.Now.Subtract (start);
				sr.ExitCode = 1;
				return sr;
			}

			sr.Log += string.Format ("CopyFile: {0} => {1}\n", source, dest);

			try {
				File.Copy (source, dest, true);
				sr.Log += string.Format ("Success.");
				sr.ExitCode = 0;
			} catch (Exception ex) {
				sr.Log += string.Format ("Failed:\n{0}\n", ex.ToString ());
				sr.ExitCode = 1;
			}

			sr.ExecutionTime = DateTime.Now.Subtract (start);

			return sr;
		}
		
		public static string CombinePaths (params object[] paths)
		{
			if (paths.Length < 1)
				return string.Empty;

			string ret = (string)paths[0];

			for (int i = 1; i < paths.Length; i++)
				ret = Path.Combine (ret, (string)paths[i]);

			return ret;
		}

		public static StepResults DownloadFile (BuildStep step)
		{
			DateTime start = DateTime.Now;

			StepResults sr = new StepResults ();
			sr.Command = Path.GetFileName (step.Command);

			sr.Log += string.Format ("DownloadFile: {0}\n", step.Arguments);

			try {
				if (!Directory.Exists (Path.GetDirectoryName (step.Arguments)))
					Directory.CreateDirectory (Path.GetDirectoryName (step.Arguments));
					
				WebClient wc = new WebClient ();
				wc.DownloadFile (step.Arguments, Path.Combine (step.WorkingDirectory, Path.GetFileName (step.Arguments)));
				
				sr.Log += string.Format ("Success.");
				sr.ExitCode = 0;
			} catch (Exception ex) {
				sr.Log += string.Format ("Failed:\n{0}\n", ex.ToString ());
				sr.ExitCode = 1;
			}

			sr.ExecutionTime = DateTime.Now.Subtract (start);

			return sr;
		}
	}
}
