namespace Mfconsulting.Vsprj2make
{
	using System;
	using System.Diagnostics;
	using System.Collections;
	using Microsoft.Office.Core;
	using Extensibility;
	using System.Runtime.InteropServices;
	using EnvDTE;
	using System.Windows.Forms;

	#region Read me for Add-in installation and setup information.
	// When run, the Add-in wizard prepared the registry for the Add-in.
	// At a later time, if the Add-in becomes unavailable for reasons such as:
	//   1) You moved this project to a computer other than which is was originally created on.
	//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
	//   3) Registry corruption.
	// you will need to re-register the Add-in by building the MyAddin21Setup project 
	// by right clicking the project in the Solution Explorer, then choosing install.
	#endregion
	
	/// <summary>
	///   The object for implementing an Add-in.	
	/// </summary>	
	/// <seealso class='IDTExtensibility2' />
	[GuidAttribute("1B888614-0203-48F9-BA4C-BFE1ED9DC13D"), ProgId("vsprj2make.Connect")]	
	public class Connect : Object, Extensibility.IDTExtensibility2, IDTCommandTarget
	{
		private _DTE applicationObject;
		private AddIn addInInstance;
		private OutputWindowPane outputWindowPane;
		private CommandBarComboBox versionComboBox = null;
		private CommandBarButton testInMonoCommandBarButton = null;
		
		private IDictionary _commands = new Hashtable();
		private IList _commandBars = new ArrayList();

		public string ProgID
		{
			get { return addInInstance.ProgID; }
		}

		public _DTE DTE
		{
			get { return applicationObject; }
		}

		/// <summary>
		///		Implements the constructor for the Add-in object.
		///		Place your initialization code within this method.
		/// </summary>
		public Connect()
		{
			// MessageBox.Show("In Connect ctor", "Prj2make Debug");
		}


		/// <summary>
		///      Implements the OnConnection method of the IDTExtensibility2 interface.
		///      Receives notification that the Add-in is being loaded.
		/// </summary>
		/// <param term='application'>
		///      Root object of the host application.
		/// </param>
		/// <param term='connectMode'>
		///      Describes how the Add-in is being loaded.
		/// </param>
		/// <param term='addInInst'>
		///      Object representing this Add-in.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref System.Array custom)
		{
			Mfconsulting.Vsprj2make.RegistryHelper regHlpr = new RegistryHelper();
			string []monoVersions = null;
			
			try
			{
				monoVersions = regHlpr.GetMonoVersions();
			}
			catch(Exception exc)
			{
				// Mono may not be installed
				MessageBox.Show(exc.Message, "Prj2make Error",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				return; // Pull the cord and discontinue loading the add-in
			}

			applicationObject = (_DTE)application;
			addInInstance = (AddIn)addInInst;
			int selectedIndexForComboBox = 1;
			
			EnvDTE.Events events = applicationObject.Events;
			OutputWindow outputWindow = (OutputWindow)applicationObject.Windows.Item(Constants.vsWindowKindOutput).Object;
			outputWindowPane = outputWindow.OutputWindowPanes.Add("Prj2Make Messages");

			object []contextGUIDS = new object[] { };
			Commands commands = applicationObject.Commands;
			_CommandBars commandBars = applicationObject.CommandBars;
			CommandBar cmdBarMonoBarra;

			CommandBar commandBar = (CommandBar)commandBars["Tools"];
			CommandBarPopup popMenu;	// Prj2Make popupmenu
			CommandBarPopup popMenu2;	// Explorer Current Project

			// Creates a more legible representation of the
			// command bar control collection contained in 
			// the Tools command bar
			CommandBarControls commandBarControls;
			commandBarControls = commandBar.Controls;

			// Create Makefile
			Command command1 = null;
			// Generate MonoDevelop files
			Command command2 = null;
			// Import MonoDevelop Solutions
			Command command3 = null;
			// Run on Mono
			Command command5 = null;
			// vsprj2make Options
			Command command6 = null;
			// Generate a distribution unit
			Command command7 = null;
			// Explore current solution
			Command command8 = null;			
			// Explore current Project
			Command command9 = null;
			
			
			// ------------- Add Pop-up menu ----------------
			popMenu2 = (CommandBarPopup)commandBarControls.Add(
				MsoControlType.msoControlPopup,
				System.Reflection.Missing.Value, // Object ID
				System.Reflection.Missing.Value, // Object parameters
				1, // Object before
				true);

			popMenu2.Caption = "&Windows Explore";

			// ------------- Add Pop-up menu ----------------
			popMenu = (CommandBarPopup)commandBarControls.Add(
				MsoControlType.msoControlPopup,
				System.Reflection.Missing.Value, // Object ID
				System.Reflection.Missing.Value, // Object parameters
				1, // Object before
				true);

			popMenu.Caption = "Prj&2Make";

			// Add the create makefile command -- command1
			command1 = CreateNamedCommand(
				addInInstance,
				commands,
				"CreateMake",
				"Create &Makefile", 
				"Generate Makefile",
				ref contextGUIDS
				);
			
			if(command1 == null)
			{
				command1 = GetExistingNamedCommand(commands, "vsprj2make.Connect.CreateMake");
			}

			try
			{
				command1.AddControl(popMenu.CommandBar, 1);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the generate a dist unit command -- command7
			command7 = CreateNamedCommand(
				addInInstance,
				commands,
				"GenDistUnit",
				"Generate Distribution &Unit", 
				"Generates a distribution unit (zip file)",
				ref contextGUIDS
				);
			
			if(command7 == null)
			{
				command7 = GetExistingNamedCommand(commands, "vsprj2make.Connect.GenDistUnit");
			}

			try
			{
				command7.AddControl(popMenu.CommandBar, 2);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the GenMDFiles command -- command2
			command2 = CreateNamedCommand(
				addInInstance,
				commands,
				"GenMDFiles",
				"Create Mono&Develop Solution",
				"Generate MonoDevelop Solution",
				ref contextGUIDS
				);
			
			if(command2 == null)
			{
				command2 = GetExistingNamedCommand(commands, "vsprj2make.Connect.GenMDFiles");
			}

			try
			{
				command2.AddControl(popMenu.CommandBar, 3);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the PrjxToCsproj command -- command3
			command3 = CreateNamedCommand(
				addInInstance,
				commands,
				"PrjxToCsproj",
				"&Import MonoDevelop Solution...",
				"Imports a MonoDevelop Solution",
				ref contextGUIDS
				);
			
			if(command3 == null)
			{
				command3 = GetExistingNamedCommand(commands, "vsprj2make.Connect.PrjxToCsproj");
			}

			try
			{
				command3.AddControl(popMenu.CommandBar, 4);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the Ron on Mono command -- command5
			command5 = CreateNamedCommand(
				addInInstance,
				commands,
				"RunOnMono",
				"&Run on Mono", 
				"Run solution on mono",
				ref contextGUIDS
				);
			
			if(command5 == null)
			{
				command5 = GetExistingNamedCommand(commands, "vsprj2make.Connect.RunOnMono");
			}

			try
			{
				command5.AddControl(popMenu.CommandBar, 5);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the Options command -- command6
			command6 = CreateNamedCommand(
				addInInstance,
				commands,
				"Options",
				"&Options...", 
				"Options for prj2make Add-in",
				ref contextGUIDS
				);
			
			if(command6 == null)
			{
				command6 = GetExistingNamedCommand(commands, "vsprj2make.Connect.Options");
			}

			try
			{
				command6.AddControl(popMenu.CommandBar, 6);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the ExploreCurrSln command -- command8
			command8 = CreateNamedCommand(
				addInInstance,
				commands,
				"ExploreCurrSln",
				"Current &Solution", 
				"Explore the current solution",
				ref contextGUIDS
				);
			
			if(command8 == null)
			{
				command8 = GetExistingNamedCommand(commands, "vsprj2make.Connect.ExploreCurrSln");
			}

			try
			{
				command8.AddControl(popMenu2.CommandBar, 1);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Add the ExploreCurrDoc command -- command9
			command9 = CreateNamedCommand(
				addInInstance,
				commands,
				"ExploreCurrDoc",
				"Current &Document", 
				"Explore the current Document",
				ref contextGUIDS
				);
			
			if(command9 == null)
			{
				command9 = GetExistingNamedCommand(commands, "vsprj2make.Connect.ExploreCurrDoc");
			}

			try
			{
				command9.AddControl(popMenu2.CommandBar, 2);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Error during OnConnect of Add-in:\n {0}", exc.Message));
			}

			// Mono Toolbar
			CommandBar cmdBarBuild = (CommandBar)commandBars["Build"];
			
			try
			{
				cmdBarMonoBarra = (CommandBar)commandBars["MonoBarra"];
			}
			catch(Exception)
			{
				commands.AddCommandBar("MonoBarra",
					vsCommandBarType.vsCommandBarTypeToolbar,
					cmdBarBuild,
					1
					);

				cmdBarMonoBarra = (CommandBar)commandBars["MonoBarra"];
				cmdBarMonoBarra.Visible = true;
			}

			if(testInMonoCommandBarButton == null)
			{

				// Create the Ron on Mono Button
				testInMonoCommandBarButton = (CommandBarButton)cmdBarMonoBarra.Controls.Add(
					Microsoft.Office.Core.MsoControlType.msoControlButton,
					System.Reflection.Missing.Value,
					System.Reflection.Missing.Value,
					1,
					false
					);

				testInMonoCommandBarButton.Caption = "Run on &Mono";
				testInMonoCommandBarButton.DescriptionText = "Run solution with the mono runtime";
				testInMonoCommandBarButton.TooltipText = "Run on mono";
				testInMonoCommandBarButton.ShortcutText = "Run on &Mono";
				testInMonoCommandBarButton.Style = MsoButtonStyle.msoButtonCaption;
				testInMonoCommandBarButton.Click +=new _CommandBarButtonEvents_ClickEventHandler(testInMonoCommandBarButton_Click);
			}

			if(versionComboBox == null)
			{

				// Create the combobox
				versionComboBox = (CommandBarComboBox)cmdBarMonoBarra.Controls.Add(
					Microsoft.Office.Core.MsoControlType.msoControlDropdown,
					System.Reflection.Missing.Value,
					System.Reflection.Missing.Value,
					2,
					false
					);

				for(int i = 0; i < monoVersions.Length; i++)
				{
					versionComboBox.AddItem(monoVersions[i], i + 1);
					if(monoVersions[i].CompareTo(regHlpr.GetDefaultClr()) == 0)
					{
						selectedIndexForComboBox = i + 1;
					}
				}

				versionComboBox.Change +=new _CommandBarComboBoxEvents_ChangeEventHandler(versionComboBox_Change);

				// Select the active index based on
				// the current mono version
				versionComboBox.ListIndex = selectedIndexForComboBox;
			}
		}

		/// <summary>
		///     Implements the OnDisconnection method of the IDTExtensibility2 interface.
		///     Receives notification that the Add-in is being unloaded.
		/// </summary>
		/// <param term='disconnectMode'>
		///      Describes how the Add-in is being unloaded.
		/// </param>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref System.Array custom)
		{
			Commands commands = applicationObject.Commands;
			_CommandBars commandBars = applicationObject.CommandBars;
			CommandBar cmdBarMonoBarra = (CommandBar)commandBars["MonoBarra"];

			try
			{
				cmdBarMonoBarra.Visible = false;
			}
			catch(Exception)
			{
			}

		}

		/// <summary>
		///      Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		///      Receives notification that the collection of Add-ins has changed.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnAddInsUpdate(ref System.Array custom)
		{
			// MessageBox.Show("In OnAddInsUpdate", "Prj2make Debug");
		}

		/// <summary>
		///      Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		///      Receives notification that the host application has completed loading.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref System.Array custom)
		{
			// MessageBox.Show("In OnStartupComplete", "Prj2make Debug");
		}

		/// <summary>
		///      Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		///      Receives notification that the host application is being unloaded.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref System.Array custom)
		{
			// MessageBox.Show("In OnBeginShutdown", "Prj2make Debug");
		}
		
		/// <summary>
		///      Implements the QueryStatus method of the IDTCommandTarget interface.
		///      This is called when the command's availability is updated
		/// </summary>
		/// <param term='commandName'>
		///		The name of the command to determine state for.
		/// </param>
		/// <param term='neededText'>
		///		Text that is needed for the command.
		/// </param>
		/// <param term='status'>
		///		The state of the command in the user interface.
		/// </param>
		/// <param term='commandText'>
		///		Text requested by the neededText parameter.
		/// </param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, EnvDTE.vsCommandStatusTextWanted neededText, ref EnvDTE.vsCommandStatus status, ref object commandText)
		{
			// MessageBox.Show("Prj2make Debug", "In QueryStatus");
			if(neededText == EnvDTE.vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if(commandName == "vsprj2make.Connect.CreateMake")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.GenDistUnit")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.GenMDFiles")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.PrjxToCsproj")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.MonoRuntime")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.RunOnMono")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.Options")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.ExploreCurrSln")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
				
				if(commandName == "vsprj2make.Connect.ExploreCurrDoc")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
			}
		}

		/// <summary>
		///      Implements the Exec method of the IDTCommandTarget interface.
		///      This is called when the command is invoked.
		/// </summary>
		/// <param term='commandName'>
		///		The name of the command to execute.
		/// </param>
		/// <param term='executeOption'>
		///		Describes how the command should be run.
		/// </param>
		/// <param term='varIn'>
		///		Parameters passed from the caller to the command handler.
		/// </param>
		/// <param term='varOut'>
		///		Parameters passed from the command handler to the caller.
		/// </param>
		/// <param term='handled'>
		///		Informs the caller if the command was handled or not.
		/// </param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, EnvDTE.vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == EnvDTE.vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "vsprj2make.Connect.CreateMake")
				{
					// handled = true;
					handled = CreateMakefile();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.GenDistUnit")
				{
					// handled = true;
					handled = GenerateDistUnit();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.GenMDFiles")
				{
					// handled = true;
					handled = GenerateMDcmbx();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.PrjxToCsproj")
				{
					// handled = true;
					handled = ImportMDcmbx();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.MonoRuntime")
				{
					handled = true;
					// handled = MonoRuntime();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.RunOnMono")
				{
					// handled = true;
					handled = TestInMono();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.Options")
				{
					// handled = true;
					handled = OptionsAndSettings();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.ExploreCurrSln")
				{
					// handled = true;
					handled = ExploreCurrSln();
					return;
				}
				
				if(commandName == "vsprj2make.Connect.ExploreCurrDoc")
				{
					// handled = true;
					handled = ExploreCurrDoc();
					return;
				}
			}
		}

		// Actual Invocation
		// public void CreateMakefile(Object commandBarControl, ref bool handled, ref bool cancelDefault)
		public bool CreateMakefile()
		{
			// Create Makefiles
			Prj2MakeHelper p2mhObj = new Prj2MakeHelper();
			string strSLNFile = applicationObject.Solution.FileName;
			
			applicationObject.StatusBar.Clear();
			outputWindowPane.OutputString("--------------------------------------\nCreating Makefile for ");
			outputWindowPane.OutputString(String.Format("Solution: {0}\n", strSLNFile));

			applicationObject.StatusBar.Text = "Creating Makefile.Win32 file...";
			outputWindowPane.OutputString("\tCreating Makefile.Win32 file...\n");

			// csc and nmake
			outputWindowPane.OutputString(p2mhObj.CreateMakeFile(true, true, strSLNFile));
			applicationObject.StatusBar.Text = "Makefile.csc.nmake file created!";
			outputWindowPane.OutputString("\tMakefile.csc.nmake file created!\n");
			// mcs and nmake
			outputWindowPane.OutputString(p2mhObj.CreateMakeFile(false, true, strSLNFile));
			applicationObject.StatusBar.Text = "Makefile.mcs.nmake file created!";
			outputWindowPane.OutputString("\tMakefile.mcs.nmake file created!\n");
			// mcs and gmake
			outputWindowPane.OutputString(p2mhObj.CreateMakeFile(false, false, strSLNFile));
			applicationObject.StatusBar.Text = "Makefile.mcs.gmake file created!";
			outputWindowPane.OutputString("\tMakefile.mcs.gmake file created!\n");

			return true;
		}

		// Generate a distribution unit
		public bool GenerateDistUnit()
		{
			// Create Distribution Unit
			Mfconsulting.Vsprj2make.RegistryHelper regH = new RegistryHelper();
			CreateZipHelper zipObj = new CreateZipHelper();			
			string strSLNFile = applicationObject.Solution.FileName;
			string 	strIgDirs;
			string strIgEx;
			int nLevel;

			// Get Regvalues
			strIgDirs = regH.IgnoredDirectories;
			strIgEx = regH.IgnoredExtensions;
			nLevel = regH.CompressionLevel;

			applicationObject.StatusBar.Clear();
			outputWindowPane.OutputString("--------------------------------------\nGenerating a distribution unit for ");
			outputWindowPane.OutputString(String.Format("Solution: {0}\n", strSLNFile));

			applicationObject.StatusBar.Text = "Creating Zip file...";
			outputWindowPane.OutputString("\tCreating Zip file...\n");

			outputWindowPane.OutputString(zipObj.CreateZipFile(
				strSLNFile,
				strIgDirs,
				strIgEx,
				nLevel
				)
				);

			applicationObject.StatusBar.Text = "Zip file created!";
			outputWindowPane.OutputString("\tZip file created!\n");
			
			return true;
		}

		// Generate a MonoDevelop combine and/or prjx
		// public void GenerateMDcmbx(Object commandBarControl, ref bool handled, ref bool cancelDefault)
		public bool GenerateMDcmbx()
		{
			// Create MonoDevelop files
			Prj2MakeHelper p2mhObj = new Prj2MakeHelper();
			string strSLNFile = applicationObject.Solution.FileName;

			applicationObject.StatusBar.Clear();
			outputWindowPane.OutputString("--------------------------------------\nGenerating MonoDevelop files for ");
			outputWindowPane.OutputString(String.Format("Solution: {0}\n", strSLNFile));

			applicationObject.StatusBar.Text = "Creating MonoDevelop files...";
			outputWindowPane.OutputString("\tCreating MonoDevelop files...\n");

			outputWindowPane.OutputString(p2mhObj.CreateMdFiles(strSLNFile));

			applicationObject.StatusBar.Text = "MonoDevelop files created!";
			outputWindowPane.OutputString("\tMonoDevelop files created!\n");
			
			return true;
		}

		// Import MonoDevelop combine and/or prjx
		public bool ImportMDcmbx()
		{
			MessageBox.Show("Import MonoDevelop Solution is not implemented yet.", "prj2make-sharp debug");
			/*
			string strSLNFile = applicationObject.Solution.FileName;

			applicationObject.StatusBar.Clear();
			outputWindowPane.OutputString("--------------------------------------\nCompile and Run in Mono: ");
			outputWindowPane.OutputString(String.Format("Solution: {0}\n", strSLNFile));

			applicationObject.StatusBar.Text = "Attemp to build in Mono...";
			outputWindowPane.OutputString("\tAttemp to build in Mono...\n");

			// Build for Mono

			applicationObject.StatusBar.Text = "Launch in Mono...";
			outputWindowPane.OutputString("\tLaunch in Mono...\n");

			// Lanunch in Mono
			*/

			return true;
		}

		// Run on Mono
		public bool TestInMono()
		{
			// Registry Helper Obj
			Mfconsulting.Vsprj2make.RegistryHelper regH = new RegistryHelper();

			// MonoLaunchHelper
			Mfconsulting.Vsprj2make.MonoLaunchHelper launchHlpr = new MonoLaunchHelper();

			// Web
			bool isWebProject = false;
			string aciveFileSharePath = @"C:\inetpub\wwwroot";
			string startPage = "index.aspx";
			string portForXsp = "8189";

			// Other
			string startUpProject = "";
			string projectOutputFileName = "";
			string projectOutputPathFromBase = "";
			int projectOutputType = 0;
			string projectOutputWorkingDirectory = "";

			EnvDTE.Solution thisSln = applicationObject.Solution;

			// Run in Mono Process
			System.Diagnostics.ProcessStartInfo procInfo = new ProcessStartInfo();
			System.Diagnostics.Process monoLauncC = new System.Diagnostics.Process();
			monoLauncC.StartInfo = procInfo;

			// Get the Solution's startup project
            startUpProject = thisSln.Properties.Item(5).Value.ToString();

			// Run in Mono
			EnvDTE.Projects projs = thisSln.Projects;
			foreach(EnvDTE.Project proj in projs)
			{
				if(startUpProject.CompareTo(proj.Name) == 0)
				{				
					foreach(EnvDTE.Property prop in proj.Properties)
					{
						if(prop.Name.CompareTo("ProjectType") == 0)
						{
							if(Convert.ToInt32(prop.Value) == 1)
							{
								isWebProject = true;
								portForXsp = regH.Port.ToString();
							}
						}

						// Web Root for XSP
						if(prop.Name.CompareTo("ActiveFileSharePath") == 0)
						{
							aciveFileSharePath = prop.Value.ToString();
						}

						if(prop.Name.CompareTo("OutputType") == 0)
						{
							projectOutputType = Convert.ToInt32(prop.Value);
						}

						if(prop.Name.CompareTo("OutputFileName") == 0)
						{
							projectOutputFileName = prop.Value.ToString();
						}

						if(prop.Name.CompareTo("LocalPath") == 0)
						{
							projectOutputWorkingDirectory = prop.Value.ToString();
						}					
					}

					// Active Configuration Properties
					foreach(EnvDTE.Property prop in proj.ConfigurationManager.ActiveConfiguration.Properties)
					{
						// XSP startup page
						if(prop.Name.CompareTo("StartPage") == 0)
						{
							startPage = prop.Value.ToString();
						}

						// Output path for non web projects
						if(prop.Name.CompareTo("OutputPath") == 0)
						{
							projectOutputPathFromBase = prop.Value.ToString();
						}
					}

					// If is an Executable and not a DLL or web project
					// then launch it with monoLaunchC
					if(isWebProject == false && (projectOutputType == 0 || projectOutputType == 1))
					{
						monoLauncC.StartInfo.FileName = launchHlpr.MonoLaunchCPath;
						monoLauncC.StartInfo.WorkingDirectory = System.IO.Path.Combine(
							projectOutputWorkingDirectory,
							projectOutputPathFromBase);
						monoLauncC.StartInfo.Arguments = projectOutputFileName;

						monoLauncC.Start();
						return true;
					}

					// Web Project execution and launching of XSP
					if(isWebProject == true)
					{
						string startURL = String.Format(
							"http://localhost:{0}/{1}",
							portForXsp,
							startPage);

						System.Diagnostics.ProcessStartInfo procInfo1 = new ProcessStartInfo();
						System.Diagnostics.Process launchStartPage = new System.Diagnostics.Process();
						launchStartPage.StartInfo = procInfo1;
			
						monoLauncC.StartInfo.FileName = launchHlpr.MonoLaunchCPath;
						monoLauncC.StartInfo.WorkingDirectory = aciveFileSharePath;
						monoLauncC.StartInfo.Arguments = String.Format(
							"{0} --root . --port {1} --applications /:.",
							launchHlpr.GetXspExePath(regH.XspExeSelection),
							portForXsp
							);

						// Actually start XSP
						monoLauncC.Start();

						launchStartPage.StartInfo.UseShellExecute = true;
						launchStartPage.StartInfo.Verb = "open";
						launchStartPage.StartInfo.FileName = startURL;
                                    
						// Do a little delay so XSP can launch
						System.Threading.Thread.Sleep(1500);
						launchStartPage.Start();
					}
				}
			}
			
			/*
			string strSLNFile = applicationObject.Solution.FileName;

			applicationObject.StatusBar.Clear();
			outputWindowPane.OutputString("--------------------------------------\nCompile and Run in Mono: ");
			outputWindowPane.OutputString(String.Format("Solution: {0}\n", strSLNFile));

			applicationObject.StatusBar.Text = "Attemp to build in Mono...";
			outputWindowPane.OutputString("\tAttemp to build in Mono...\n");

			// Build for Mono

			applicationObject.StatusBar.Text = "Launch in Mono...";
			outputWindowPane.OutputString("\tLaunch in Mono...\n");

			// Lanunch in Mono
			*/

			return true;
		}

		// vsprj2make settings
		public bool OptionsAndSettings()
		{
			OptionsDlg optDlg = new OptionsDlg();
			
			if(optDlg.ShowDialog() == DialogResult.OK)
			{
			}
			return true;
		}

		public bool ExploreCurrSln()
		{
			string strSlnFile = applicationObject.Solution.FileName;
			System.Diagnostics.ProcessStartInfo procInfo = new ProcessStartInfo();
			System.Diagnostics.Process procLaunchExplore = new System.Diagnostics.Process();
			procLaunchExplore.StartInfo = procInfo;
			
			if(strSlnFile != null)
			{
				procLaunchExplore.StartInfo.UseShellExecute = true;
				procLaunchExplore.StartInfo.Verb = "Open";
				procLaunchExplore.StartInfo.FileName = System.IO.Path.GetDirectoryName(strSlnFile);
				procLaunchExplore.Start();
			}

			return true;
		}

		public bool ExploreCurrDoc()
		{
			string strDocFile = null;
			System.Diagnostics.ProcessStartInfo procInfo = new ProcessStartInfo();
			System.Diagnostics.Process procLaunchExplore = new System.Diagnostics.Process();
			procLaunchExplore.StartInfo = procInfo;
			
			try
			{
				strDocFile = applicationObject.ActiveDocument.Path;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message);
			}

			if(strDocFile != null)
			{
				procLaunchExplore.StartInfo.UseShellExecute = true;
				procLaunchExplore.StartInfo.Verb = "Open";
				procLaunchExplore.StartInfo.FileName = System.IO.Path.GetDirectoryName(strDocFile);
				procLaunchExplore.Start();
			}

			return true;
		}

		#region Utility functions

		/// <summary>
		/// Searches the existing commands for a match based on the input command name
		/// </summary>
		/// <param name="cmdCollectionContainer">Collection of Command objects</param>
		/// <param name="strCommandName">A string representing the command name</param>
		/// <returns>Returns an Command object instance. If it fails returns null</returns>
		protected Command GetExistingNamedCommand(Commands cmdCollectionContainer, string strCommandName)
		{
			Command cmdRetVal = null;

			// Itereate through all of the commands
			foreach(Command cmd in cmdCollectionContainer)
			{
				if(cmd != null && cmd.Name != null)
				{
					if(cmd.Name.CompareTo(strCommandName) == 0)
					{
						cmdRetVal = cmd;
						break;
					}
				}
			}

			return cmdRetVal;
		}

		/// <summary>
		/// Creates a Named command.
		/// </summary>
		/// <param name="addInInstance">Addin Instance</param>
		/// <param name="cmdCollectionContainer">Collection of Command objects</param>
		/// <param name="strCtrlName">The control name</param>
		/// <param name="strCtrlCaption">The control caption</param>
		/// <param name="strCtrlToolTip">Text for tool tip</param>
		/// <param name="contextGUIDS"></param>
		/// <returns>Returns a newly create Command object or null if it fails</returns>
		protected Command CreateNamedCommand(AddIn addInInstance, Commands cmdCollectionContainer, string strCtrlName, string strCtrlCaption, string strCtrlToolTip, ref object[] contextGUIDS)
		{
			Command cmdRetVal = null;
			try
			{
				cmdRetVal = cmdCollectionContainer.AddNamedCommand(
					addInInstance,
					strCtrlName,
					strCtrlCaption,
					strCtrlToolTip,
					true,
					0 ,
					ref contextGUIDS,
					(int)vsCommandStatus.vsCommandStatusSupported+(int)vsCommandStatus.vsCommandStatusEnabled
					);
			}
			catch(System.Exception exc)
			{
				Trace.WriteLine(String.Format("Exception caught in Adding Command1:\n {0}", exc.Message));
				Trace.WriteLine(String.Format("Number of commands:\n {0}", cmdCollectionContainer.Count));
			}
			
			return cmdRetVal;
		}
		
		/// <summary>
		/// Delete all CommandBarControl objects that were tagged using this VSAddIn.
		/// </summary>
		protected void DeleteControls()
		{
			CommandBarControls controls = DTE.CommandBars.FindControls(System.Reflection.Missing.Value, System.Reflection.Missing.Value, addInInstance.ProgID, System.Reflection.Missing.Value);
			foreach(CommandBarControl control in controls)
			{
				control.Delete(false);
			}

			foreach(CommandBar commandBar in _commandBars)
			{
				DTE.Commands.RemoveCommandBar(commandBar);
			}
		}

		#endregion

		#region Event handlers for the MonoBarra toolbar
		
		private void versionComboBox_Change(CommandBarComboBox Ctrl)
		{
			Mfconsulting.Vsprj2make.RegistryHelper regHlpr = new RegistryHelper();
			regHlpr.SetDefaultClr(versionComboBox.Text);

		}

		private void testInMonoCommandBarButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
		{
			TestInMono();
		}
		
		#endregion

	}
}
