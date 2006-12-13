using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace Mfconsulting.Vsprj2make
{
	/// <summary>
	/// Summary description for Drive.
	/// </summary>
	public class Drive
	{
		static System.Collections.ArrayList g_straDirList = new ArrayList();
		static System.Collections.ArrayList g_straFileList = new ArrayList();
		static char []g_chaSeparator = {';'};
		static string g_strBlacListedDirectories = "CVS;obj;.svn";
		static string g_strBlacListedFiles = ".suo;.cvsignore;.vssscc;.vspscc";

		public Drive()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void Main(string[] args)
		{
			string strOutFile;
			string strSolutionFile;
			MainOpts optObj = new MainOpts();    		
			ZipCompressor zcObj = new ZipCompressor();
			int i;

			// Handle command line arguments    	
			optObj.ProcessArgs(args);

			// Hangle no parameters being passed
			if(args.Length < 1 || optObj.SolutionFile.Length < 1)
			{
				optObj.DoHelp();
				return;
			}

			// Check for BlackListed Extensions being passed
			if(optObj.Extensions.Length > 0)
			{
				g_strBlacListedFiles = optObj.Extensions;
			}

			// Check for BlackListed directories being passed
			if(optObj.BlacListedDirectories.Length > 0)
			{
				g_strBlacListedDirectories = optObj.BlacListedDirectories;
			}

			strSolutionFile = optObj.SolutionFile;

			if(optObj.OutputFilePath.Length > 0)
			{
				strOutFile = optObj.OutputFilePath;
			}
			else
			{
				strOutFile = 
					Path.Combine(
					Path.GetDirectoryName(strSolutionFile),
					(Path.GetFileNameWithoutExtension(strSolutionFile) + ".zip")
					);
			}

			if(System.IO.Path.GetExtension(strSolutionFile).ToUpper().CompareTo(".SLN") != 0)
			{
				Console.WriteLine("The file you submited does not seem like a Visual Studio .NET Solution file.");
				return;
			}

			// Parse solution
			Drive.ParseSolution(strSolutionFile);

			// build a string array of the base solution directory
			// and the projects directory
			string []straBaseDirs = new string[g_straDirList.Count];
			for(i = 0; i < straBaseDirs.Length; i++)
			{
				straBaseDirs[i] = (string)g_straDirList[i];
				AddFilesToFileList(straBaseDirs[i]);
			}
			
			// Clear the global Directory ArrayList
			g_straDirList.Clear();

			// Build the File ArrayList
			string []straFilenames = new string[g_straFileList.Count];
			for(i = 0; i < straFilenames.Length; i++)
			{
				straFilenames[i] = (string)g_straFileList[i];
			}

			// Clear the global File ArrayList
			g_straFileList.Clear();

            // Invoke the Programs main functionality and purpose
			zcObj.CompressionLevel = optObj.Level;
			zcObj.CreateZipFile(straFilenames, strOutFile);
		}

		#region Support functions

		/// <summary>
		/// Parse through all the projects in a given solution
		/// </summary>
		/// <param name="strFname">The fully qualified path to the solution file.</param>
		static void ParseSolution(string strFname)
		{
			string strSolutionDir;
			FileStream fis = new FileStream(
				strFname, 
				FileMode.Open, 
				FileAccess.Read, 
				FileShare.Read
				);

			StreamReader reader = new StreamReader(fis);
			Regex regex = new Regex(@"Project\(""\{(.*)\}""\) = ""(.*)"", ""(.*)"", ""(\{.*\})""");
			
			// Get the solution directory and added
			strSolutionDir = System.IO.Path.GetDirectoryName(strFname);
			
			// Change the current directory to the solution directory
			System.IO.Directory.SetCurrentDirectory(strSolutionDir);

			Drive.g_straDirList.Add(strSolutionDir);

			while (true)
			{
				string s = reader.ReadLine();
				Match match;
    
				match = regex.Match(s);
				if (match.Success)
				{
					string projectName = match.Groups[2].Value;
					string csprojPath = match.Groups[3].Value;
					string projectGuid = match.Groups[4].Value;
    			
					if (csprojPath.StartsWith("http://"))
					{
						Console.WriteLine("WARNING: got http:// project, guessing actual path.");
						csprojPath = Path.Combine(projectName, Path.GetFileName(csprojPath));
					}
					if (csprojPath.EndsWith (".csproj"))
					{
						string strProjDir =	System.IO.Path.GetDirectoryName(
							System.IO.Path.GetFullPath(csprojPath));

						if(strProjDir.CompareTo(strSolutionDir) != 0)
						{
							Drive.g_straDirList.Add(strProjDir);
							// Recurse to look for subdirs
							ListSubDirs(strProjDir);
						}
						else
						{
							ListSubDirs(strProjDir);
						}
					}
				}
    
				if (s.StartsWith("Global"))
				{
					break;
				}
			}
		}

		/// <summary>
		/// Iterates through all of the directories (if any) that
		/// are contained within a given directory
		/// </summary>
		/// <param name="strInDirPath">An imput string that 
		/// represents the directory to search in</param>
		public static void ListSubDirs(string strInDirPath)
		{
			System.IO.DirectoryInfo di = new DirectoryInfo(strInDirPath);

			foreach(System.IO.DirectoryInfo diObj in di.GetDirectories())
			{
				// Determine if is fit
				if(IsDirBlackListed(diObj.Name) == false)
				{
					g_straDirList.Add(diObj.FullName);
					ListSubDirs(diObj.FullName);
				}
			}
		}

		/// <summary>
		/// Determines if a directory name is listed on the ignore.
		/// </summary>
		/// <param name="strInDirName">String value that represents the name to check.</param>
		/// <returns>Returns true if black listed or false otherwise</returns>
		public static bool IsDirBlackListed(string strInDirName)
		{
			string []straBlackListItems = g_strBlacListedDirectories.Split(g_chaSeparator);

			foreach(string strDirName in straBlackListItems)
			{
				if(strInDirName.ToUpper().CompareTo(strDirName.ToUpper()) == 0)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Determines if a given file name is in the Black List
		/// </summary>
		/// <param name="strInFileName">The file name to check against the list</param>
		/// <returns>Returns true if black listed or false otherwise</returns>
		public static bool IsFileBlackListed(string strInFileName)
		{
			string []straBlackListItems = g_strBlacListedFiles.Split(g_chaSeparator);

			foreach(string strExtention in straBlackListItems)
			{
				if(Path.GetExtension(strInFileName).ToUpper().CompareTo(strExtention.ToUpper()) == 0)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Extract the list of files from a given directory and 
		/// inserts them into the global File ArrayList
		/// </summary>
		/// <param name="strInDirPath">The full directory path to look for files</param>
		public static void AddFilesToFileList(string strInDirPath)
		{
			string []straFiles = Directory.GetFiles(strInDirPath);

			foreach(string strFileName in straFiles)
			{
				// Check to see if blacklisted
				if(IsFileBlackListed(strFileName) == false)
				{
					Drive.g_straFileList.Add(strFileName);
				}
			}
		}
		
		#endregion
	}
}
