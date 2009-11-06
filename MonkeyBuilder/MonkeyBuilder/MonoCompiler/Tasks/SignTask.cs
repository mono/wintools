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
using System.Xml;
using MonkeyBuilder.Properties;

namespace MonkeyBuilder.MonoCompiler
{
	public class SignTask : BaseTask
	{
		public override void Execute (XmlElement config)
		{
			string tool_path = Utilities.CombinePaths (Environment.CurrentDirectory, "build", "lib", "mono", "2.0", "sn.exe");
			string mono_path = Utilities.CombinePaths (Environment.CurrentDirectory, "build", "bin", "mono.exe");
			string assembly = Utilities.ReplaceArgs (config.GetAttribute ("assembly"), Revision);
			string keyfile = Utilities.ReplaceArgs (config.GetAttribute ("key"), Revision);
			string args = string.Format ("\"{0}\" -q -R \"{1}\" \"{2}\"", tool_path, assembly, keyfile);

			CommandLineRunner.ExecuteCommand (mono_path, null, args);
		}
	}
}
