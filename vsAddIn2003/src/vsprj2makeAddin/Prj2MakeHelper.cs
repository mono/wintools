using System;
using System.Diagnostics;

namespace Mfconsulting.Vsprj2make
{
	/// <summary>
	/// Summary description for Prj2MakeHelper.
	/// </summary>
	public class Prj2MakeHelper
	{
		private string m_strPrj2makePath;

		public Prj2MakeHelper()
		{
			if(IsPrj2MakeAvailable() == false)
			{
				throw new Exception("Prj2make may not be installed");
			}
		}

		public string CreateMdFiles(string strInputFilePath)
		{
			string strPrj2MakeStdOut;
			System.Text.StringBuilder strbArgLine = new System.Text.StringBuilder();
			char []carSpecialCharacters = {' ','\t', ',', '*', '%', '!'};
			MonoLaunchHelper launchHlpr = new MonoLaunchHelper();

			// Build the command line parameters
			strbArgLine.AppendFormat("\"{0}\" ", m_strPrj2makePath);

			strbArgLine.Append("--csproj2prjx ");

			if(strInputFilePath.IndexOfAny(carSpecialCharacters)== -1)
			{
				strbArgLine.Append(strInputFilePath);
			}
			else
			{
				strbArgLine.AppendFormat("\"{0}\"",
					strInputFilePath);
			}
            
			ProcessStartInfo pi = new ProcessStartInfo();
			pi.FileName = launchHlpr.MonoLaunchWPath;
			// pi.FileName = m_strPrj2makePath;
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
				Console.WriteLine("Couldn't run prj2makesharpWin32: " + e.Message);
				return null;
			}
			
			strPrj2MakeStdOut = p.StandardOutput.ReadToEnd ();		
			p.WaitForExit ();
			if (p.ExitCode != 0) 
			{
				Console.WriteLine("Error running prj2makesharpWin32. Check the above output.");
				return null;
			}

			if (strPrj2MakeStdOut != null)
			{
				p.Close ();
				return strPrj2MakeStdOut;
			}

			p.Close ();

			return null;
		}
		
		public string CreateMakeFile(bool IsCsc, bool IsNmake, string strInputFilePath)
		{
			string strPrj2MakeStdOut;
			System.Text.StringBuilder strbArgLine = new System.Text.StringBuilder();
			char []carSpecialCharacters = {' ','\t', ',', '*', '%', '!'};
			MonoLaunchHelper launchHlpr = new MonoLaunchHelper();

			// Build the command line parameters
			strbArgLine.AppendFormat("\"{0}\" ", m_strPrj2makePath);

			if(IsCsc == true)
			{
				strbArgLine.Append("-c ");
			}

			if(IsNmake == true)
			{
				strbArgLine.Append("-n ");
			}

			if(strInputFilePath.IndexOfAny(carSpecialCharacters)== -1)
			{
				strbArgLine.Append(strInputFilePath);
			}
			else
			{
				strbArgLine.AppendFormat("\"{0}\"",
					strInputFilePath);
			}
            
			ProcessStartInfo pi = new ProcessStartInfo();
			pi.FileName = launchHlpr.MonoLaunchWPath;
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
				Console.WriteLine("Couldn't run prj2makesharpWin32: " + e.Message);
				return null;
			}
			
			strPrj2MakeStdOut = p.StandardOutput.ReadToEnd ();		
			p.WaitForExit ();
			if (p.ExitCode != 0) 
			{
				Console.WriteLine("Error running prj2makesharpWin32. Check the above output.");
				return null;
			}

			if (strPrj2MakeStdOut != null)
			{
				p.Close ();
				return strPrj2MakeStdOut;
			}

			p.Close ();

			return null;
		}

		protected bool IsPrj2MakeAvailable()
		{
			string baseDirectory;
			string strMonoBasePath;
			string strTmp;
			RegistryHelper regH = null;
			
			try
			{
				regH = new RegistryHelper();
				
				// This acts as a test to see if mono is installed
				strMonoBasePath = regH.GetMonoBasePath();
				System.Reflection.Assembly myAddIn = System.Reflection.Assembly.GetCallingAssembly();
				baseDirectory = System.IO.Path.GetDirectoryName(myAddIn.Location);
				strTmp = System.IO.Path.Combine(baseDirectory, "prj2makesharpWin32.exe");
				m_strPrj2makePath = (System.IO.File.Exists(strTmp) == true) ? strTmp : null;
				if(m_strPrj2makePath != null)
					return true;
			}
			catch(Exception)
			{
				m_strPrj2makePath = null;
				return false;
			}
			return false;
		}
	}
}
