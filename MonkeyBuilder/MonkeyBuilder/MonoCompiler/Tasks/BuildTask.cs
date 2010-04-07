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
using System.Xml;
using MonkeyBuilder.Properties;

namespace MonkeyBuilder.MonoCompiler
{
	public class BuildTask : BaseTask
	{
		public override void Execute (XmlElement config)
		{
			string tool_path = Utilities.CombinePaths (Environment.CurrentDirectory, "build", "lib", "mono", "2.0", "gmcs.exe");
			string mono_path = Utilities.CombinePaths (Environment.CurrentDirectory, "build", "bin", "mono.exe");
			string output_path = Utilities.ReplaceArgs (config.GetAttribute ("destination"), Revision);

			if (config.GetAttribute ("mono") == "install") {
				tool_path = Utilities.CombinePaths (Settings.Default.installedmono, "lib", "mono", "2.0", "gmcs.exe");
				mono_path = Utilities.CombinePaths (Settings.Default.installedmono, "bin", "mono.exe");
			}

			Log.AppendFormat ("Building: {0}\n", config.GetAttribute ("name"));
			Console.WriteLine ("Building: {0}", config.GetAttribute ("name"));

			if (!Directory.Exists (Path.GetDirectoryName (output_path)))
				Directory.CreateDirectory (Path.GetDirectoryName (output_path));

			string args = string.Empty;

			args = string.Format ("\"{0}\"", tool_path);
			args += " /codepage:65001 -optimize";

			//args += " -d:" + profile_defines;

			if (config["DefineConstants"] != null)
				args += " -d:" + config["DefineConstants"].InnerText;
			if (config["AllowUnsafeBlocks"] != null)
				args += " -unsafe";
			if (config["OutputType"] != null)
				args += " -target:" + config["OutputType"].InnerText;
			if (config["IgnoreWarnings"] != null)
				args += " -nowarn:" + config["IgnoreWarnings"].InnerText;
			if (config["Debug"] == null || config["Debug"].InnerText == "true")
				args += " -debug";
			if (config["NoConfig"] == null || config["NoConfig"].InnerText == "true")
				args += " -noconfig";
			if (config["NoStandardLib"] != null && config["NoStandardLib"].InnerText == "true")
				args += " -nostdlib";
			if (config["KeyFile"] != null)
				args += string.Format (" -keyfile:{0}", Utilities.ReplaceArgs (config["KeyFile"].InnerText, Revision));

			foreach (XmlElement reference in config.SelectNodes ("References/Reference")) {
				if (!string.IsNullOrEmpty (reference.GetAttribute ("alias")))
					args += string.Format (" -r:{0}={1}", reference.GetAttribute ("alias"), reference.InnerText);
				else
					args += " -r:" + reference.InnerText;
			}

			args += string.Format (" -out:{0}", output_path);

			foreach (XmlElement sourcenode in config.SelectNodes ("Sources/Source")) {
				if (sourcenode.GetAttribute ("type") == "list")
					args += " @" + sourcenode.InnerText;
				else
					args += " " + sourcenode.InnerText;
			}

			string working_path = Utilities.ReplaceArgs (config["SourcePath"].InnerText, Revision);

			CommandLineResults results = CommandLineRunner.ExecuteCommand (mono_path, working_path, args);
			
			Log.AppendLine (results.Output);

			if (results.ExitCode != 0) {
				Log.AppendFormat ("BuildTask returned: {0}\n\n--- Error Log ---\n", results.ExitCode);
				Log.AppendLine (results.ErrorOutput);
				Console.WriteLine ("Build Failed");
				Console.WriteLine (results.ErrorOutput);
				throw new ApplicationException (string.Format ("BuildTask returned: {0}", results.ExitCode));
			}
		}
	}
}
