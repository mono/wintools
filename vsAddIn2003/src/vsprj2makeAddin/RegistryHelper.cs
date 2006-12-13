using System;

namespace Mfconsulting.Vsprj2make
{
	/// <summary>
	/// Summary description for RegistryHelper.
	/// </summary>
	public class RegistryHelper
	{
		private Microsoft.Win32.RegistryKey m_MonoSoftwareKey = null;
		private Microsoft.Win32.RegistryKey m_GtkSharpSoftwareKey = null;
		private Microsoft.Win32.RegistryKey m_Prj2MakeSoftwareKey = null;

		public Microsoft.Win32.RegistryKey MonoSoftwareKey
		{
			get { return m_MonoSoftwareKey; }
		}

		public Microsoft.Win32.RegistryKey GtkSharpSoftwareKey
		{
			get { return m_GtkSharpSoftwareKey; }
		}

		public Microsoft.Win32.RegistryKey Prj2MakeSoftwareKey
		{
			get { return m_Prj2MakeSoftwareKey; }
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		public RegistryHelper()
		{
			m_Prj2MakeSoftwareKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
				@"SOFTWARE\MFConsulting\VSPrj2Make\settings"
				);

			if(m_Prj2MakeSoftwareKey == null)
			{
				throw new Exception("Prj2Make Add-in may not be installed correctly.");
			}
		}

		#region Packaging options
		
		public int CompressionLevel
		{
			get 
			{
				int nRetVal = 
					(int)Prj2MakeSoftwareKey.GetValue("CompressionLevel", 6);
				return nRetVal; 
			}
			set { Prj2MakeSoftwareKey.SetValue("CompressionLevel", value);  }
		}

		public string IgnoredExtensions
		{
			get 
			{ 
				return (string)Prj2MakeSoftwareKey.GetValue(
					"IgnoredExtension",
					".suo;.cvsignore;.vssscc;.vspscc"
					);
			}
			set { Prj2MakeSoftwareKey.SetValue("IgnoredExtension", value); }
		}
		
		public string IgnoredDirectories
		{
			get 
			{ 
				return (string)Prj2MakeSoftwareKey.GetValue(
					"IgnoredDirectories",
					"CVS;obj;.svn"
					);
			}
			set { Prj2MakeSoftwareKey.SetValue("IgnoredDirectories", value); }
		}		
		
		#endregion

		#region XSP settings
		
		public int Port
		{
			get { return (int)m_Prj2MakeSoftwareKey.GetValue("XSPPort", 8189); }
			set { m_Prj2MakeSoftwareKey.SetValue("XSPPort", value); }
		}

		public int XspExeSelection
		{
			get { return (int)m_Prj2MakeSoftwareKey.GetValue("XspExeSelection", 1); }
			set { m_Prj2MakeSoftwareKey.SetValue("XspExeSelection", value); }
		}

		#endregion

		#region Prj2make-Sharp Makefile and MD file creation
		public string MonoLibPath
		{
			get
			{
                return (string)Prj2MakeSoftwareKey.GetValue("MonoLibPath",
					"/usr/lib/mono/1.0,/usr/lib/mono/gtk-sharp-2.0");
			}
			set { Prj2MakeSoftwareKey.SetValue("MonoLibPath", value); }
		}

		#endregion     

		#region Mono Key
		/// <summary>		
		/// Tries to pupolate the Mono Registry key		
		/// </summary>
		private void LoadMonoKey()		
		{			
			Microsoft.Win32.RegistryKey MonoRoot = null;
			MonoRoot = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
				@"SOFTWARE\Novell\Mono"
				);
            if(MonoRoot == null)			
			{
				throw new Exception("Mono may not be installed correctly");			
			}
			string strMonoVersion = (string)MonoRoot.GetValue("DefaultCLR");
           	m_MonoSoftwareKey = MonoRoot.OpenSubKey(strMonoVersion);
			
			if(this.m_MonoSoftwareKey == null)
			{
				throw new Exception("Mono may not be installed correctly");    
			}		
		}

		public string GetDefaultClr()
		{
			Microsoft.Win32.RegistryKey MonoRoot = null;
			MonoRoot = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
				@"SOFTWARE\Novell\Mono"
				);
			if(MonoRoot == null)			
			{
				throw new Exception("Mono may not be installed correctly");			
			}

			return (string)MonoRoot.GetValue("DefaultCLR");
		}

		public void SetDefaultClr(string monoVersionValue)
		{
			Microsoft.Win32.RegistryKey MonoRoot = null;
			MonoRoot = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
				@"SOFTWARE\Novell\Mono", true
				);
			if(MonoRoot == null)			
			{
				throw new Exception("Mono may not be installed correctly");			
			}

			MonoRoot.SetValue("DefaultCLR", monoVersionValue);
		}

		public string[] GetMonoVersions()
		{
			Microsoft.Win32.RegistryKey MonoRoot = null;
			MonoRoot = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
				@"SOFTWARE\Novell\Mono"
				);
			if(MonoRoot == null)			
			{
				throw new Exception("A Mono installation could not be detected in your system.  Mono may not be installed.");			
			}

			return MonoRoot.GetSubKeyNames();
		}

		/// <summary>		
		/// Optains the base path where the Mono installation resides
		/// </summary>
		/// <returns>Returns a string containing the path to
		/// where the Mono installation resides
		/// </returns>
		public string GetMonoBasePath()
		{
			string strRetVal;
			if(m_MonoSoftwareKey == null)
			{
				try
				{
					LoadMonoKey();
				}
				catch(Exception exc)
				{
					throw exc;
				}
			}
            
			strRetVal = m_MonoSoftwareKey.GetValue("SdkInstallRoot", "").ToString();
            
			return strRetVal;
		}
		#endregion
	}
}
