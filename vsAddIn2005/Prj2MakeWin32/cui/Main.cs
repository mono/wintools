// project created on 3/13/04 at 5:22 a
using System;

namespace Mfconsulting.General.Prj2Make.Cui
{
    class MainClass 
	{
    	public static void Main(string[] args)
    	{
			// Handle command line arguments    	
			MainOpts optObj = new MainOpts();    		
			optObj.ProcessArgs(args);

			if ( optObj.csproj2prjx == true && optObj.RemainingArguments.Length > 0)
			{
				new MainMod (optObj.RemainingArguments[0]);
				return;
			}

			// Asuming residual arguments possibly a file path
			if (optObj.RemainingArguments.Length > 0)
			{
				new MainMod (optObj.isNmake, optObj.isCsc, optObj.RemainingArguments[0]);
			}
			else  // No arguments
			{
				optObj.DoHelp();
			}
		}
    }    
}
