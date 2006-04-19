using System;
using System.Collections;
using Mono.GetOptions;

namespace Mfconsulting.General.Prj2Make.Cui
{
	class MainOpts : Options 
	{
		[Option("Output for nmake.exe", 'n')]
		public bool isNmake = false;

		[Option("Use csc instead of mcs", 'c')]
		public bool isCsc = false;
		
		[Option(1, "Converts a csproj/sln to prjx/cmbx", "csproj2prjx")]
		public bool csproj2prjx = false;
		
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
