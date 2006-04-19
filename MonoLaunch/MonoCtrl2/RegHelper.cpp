#include "StdAfx.h"
#include ".\reghelper.h"

CRegHelper::CRegHelper(void)
{
}

CRegHelper::~CRegHelper(void)
{
}

// Retrieves from the registry the default CLR
LRESULT CRegHelper::GetDefaultCLR(LPTSTR szBuffer, DWORD dwBuffLen)
{
	LRESULT lRc = 0;
	HKEY hkMonoKey = NULL;
	DWORD dwType = REG_SZ;
	DWORD dwcbValLen = REGBUFFLEN;
	BYTE cbVal[REGBUFFLEN];
	DWORD dwIdx = 0;

	ZeroMemory(cbVal, sizeof(cbVal));

	lRc = ::RegOpenKeyEx(
		HKEY_LOCAL_MACHINE,
		_T("SOFTWARE\\Novell\\Mono"),
		0,
		KEY_READ,
		&hkMonoKey
		);

	// Check for Errors opening the key
	if(lRc != ERROR_SUCCESS)
	{
		AfxMessageBox(_T("Could not open the Novell\\Mono registry key."));
		return -1;
	}

	lRc = ::RegQueryValueEx(
		hkMonoKey,
		_T("DefaultCLR"),
		0,
		&dwType,
		cbVal,
		&dwcbValLen
		);
	
	if(lRc == ERROR_SUCCESS)
	{
		if(dwcbValLen > dwBuffLen)
		{
			szBuffer = NULL;
			return dwcbValLen;
		}
		else
		{
			lstrcpy(szBuffer, (LPCTSTR)cbVal);
		}
	}

	::RegCloseKey(hkMonoKey);

	return 0;
}

// Retrieves from the registry the default CLR
LRESULT CRegHelper::SetDefaultCLR(LPCTSTR szBuffer)
{
	LRESULT lRc = 0;
	HKEY hkMonoKey = NULL;
	DWORD dwType = REG_SZ;
	DWORD dwcbValLen = 0;
	DWORD dwIdx = 0;

	lRc = ::RegOpenKeyEx(
		HKEY_LOCAL_MACHINE,
		_T("SOFTWARE\\Novell\\Mono"),
		0,
		KEY_WRITE,
		&hkMonoKey
		);

	// Check for Errors opening the key
	if(lRc != ERROR_SUCCESS)
	{
		AfxMessageBox(_T("Could not open the Novell\\Mono registry key."));
		return -1;
	}

	dwcbValLen = ::lstrlen((LPCTSTR)szBuffer);

	lRc = ::RegSetValueEx(
		hkMonoKey,
		_T("DefaultCLR"),
		0,
		dwType,
		(const BYTE*)szBuffer,
		dwcbValLen
		);
	
	::RegCloseKey(hkMonoKey);

	return 0;
}
