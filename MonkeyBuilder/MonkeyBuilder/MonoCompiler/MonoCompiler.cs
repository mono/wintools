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
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Xml;

namespace MonkeyBuilder.MonoCompiler
{
	public class MonoCompiler
	{
		//static void Main (string[] args)
		//{
		//        if (args.Length != 2) {
		//                Console.WriteLine ("Usage: MonoCompiler.exe <revision> <config file>");
		//                Environment.Exit (1);
		//        }

		//        Compile (args[0], args[1]);
		//}
		SerialWorkQueue queue;
		string revision;
		StringBuilder sb = new StringBuilder ();
		
		public StepResults Compile (string revision, string configFile)
		{
			this.revision = revision;
			
			Stopwatch sw = new Stopwatch ();
			sw.Start ();

			StepResults sr = new StepResults ();
			sr.Command = "monocompiler";

			// Create a log file
			

			try {
				// Load the configuration file
				XmlDocument doc = new XmlDocument ();
				doc.Load (configFile);

				queue = new SerialWorkQueue (doc);

				List<Thread> threads = new List<Thread> ();

				for (int i = 0; i < 1; i++) {
					Thread t = new Thread (new ThreadStart (WorkerThread));
					threads.Add (t);
					t.Start ();
				}

				foreach (Thread t in threads)
					t.Join ();

				// Report results
				sr.ExitCode = 0;
				sb.AppendLine ("Done");
			} catch (Exception ex) {
				//Console.WriteLine (ex.ToString ());
				sr.ExitCode = 1;
				sb.AppendFormat ("MonoCompiler Error:\n{0}\n", ex.ToString ());
			}

			sw.Stop ();
			//Console.WriteLine (sw.Elapsed);
			sr.ExecutionTime = sw.Elapsed;
			sr.Log = sb.ToString ();

			return sr;
		}
		
		public void WorkerThread ()
		{
			XmlElement xe = null;
			
			// Loop through the tasks in the config file
			while (queue.GetWork (out xe)) {
				if (xe == null) {
					Thread.Sleep (5 * 1000);
					continue;
				}
				
				BaseTask task = TaskFactory.Create (xe.Name);
				task.Revision = revision;
				task.Log = sb;

				task.Execute (xe);
				queue.ReportWorkCompleted (xe);
			}
		}
	}
}