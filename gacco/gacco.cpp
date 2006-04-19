// The MIT License
// Copyright (c) 2004 Francisco "Paco" Martinez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


// gacco.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "GacHelper.h"

#define VALUESIZE 4096

// Prints the correct usage for
// using this utility
void WINAPI PrintUsage(void);

// Function to process input parameters
void WINAPI PocessCmdLine(int, _TCHAR**, short*, TCHAR*);

// Install the specified assembly
void WINAPI InstallIntoGac(LPCTSTR);

// Enumerate GAC assemblies
void WINAPI EnumGacForAssembly(LPCTSTR);

// Query GAC for assembly
void WINAPI QueryGacForAssembly(LPCTSTR);

// Uninstall the specified assembly
void WINAPI UnInstallFromGac(LPCTSTR);

int _tmain(int argc, _TCHAR* argv[])
{
	short nOption = 0;
	static TCHAR szValue[VALUESIZE];

	// Initialize COM
	::CoInitialize(NULL);

	if(argc != 3)
	{
		// Print usage
		PrintUsage();
		::CoUninitialize();
		return 1;
	}

	// Process command line
	PocessCmdLine(argc, argv, &nOption, szValue);

	switch(nOption)
	{
	case 1:	// Install
		InstallIntoGac(szValue);
		break;
	case 2: // Enumerate
		EnumGacForAssembly(szValue);
		break;
	case 3: // Query
		QueryGacForAssembly(szValue);
		break;
	case 4: // Uninstall
		UnInstallFromGac(szValue);
		break;
	default:
		PrintUsage();
		::CoUninitialize();
		return 1;
	}

	::CoUninitialize();
	return 0;
}

// Install the specified assembly
void WINAPI InstallIntoGac(LPCTSTR szAssemblyFileName)
{
	HRESULT hrRetVal = S_OK;
	CGacHelper* pGacHlpr = NULL;

	pGacHlpr = new CGacHelper;

	hrRetVal = pGacHlpr->InstallAssemblyWithTrace(szAssemblyFileName,
		_T("gacco"),
		_T("MS GAC installer helper utility")
		);

    delete pGacHlpr;

	if(!SUCCEEDED(hrRetVal))
	{
		wprintf(_T("Failed to add %s to the Microsft Global Assembly Cache.\n"), szAssemblyFileName); 
	}
	else
	{
		wprintf(_T("%s was added successfully to the Microsft Global Assembly Cache.\n"), szAssemblyFileName); 
	}
}

// Enumerate GAC assemblies
void WINAPI EnumGacForAssembly(LPCTSTR szAssemblyName)
{
	HRESULT hrRetVal = S_FALSE;
	CGacHelper* pGacHlpr = NULL;

	pGacHlpr = new CGacHelper;

	hrRetVal = pGacHlpr->EnumerateAssemblyInGac(szAssemblyName);

    delete pGacHlpr;

	if(hrRetVal != S_OK)
	{
		wprintf(_T("%s was not found in the Microsft Global Assembly Cache.\n"), szAssemblyName); 
	}
}

// Query GAC for assembly
void WINAPI QueryGacForAssembly(LPCTSTR szFullySpecifiedAssamblyName)
{
	HRESULT hrRetVal = S_OK;
	CGacHelper* pGacHlpr = NULL;
	static TCHAR szFileName[MAX_PATH];

	pGacHlpr = new CGacHelper;

	hrRetVal = pGacHlpr->QueryAssemblyInGac(szFullySpecifiedAssamblyName, szFileName);

    delete pGacHlpr;

	if(!SUCCEEDED(hrRetVal))
	{
		wprintf(_T("Failed to find %s in the Microsft Global Assembly Cache.\n"), szFullySpecifiedAssamblyName); 
	}
	else
	{
		wprintf(_T("The file for assembly %s can be found in:\n%s\n"), szFullySpecifiedAssamblyName, szFileName); 
	}
}

// Uninstall the specified assembly
void WINAPI UnInstallFromGac(LPCTSTR szFullySpecifiedAssamblyName)
{
	HRESULT hrRetVal = S_OK;
	CGacHelper* pGacHlpr = NULL;

	pGacHlpr = new CGacHelper;

	hrRetVal = pGacHlpr->UnInstallAssemblyWithTrace(szFullySpecifiedAssamblyName,
		_T("gacco"),
		_T("MS GAC installer helper utility")
		);

    delete pGacHlpr;
	if(!SUCCEEDED(hrRetVal))
	{
		wprintf(_T("Failed to uninstall %s from the Microsft Global Assembly Cache.\n"), szFullySpecifiedAssamblyName); 
	}
	else
	{
		wprintf(_T("%s has been removed from the Microsft Global Assembly Cache.\n"), szFullySpecifiedAssamblyName); 
	}
}

void WINAPI PocessCmdLine(int argc, _TCHAR* argv[], short* nOpion, TCHAR* szValue)
{
	CString strFirstParam;
	CString strSecondParam;
	
	// easy diqualification. There should be 3 parameters:
	// running executable name, options (-i,-u,-q) and
	// Value being passed (assembly name, file name or 
	// fully qualified assembly name)
	if(argc != 3)
	{
		*nOpion = 0;
		return;
	}

	strSecondParam = argv[2];

	if(strSecondParam.GetLength() < 1)
	{
		*nOpion = 0;
		return;
	}
	else
	{
		::lstrcpyn(szValue, (LPCTSTR)strSecondParam, VALUESIZE - 1);
	}

	strFirstParam = argv[1];

	// Disqulify if more than one character in the first parameter
	// and the first character is neither dash or slash
	if(strFirstParam.GetLength() != 2 && (strFirstParam[0] != _T('-') || strFirstParam[0] != _T('/')))
	{
		*nOpion = 0;
		return;
	}
	else
	{
		TCHAR ch = _T('\0');

		strFirstParam.MakeUpper();
		ch = strFirstParam[1];

		switch(ch)
		{
		case _T('I'):
			*nOpion = 1;
			return;
		case _T('E'):
			*nOpion = 2;
			return;
		case _T('Q'):
			*nOpion = 3;
			return;
		case _T('U'):
			*nOpion = 4;
			return;
		default:
			*nOpion = 0;
			return;
		}
	}	

	return;
}

void WINAPI PrintUsage(void)
{
	wprintf(
		_T("gacco.exe is a MS .NET Framework GAC helper utility\r\n") \
		_T("by Paco Maritnez (c) 2004\r\n") \
		_T("\r\n") \
		_T("Invalid number of arguments. The correct usage is:\r\n") \
		_T("\r\n") \
		_T("gacco <option> <file name>\r\n") \
		_T("\r\n") \
		_T("or\r\n") \
		_T("\r\n") \
		_T("gacco <option> <fully qualified assembly name>\r\n") \
		_T("\r\n") \
		_T("or\r\n") \
		_T("\r\n") \
		_T("gacco <option> <assembly name>\r\n") \
		_T("\r\n") \
		_T("where options are:\r\n") \
		_T("\r\n") \
		_T("  -i <file name> to installing in the GAC\r\n") \
		_T("  -e <assembly name> to enumarate all matching assembly names\r\n") \
		_T("  -q <fully qualified assembly name> to query for existing assembly\r\n") \
		_T("  -u <fully qualified assembly name> to uninstall from the GAC\r\n") \
		_T("\r\n") \
		_T("Usage examples:\r\n") \
		_T("\r\n") \
		_T("For installing:\r\n") \
		_T("\r\n") \
		_T("\tgacco -i D:\\mono\\Mono-1.0\\lib\\mono\\1.0\\Mono.GetOptions.dll\r\n") \
		_T("\r\n") \
		_T("To enumerate:\r\n") \
		_T("\r\n") \
		_T("\tgacco -q \"Mono.GetOptions\"\r\n") \
		_T("\r\n") \
		_T("To query:\r\n") \
		_T("\r\n") \
		_T("\tgacco -e \"Mono.GetOptions, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756\"\r\n") \
		_T("\r\n") \
		_T("To uninstall:\r\n") \
		_T("\r\n") \
		_T("\tgacco -u \"Mono.GetOptions, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756\"\r\n") \
		_T("\r\n")
		);
}

