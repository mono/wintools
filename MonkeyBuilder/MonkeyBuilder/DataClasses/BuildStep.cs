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
using System.Data;

namespace MonkeyBuilder
{
	public class BuildStep
	{
		public int ID { get; set; }
		public int Revision { get; set; }
		public long BuildRevisionID { get; set; }
		public string Command { get; set; }
		public string Arguments { get; set; }
		public string WorkingDirectory { get; set; }
		public bool AlwaysExecute { get; set; }
		public bool IsFatal { get; set; }
		public bool IsTestStep { get; set; }
	
		public BuildStep ()
		{
		}
		
		public BuildStep (DataRow row)
		{
			ID = (int)row["ID"];
			Revision = (int)row["Revision"];
			BuildRevisionID = (long)row["BuildRevisionID"];

			Command = Utilities.ReplaceArgs (row["Command"].ToString (), Revision.ToString ());
			Arguments = Utilities.ReplaceArgs (row["Arguments"].ToString (), Revision.ToString ());
			WorkingDirectory = Utilities.ReplaceArgs (row["WorkingDirectory"].ToString (), Revision.ToString ());
			AlwaysExecute = (bool)row["AlwaysExecute"];
			IsFatal = (bool)row["Fatal"];
			IsTestStep = (bool)row["IsTestStep"];
		}
		
		public static List<BuildStep> FromDataSet (DataSet data)
		{
			List<BuildStep> steps = new List<BuildStep> ();
			
			if (data == null || data.Tables.Count < 1)
				return steps;
				
			foreach (DataRow row in data.Tables[0].Rows)
				steps.Add (new BuildStep (row));
				
			return steps;
		}
	}
}
