// GetNetSdkLocation.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "GetNetSdkLocation.h"
#include "MFConRegEditor.h"

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	int nRetCode = 0;
	CString strSDKPath;

	// Quick command line parameter checking
	if(argc < 2)
	{
		PrintUsage();
		return 0;
	}

	if(ParseCmdLine(argc, argv) != 0)
	{
		cerr << _T("Error parsing command line arguments.") << endl;
		PrintUsage();
		return 1;
	}

	if(g_bShowUsage == TRUE)
	{
		PrintUsage();
		return 0;
	}
	
	if(::lstrlen(g_szLocation) < 1)
	{
		cerr << _T("Location was not specified.") << endl;
		PrintUsage();
		return 1;
	}

	// Invoke the function to process the location 
	if(::lstrcmp(g_szLocation, _T("MONO_1.0.2")) == 0)
	{
		GetMonoSdkRoot(strSDKPath.GetBuffer(MAX_PATH));
		strSDKPath.ReleaseBuffer();
	}
	else if(::lstrcmp(g_szLocation, _T("MSNET_1.1")) == 0)
	{
		GetMsNetRoot(strSDKPath.GetBuffer(MAX_PATH));
		strSDKPath.ReleaseBuffer();
	}
	else if(::lstrcmp(g_szLocation, _T("MSNETSDK_1.1")) == 0)
	{
		GetMsNetSdkRoot(strSDKPath.GetBuffer(MAX_PATH));
		strSDKPath.ReleaseBuffer();
	}
	else
	{
		cerr << _T("Location is not recognized.") << endl;
		PrintUsage();
		return 1;
	}

	// React to the request to use Short Path
	if(g_bUseShortPath == TRUE)
	{
		::GetShortPathName(
			(LPCTSTR)strSDKPath,
			strSDKPath.GetBuffer(MAX_PATH),
			(DWORD)MAX_PATH
			);
		strSDKPath.ReleaseBuffer();
	}

	// React to the request to use Cygwin style path
	if(g_bUseCygwinPath == TRUE)
	{
		CString strCygPath;
		CString strDriveLetter;
		TCHAR chDriveLetter = _T('\0');

		strDriveLetter = strSDKPath.Mid(0,1);
		strDriveLetter.MakeLower();
		chDriveLetter = strDriveLetter.GetAt(0);

		strSDKPath.Replace(_T('\\'), _T('/'));
		strCygPath.Format(
			_T("/cygdrive/%c%s"),
			chDriveLetter,
			(LPCTSTR)(strSDKPath.Mid(2))
			);
		cout << (LPCTSTR)strCygPath << endl;
		return 0;
	}
	cout << (LPCTSTR)strSDKPath << endl;

	return nRetCode;
}

LRESULT APIENTRY GetMonoSdkRoot(LPTSTR szOutPath)
{
	CMFConRegEditor regEdt;
	CString strTemp;

	if(regEdt.Open(_T("SOFTWARE\\Novell\\Mono\\1.0.2"), 0) == false)
	{
		return 1;
	}

	strTemp = regEdt.GetValue(_T("SdkInstallRoot"));

	regEdt.Close();

	if(strTemp.GetLength() < 1)
	{
		return -1;
	}
	else
	{
		lstrcpy(szOutPath, (LPCTSTR)strTemp);
	}
	
	return 0;
}

LRESULT APIENTRY GetMsNetSdkRoot(LPTSTR szOutPath)
{
	CMFConRegEditor regEdt;
	CString strTemp;

	if(regEdt.Open(_T("SOFTWARE\\Microsoft\\.NETFramework"), 0) == false)
	{
		return 1;
	}

	strTemp = regEdt.GetValue(_T("sdkInstallRootv1.1"));

	regEdt.Close();

	if(strTemp.GetLength() < 1)
	{
		return -1;
	}
	else
	{
		lstrcpy(szOutPath, (LPCTSTR)strTemp);
	}
	
	return 0;
}

LRESULT APIENTRY GetMsNetRoot(LPTSTR szOutPath)
{
	CMFConRegEditor regEdt;
	CString strTemp;

	if(regEdt.Open(_T("SOFTWARE\\Microsoft\\.NETFramework"), 0) == false)
	{
		return 1;
	}

	strTemp = regEdt.GetValue(_T("InstallRoot"));

	regEdt.Close();

	if(strTemp.GetLength() < 1)
	{
		return -1;
	}
	else
	{
		lstrcpy(szOutPath, (LPCTSTR)strTemp);
	}
	
	return 0;
}

LRESULT APIENTRY ParseCmdLine(int argc, TCHAR* argv[])
{
	CString strArgv;
	CString strOption;
	g_bUseShortPath = FALSE;
	g_bUseCygwinPath = FALSE;
	g_bShowUsage = FALSE;
	ZeroMemory(g_szExe, (sizeof(TCHAR) * MAX_PATH));
	ZeroMemory(g_szLocation, (sizeof(TCHAR) * MAXLOCATIONLEN));

	lstrcpy(g_szExe, argv[0]);

	// Go through all arguments
	for(int i = 1; i < argc; i++)
	{
		::lstrcpyn(strArgv.GetBuffer(1024), argv[i], 1023);
		strArgv.ReleaseBuffer();

		if(strArgv.GetLength() < 1)
		{
			continue;
		}

		strArgv.MakeUpper();
		// Handle known locations
		if(strArgv.Compare(_T("MONO_1.0.2")) == 0)
		{
			lstrcpy(g_szLocation, (LPCTSTR)strArgv);
			continue;
		}

		if(strArgv.Compare(_T("MSNET_1.1")) == 0)
		{
			lstrcpy(g_szLocation, (LPCTSTR)strArgv);
			continue;
		}

		if(strArgv.Compare(_T("MSNETSDK_1.1")) == 0)
		{
			lstrcpy(g_szLocation, (LPCTSTR)strArgv);
			continue;
		}


		// Handle options designated with option marker character
		if(strArgv[0] == _T('-') || strArgv[0] == _T('/'))
		{
			if(strArgv.GetLength() < 2)
			{
				// nothing followed the marker char
				continue;
			}

			strOption = strArgv.Mid(1);
		}
		else
		{
			strOption = strArgv;
		}
		
		for(int i2 = 0; i2 < strOption.GetLength(); i2++)
		{
			switch(strOption[i2])
			{
			case _T('C'):
				g_bUseCygwinPath = TRUE;
				break;
			case _T('S'):
				g_bUseShortPath = TRUE;
				break;
			case _T('H'):
			case _T('?'):
				g_bShowUsage = TRUE;
				return 0;
			default:
				continue;
			}
		}
	}

	return 0;
}

void APIENTRY PrintUsage()
{
	cout << _T("GetNetSdkLocations [options] <location>")<< endl;
	cout << endl;
	cout << _T("Where options are:")<< endl;
	cout << _T("  -s\tUse short path form (8.3 friendly file/path format)")<< endl;
	cout << _T("  -c\tUse Cygwin drive prefix")<< endl;
	cout << _T("  -?\tThis help text")<< endl;
	cout << endl;
	cout << _T("Available locations:")<< endl;
	cout << _T("  mono_1.0.2\tMono 1.0.2 Base Path")<< endl;
	cout << _T("  msnet_1.1\tMicrosoft .NET Framework 1.1 Runtime")<< endl;
	cout << _T("  msnetsdk_1.1\tMicrosoft .NET Framework 1.1 SDK")<< endl;
	cout << endl;
}

