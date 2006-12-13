using System;
using System.IO;

namespace Mfconsulting.Vsprj2make
{
    /// <summary>
    /// MonoLaunchHelper is a support class meant to facilitate
    /// interaction between the Add-in's main class Connect, and
    /// the monoLaunch executable.
    /// </summary>
    public class MonoLaunchHelper
    {
        private string m_MonoLaunchCPath;

        public string MonoLaunchCPath
        {
            get { return m_MonoLaunchCPath; }
        }

        private string m_MonoLaunchWPath;

        public string MonoLaunchWPath
        {
            get { return m_MonoLaunchWPath; }
        }

        public MonoLaunchHelper()
        {
            if (IsMonoLaunchAvailable() == false)
            {
                throw new Exception("monoLaunchC may not be installed");
            }
        }

        protected bool IsMonoLaunchAvailable()
        {
            string baseDirectory;
            string strTmp;

            try
            {
                System.Reflection.Assembly myAddIn = System.Reflection.Assembly.GetCallingAssembly();
                baseDirectory = System.IO.Path.GetDirectoryName(myAddIn.Location);
                strTmp = System.IO.Path.Combine(baseDirectory, "monoLaunchC.exe");
                m_MonoLaunchCPath = (System.IO.File.Exists(strTmp) == true) ? strTmp : null;
                strTmp = System.IO.Path.Combine(baseDirectory, "monoLaunchW.exe");
                m_MonoLaunchWPath = (System.IO.File.Exists(strTmp) == true) ? strTmp : null;
                if (m_MonoLaunchCPath != null && m_MonoLaunchWPath != null)
                    return true;
            }
            catch (Exception)
            {
                m_MonoLaunchCPath = null;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Returns the path to the Xsp.exe or Xsp2.exe based on the XspExeSelection
        /// </summary>
        public string GetXspExePath(int xspExeSelection)
        {
            RegistryHelper regHlpr = new RegistryHelper();
            string strMonoBasePath;
            string retVal = "";

            if (xspExeSelection > 2 || xspExeSelection < 1)
                throw new Exception("Invalid Xsp.exe selection. Value must be either 1 or 2");

            strMonoBasePath = regHlpr.GetMonoBasePath();

            if (xspExeSelection == 1)
            {
                // Handle when it is Xsp.exe
                retVal = Path.Combine(
                    strMonoBasePath,
                    @"lib\mono\1.0\xsp.exe"
                    );
                if (File.Exists(retVal) == true)
                    return retVal;
                retVal = Path.Combine(
                    strMonoBasePath,
                    @"lib\xsp\1.0\xsp.exe"
                    );
                if (File.Exists(retVal) == true)
                    return retVal;
            }

            if (xspExeSelection == 2)
            {
                // Handle when it is Xsp2.exe
                retVal = Path.Combine(
                    strMonoBasePath,
                    @"lib\mono\2.0\xsp2.exe"
                    );
                if (File.Exists(retVal) == true)
                    return retVal;
                retVal = Path.Combine(
                    strMonoBasePath,
                    @"lib\xsp\2.0\xsp2.exe"
                    );
                if (File.Exists(retVal) == true)
                    return retVal;
            }

            return retVal;
        }
    }
}
