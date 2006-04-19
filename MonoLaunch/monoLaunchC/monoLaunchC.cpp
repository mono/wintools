// The MIT License
// Copyright (c) 2004 Francisco "Paco" Martinez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// monoLaunchC.cpp : Defines the entry point for the application.
//

#include "stdafx.h"
#include "MFConRegEditor.h"

#define PATHBLOCKSIZE 32767
#define ENVIVALUEBLOCKSIZE 1024

void WINAPI PrintUsage(HWND);

void WINAPI GetMonoRuntimeBasePath(HINSTANCE, LPTSTR);
void WINAPI GetMonoRuntimeLocation(HINSTANCE, LPCTSTR, LPTSTR);

int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
	TCHAR szSdkBasePath[MAX_PATH];
	TCHAR szMonoRuntimeExec[MAX_PATH];

	if(::lstrlen(lpCmdLine) < 1)
	{
		PrintUsage(::GetDesktopWindow());
		return 0;
	}

	ZeroMemory(szSdkBasePath, MAX_PATH);
	ZeroMemory(szMonoRuntimeExec, MAX_PATH);

	try
	{
        GetMonoRuntimeBasePath(hInstance, szSdkBasePath);
	}
	catch(TCHAR *szErr)
	{
		::MessageBox(
			::GetDesktopWindow(),
			szErr,
			_T("monoLaunchC"),
			MB_OK | MB_ICONSTOP
			);

		return -1;
	}

	try
	{
        GetMonoRuntimeLocation(hInstance, szSdkBasePath, szMonoRuntimeExec);
	}
	catch(TCHAR *szErr)
	{
		::MessageBox(
			::GetDesktopWindow(),
			szErr,
			_T("monoLaunchC"),
			MB_OK | MB_ICONSTOP
			);

		return -1;
	}

	SECURITY_ATTRIBUTES SecAttib;
	SECURITY_ATTRIBUTES ThreadAttrib;
	STARTUPINFO StartUpInfo;
	PROCESS_INFORMATION ProcInfo;
	// Environmnet strings
	TCHAR szTempBuff[PATHBLOCKSIZE];
	TCHAR szNewPath[PATHBLOCKSIZE];
	TCHAR szGtkBasePath[ENVIVALUEBLOCKSIZE];
	TCHAR szMonoPath[ENVIVALUEBLOCKSIZE];
	TCHAR szMonoCfgDir[ENVIVALUEBLOCKSIZE];
	ZeroMemory(szTempBuff, PATHBLOCKSIZE);
	ZeroMemory(szNewPath, PATHBLOCKSIZE);
	ZeroMemory(szGtkBasePath, ENVIVALUEBLOCKSIZE);
	ZeroMemory(szMonoPath, ENVIVALUEBLOCKSIZE);
	ZeroMemory(szMonoCfgDir, ENVIVALUEBLOCKSIZE);

	// Structure zeroing
	ZeroMemory(&SecAttib, sizeof(SECURITY_ATTRIBUTES));
	ZeroMemory(&ThreadAttrib, sizeof(SECURITY_ATTRIBUTES));
	ZeroMemory(&StartUpInfo, sizeof(STARTUPINFO));
	ZeroMemory(&ProcInfo, sizeof(PROCESS_INFORMATION));

	StartUpInfo.cb = sizeof(STARTUPINFO);

	// Set the environment Prior to launching
	// -- Path
	::GetEnvironmentVariable(_T("PATH"), szTempBuff, PATHBLOCKSIZE);
	::lstrcpy(szNewPath, szSdkBasePath);
	::PathAddBackslash(szNewPath);
	::wsprintf(szNewPath, _T("%sbin;%s"),
		szNewPath,
		szTempBuff
		);
	::SetEnvironmentVariable(_T("PATH"), szNewPath);
	ZeroMemory(szTempBuff, PATHBLOCKSIZE);

	// -- GTK_BASEPATH
	::lstrcpy(szGtkBasePath, szSdkBasePath);
	::SetEnvironmentVariable(_T("GTK_BASEPATH"), szGtkBasePath);

	// -- MONO_PATH
	::lstrcpy(szMonoPath, szSdkBasePath);
	::PathAddBackslash(szMonoPath);
	::lstrcat(szMonoPath, _T("lib"));
	::SetEnvironmentVariable(_T("MONO_PATH"), szMonoPath);

	// -- MONO_CVG_DIR
	::lstrcpy(szMonoCfgDir, szSdkBasePath);
	::PathAddBackslash(szMonoCfgDir);
	::lstrcat(szMonoCfgDir, _T("etc"));
	::SetEnvironmentVariable(_T("MONO_CVG_DIR"), szMonoCfgDir);

	::wsprintf(szTempBuff, _T("%s %s"),
		szMonoRuntimeExec,
		lpCmdLine
		);

	::CreateProcess(
		NULL,
		szTempBuff,
		&SecAttib,
		&ThreadAttrib,
		TRUE,
		0,
		NULL,
		NULL,
		&StartUpInfo,
		&ProcInfo
		);

	return 0;
}

void WINAPI GetMonoRuntimeBasePath(HINSTANCE hInstance, LPTSTR szSdkBasePath)
{
	static TCHAR szDefaultClr[25];
	static TCHAR szMonoKey[255];
	CMFConRegEditor regEdt;

	::LoadString(hInstance, IDS_MONOREGKEY, szMonoKey, sizeof(szMonoKey));

	regEdt.Open(szMonoKey, 0);

	::lstrcpy(szDefaultClr, regEdt.GetValue(_T("DefaultCLR")));

	regEdt.Close();

	::wsprintf(
		szMonoKey,
		_T("%s\\%s"),
		szMonoKey,
		szDefaultClr
		);

	regEdt.Open(szMonoKey, 0);

	::lstrcpy(szSdkBasePath, regEdt.GetValue(_T("SdkInstallRoot")));

	regEdt.Close();

	if(::PathFileExists(szSdkBasePath) != TRUE)
		throw _T("Mono runtime not found.");
}

void WINAPI GetMonoRuntimeLocation(HINSTANCE hInstance, LPCTSTR szSdkRoot, LPTSTR szRuntime)
{
	TCHAR szLocalSdkRoot[MAX_PATH];
	ZeroMemory(szLocalSdkRoot, MAX_PATH);

	::lstrcpy(szLocalSdkRoot, szSdkRoot);

	// Add a back slash at the end
	::PathAddBackslash(szLocalSdkRoot);

	::wsprintf(
		szRuntime,
		_T("%sbin\\mono.exe"),
		szLocalSdkRoot
		);

	if(::PathFileExists(szRuntime) != TRUE)
		throw _T("Mono runtime not found.");
}

void WINAPI PrintUsage(HWND hWndParent)
{
	::MessageBox(hWndParent,
		_T("monoLaunchC.exe is a Mono/Windows Shell helper utility\r\n") \
		_T("by Paco Maritnez (c) 2004\r\n") \
		_T("\r\n") \
		_T("monoLaunchC <command line>\r\n") \
		_T("\r\n") \
		_T("Usage examples:\r\n") \
		_T("\r\n") \
		_T("monoLaunchC C:\\mono\\Mono-1.0\\lib\\sqlsharpgtk.exe\r\n") \
		_T("\r\n"),
		_T("monoLaunchC"),
		MB_OK | MB_ICONINFORMATION
		);
}

