using System;
using System.Diagnostics; 

namespace Mfconsulting.Vsprj2make
{
	/// <summary>
	/// Summary description for CreateZipHelper.
	/// </summary>
	public class CreateZipHelper
	{
		private string m_strCreateZipPath;

		public CreateZipHelper()
		{
			if(IsCreateZipAvailable() == false)
			{
				throw new Exception("CreateZip may not be installed");
			}
		}

		public string CreateZipFile(string strInputFilePath, string strIgnoredDirs, string strIgnoredEx, int nLevel)
		{
			string strCreateZipStdOut;
			System.Text.StringBuilder strbArgLine = new System.Text.StringBuilder();
			char []carSpecialCharacters = {' ','\t', ',', '*', '%', '!'};

			// Build the command line parameters
			if(strInputFilePath.IndexOfAny(carSpecialCharacters)== -1)
			{
				strbArgLine.AppendFormat("-f {0}",
					strInputFilePath);
			}
			else
			{
				strbArgLine.AppendFormat("-f \"{0}\"",
					strInputFilePath);
			}

			if(strIgnoredDirs.Length > 1)
			{
				strbArgLine.AppendFormat(" --IgnoredDirectories \"{0}\"", strIgnoredDirs);
			}
            
			if(strIgnoredEx.Length > 1)
			{
				strbArgLine.AppendFormat(" --IgnoredExtensions \"{0}\"", strIgnoredEx);
			}
            
			strbArgLine.AppendFormat(" -l {0}", nLevel.ToString());
            
			ProcessStartInfo pi = new ProcessStartInfo();
			pi.FileName = m_strCreateZipPath;
			pi.RedirectStandardOutput = true;
			pi.UseShellExecute = false;
			pi.CreateNoWindow = true;
			pi.Arguments = strbArgLine.ToString();
			Process p = null;
			try 
			{
				p = Process.Start (pi);
			} 
			catch (Exception e) 
			{
				Console.WriteLine("Couldn't run CreateZipFromSln: " + e.Message);
				return "Couldn't run CreateZipFromSln: " + e.Message;
			}
			
			strCreateZipStdOut = p.StandardOutput.ReadToEnd ();		
			p.WaitForExit ();
			if (p.ExitCode != 0) 
			{
				Console.WriteLine("Error running CreateZipFromSln. Check the above output.");
				return null;
			}

			if (strCreateZipStdOut != null)
			{
				p.Close ();
				return strCreateZipStdOut;
			}

			p.Close ();

			return null;
		}

		protected bool IsCreateZipAvailable()
		{
			string baseDirectory;
			string strTmp;
			
			try
			{
				System.Reflection.Assembly myAddIn = System.Reflection.Assembly.GetCallingAssembly();
				baseDirectory = System.IO.Path.GetDirectoryName(myAddIn.Location);
				strTmp = System.IO.Path.Combine(baseDirectory, "CreateZipFromSln.exe");
				m_strCreateZipPath = (System.IO.File.Exists(strTmp) == true) ? strTmp : null;
				if(m_strCreateZipPath != null)
					return true;
			}
			catch(Exception)
			{
				m_strCreateZipPath = null;
				return false;
			}
			return false;
		}
	}
}
