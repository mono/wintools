using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;


namespace Mfconsulting.General.Prj2Make
{
	class CsprojInfo2005
	{
		public readonly string name;
		public readonly string guid;
		public readonly string csprojpath;
		public string makename;
		public string makename_ext;
		public string assembly_name;
		public string res;
		public string src;
		private bool m_bAllowUnsafeCode;
		private Mfconsulting.General.Prj2Make.Schema.Csproj2005.Project m_projObject;
    
		public string ext_refs = "";
		public string switches = "";

		public bool AllowUnsafeCode
		{
			get { return m_bAllowUnsafeCode; }
		}
    
		public Mfconsulting.General.Prj2Make.Schema.Csproj2005.Project Proyecto
		{
			get { return m_projObject; }
		}
    
		// Project desirialization
		protected Mfconsulting.General.Prj2Make.Schema.Csproj2005.Project LoadPrjFromFile (string strIn)
		{
			FileStream fs = new FileStream (strIn, FileMode.Open);
            Mfconsulting.General.Prj2Make.Schema.Csproj2005.Project prjObj = null;

            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Mfconsulting.General.Prj2Make.Schema.Csproj.VisualStudioProject));
                prjObj = (Mfconsulting.General.Prj2Make.Schema.Csproj2005.Project)xmlSer.Deserialize(fs);
            }
            catch (Exception exc)
            {
                Trace.WriteLine(String.Format("Error while deserializing file: {0}\n{1}", strIn, exc.Message));
            }
	    
			fs.Close();
	    
			return (prjObj);
		}

		public CsprojInfo2005(bool isUnixMode, bool isMcsMode, string name, string guid, string csprojpath)
		{
			this.name = name;
			this.guid = guid;
			this.csprojpath = csprojpath;
    
			makename = name.Replace('.','_').ToUpper();
			makename_ext = makename + "_EXT";
			m_bAllowUnsafeCode = false;
    
			// convert backslashes to slashes    		
			csprojpath = csprojpath.Replace("\\", "/");

			// loads the file in order to deserialize and
			// build the object graph
			try
			{
				m_projObject = LoadPrjFromFile (csprojpath);
			} 

			catch (Exception exc)
			{
				Console.WriteLine (
					String.Format ("Could not load the file {0}\nException: {1}",
					csprojpath,
					exc.Message)
					);
				return;			
			}

			// Establish if the allow unsafe code flag is true
            foreach (Mfconsulting.General.Prj2Make.Schema.Csproj2005.PropertyGroup propGrp in m_projObject.PropertyGroup)
            {
                if (propGrp.AllowUnsafeBlocks == true)
                    m_bAllowUnsafeCode = true;
            }

            foreach (Mfconsulting.General.Prj2Make.Schema.Csproj2005.PropertyGroup propGrp in m_projObject.PropertyGroup)
            {
                switch (propGrp.OutputType)
                {
                    case "Library":
                        makename_ext = makename + "_DLL";
                        assembly_name = m_projObject.PropertyGroup[0].AssemblyName + ".dll";
                        switches += " -target:library";
                        break;

                    case "Exe":
                        makename_ext = makename + "_EXE";
                        assembly_name = m_projObject.PropertyGroup[0].AssemblyName + ".exe";
                        switches += " -target:exe";
                        break;

                    case "WinExe":
                        makename_ext = makename + "_EXE";
                        assembly_name = m_projObject.PropertyGroup[0].AssemblyName + ".exe";
                        switches += " -target:winexe";
                        break;

                    default:
                        throw new NotSupportedException("Unsupported OutputType: " + propGrp.OutputType);
                }
            }
    
			src = "";    
			string basePath = Path.GetDirectoryName(csprojpath);
			string s;

            foreach (object[][] itemGrpArray in m_projObject.ItemGroup)
            {
                foreach (object[] itemGrp in itemGrpArray)
                {
                }
            }
    
            //foreach (Mfconsulting.General.Prj2Make.Schema.Csproj.File fl in m_projObject.CSHARP.Files.Include)
            //{
            //    if(fl.BuildAction == Mfconsulting.General.Prj2Make.Schema.Csproj.FileBuildAction.Compile)
            //    {
            //        if (src != "")
            //        {
            //            src += " \\\n\t";
            //        }
	    			
            //        s = System.IO.Path.Combine(basePath, fl.RelPath);
            //        s = s.Replace("\\", "/");
            //        if (SlnMaker.slash != "/")
            //            s = s.Replace("/", SlnMaker.slash);
	
            //        // Test for spaces
            //        if (isUnixMode == false) {
            //            // We are in win32 using a cmd.exe or other
            //            // DOS shell
            //            if(s.IndexOf(' ') > -1) {
            //                src += String.Format("\"{0}\"", s);
            //            } else {
            //                src += s;
            //            }
            //        } else {
            //            // We are in *NIX or some other
            //            // GNU like shell
            //            src += s.Replace(" ", "\\ ");
            //        }
            //    }    			
            //}
    		
            //res = "";
            //string rootNS = m_projObject.CSHARP.Build.Settings.RootNamespace;
            //string relPath;
            //foreach (Mfconsulting.General.Prj2Make.Schema.Csproj.File fl in m_projObject.CSHARP.Files.Include)
            //{
            //    if(fl.BuildAction == Mfconsulting.General.Prj2Make.Schema.Csproj.FileBuildAction.EmbeddedResource)
            //    {
            //        if (res != "") {
            //            res += " \\\n\t";
            //        }
    			
            //        relPath = fl.RelPath.Replace("\\", "/");
            //        s = System.IO.Path.Combine(basePath, relPath);
            //        s = String.Format("-resource:{0},{1}", s, rootNS + "." + relPath.Replace("/", "."));
            //        s = s.Replace("\\", "/");
            //        if (SlnMaker.slash != "/")
            //            s = s.Replace("/", SlnMaker.slash);
            //        res += s;
            //    }
            //} 		
		}
	}    
}