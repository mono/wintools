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
using System.Xml;

namespace MonkeyBuilder.MonoCompiler
{
	public class ParallelWorkQueue
	{
		private Dictionary<string, WorkItem> items = new Dictionary<string, WorkItem> ();
		private List<string> dependencies = new List<string> ();
		
		public ParallelWorkQueue (XmlDocument doc)
		{
			foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
				if (!(node is XmlElement))
					continue;

				XmlElement xe = (XmlElement)node;

				if (xe.GetAttribute ("enabled") == "false")
					continue;

				string id = xe.GetAttribute ("id");
				string dependson = xe.GetAttribute ("dependson");

				if (id == null || id.Trim ().Length == 0)
					throw new ApplicationException ("Node is missing id attribute");

				WorkItem item = new WorkItem ();

				item.Id = id.Trim ();

				if (!string.IsNullOrEmpty (dependson))
					item.Dependencies.AddRange (ParseDependsOn (dependson));

				item.Item = xe;

				items.Add (item.Id, item);
				dependencies.Add (item.Id);
			}
		}

		// Return value: true if have work, false if done
		// if work is null, but true is returned, there is work, but it
		// is all waiting for dependencies to be filled.
		public bool GetWork (out XmlElement work)
		{
			lock (items) {
				work = null;

				if (items.Count == 0)
					return false;

				foreach (var item in items) {
					bool valid = true;

					foreach (var depends in item.Value.Dependencies) {
						if (dependencies.Contains (depends)) {
							valid = false;
							break;
						}
					}

					if (valid) {
						work = item.Value.Item;
						items.Remove (item.Key);
						return true;
					}

				}

				return true;
			}
		}
		
		public void ReportWorkCompleted (XmlElement xe)
		{
			lock (items)
				dependencies.Remove (xe.GetAttribute ("id").Trim ());
		}

		private List<string> ParseDependsOn (string dependson)
		{
			List<string> retval = new List<string> ();

			string[] pieces = dependson.Trim ().Split (',');

			foreach (string piece in pieces) {
				retval.Add (piece.Trim ());
			}

			return retval;
		}

		private class WorkItem
		{
			public string Id { get; set; }
			public List<string> Dependencies { get; private set; }
			public XmlElement Item { get; set; }

			public WorkItem ()
			{
				Dependencies = new List<string> ();
			}
		}
	}
}
