using System;
using System.Collections;
using Mono.GetOptions;

namespace Mfconsulting.Vsprj2make
{
	class MainOpts : Options 
	{
		[Option(1, "Compression level", 'l')]
		public int Level = 4;

		[Option(1, "Ignored extensions", "IgnoredExtensions")]
		public string Extensions = ".suo,.cvsignore,.vssscc,.vspscc";
		
		[Option(1, "Ignored directories", "IgnoredDirectories")]
		public string BlacListedDirectories = "CVS,obj,.svn";
				
		[Option(1, "Output file path", 'o')]
		public string OutputFilePath = "";
				
		[Option(1, "Input solution or project file", 'f')]
		public string SolutionFile = "";
				
		public MainOpts()
		{
			ParsingMode = OptionsParsingMode.Both;
			BreakSingleDashManyLettersIntoManyOptions = false;
			EndOptionProcessingWithDoubleDash = true;

			if (System.IO.Path.DirectorySeparatorChar.CompareTo('/') == 0)
				ParsingMode = OptionsParsingMode.Linux;
		}
	}    
}
