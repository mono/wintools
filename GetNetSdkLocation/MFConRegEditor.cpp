// MFConRegEditor.cpp: implementation of the CMFConRegEditor class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "MFConRegEditor.h"

#include <aclapi.h>

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CMFConRegEditor::CMFConRegEditor()
{
	m_RegKey = NULL;
}

CMFConRegEditor::CMFConRegEditor(LPCTSTR strKeyName)
{

	DWORD dwDisposition;
	long lRc;

	lRc = RegCreateKeyEx(
		HKEY_LOCAL_MACHINE,					// handle to an open key
		strKeyName,							// address of subkey name
		0,                                  // reserved
		_T("Key"),						        // address of class string
		REG_OPTION_NON_VOLATILE,            // special options flag
		KEY_ALL_ACCESS,					    // desired security access
		NULL,								// address of key security structure					  
	    &m_RegKey,							// address of buffer for opened handle
		&dwDisposition						// address of disposition value buffer
		);
}

CMFConRegEditor::~CMFConRegEditor()
{
	if(m_RegKey != NULL)
		RegCloseKey(m_RegKey);
}

BOOL CMFConRegEditor::Open(LPCTSTR strKeyName, int iAccMode)
{
	DWORD dwDisposition, dwAccMode;
	long lRc;

	switch(iAccMode)
	{
	case 0:
		dwAccMode = KEY_READ;
		break;
	case 1:
		dwAccMode = KEY_ALL_ACCESS;
		break;
	}

	lRc = RegCreateKeyEx(
		HKEY_LOCAL_MACHINE,					// handle to an open key
		strKeyName,							// address of subkey name
		0,                                  // reserved
		_T("Key"),					        // address of class string
		REG_OPTION_NON_VOLATILE,            // special options flag
		dwAccMode,						    // desired security access
		NULL,								// address of key security structure					  
	    &m_RegKey,							// address of buffer for opened handle
		&dwDisposition						// address of disposition value buffer
		);

	if(lRc != ERROR_SUCCESS)
		return FALSE;

	return TRUE;
}

DWORD CMFConRegEditor::GetValueDw(LPCTSTR strKeyName)
{
	DWORD dwRetVal = 0;

	long lRc;
	DWORD cbData = sizeof(DWORD);

	lRc = RegQueryValueEx(
		m_RegKey,						// handle to key to query
		(LPCTSTR) strKeyName,           // address of name of value to query
		NULL,							// reserved
		NULL,							// address of buffer for value type
		(LPBYTE)&dwRetVal,				// address of data buffer
		&cbData							// address of data buffer size
		);

	if(lRc != ERROR_SUCCESS)
		return 0;

	return dwRetVal;
}

LPCTSTR CMFConRegEditor::GetValue(LPCTSTR strKeyName)
{
	long lRc;
	DWORD cbData = 400;
	static char szData[400];

	lRc = RegQueryValueEx(
		m_RegKey,						// handle to key to query
		(LPCTSTR) strKeyName,           // address of name of value to query
		NULL,							// reserved
		NULL,							// address of buffer for value type
		(LPBYTE)szData,					// address of data buffer
		&cbData							// address of data buffer size
		);

	if(lRc != ERROR_SUCCESS)
		return FALSE;

	return (LPCTSTR)szData;
}

BOOL CMFConRegEditor::SetValue(LPCTSTR strKeyName, DWORD* lpdwKeyValue)
{

	long lRc;

	lRc = RegSetValueEx(
		m_RegKey,							// handle to key to set value for
		strKeyName,                         // name of the value to set
		0,                                  // reserved  
		REG_DWORD,                          // flag for value type
		(LPBYTE)lpdwKeyValue,						// address of value data
		sizeof(long)						// size of value data
		);

	if(lRc != ERROR_SUCCESS)
		return FALSE;

	return TRUE;
}

BOOL CMFConRegEditor::SetValue(LPCTSTR strKeyName, LPCTSTR szKeyValue)
{
	long lRc;
	int nLen;

	nLen = lstrlen(szKeyValue);

	lRc = RegSetValueEx(
		m_RegKey,							// handle to key to set value for
		strKeyName,                         // name of the value to set
		0,                                  // reserved  
		REG_SZ,                             // flag for value type
		(const UCHAR *)szKeyValue,			// address of value data
		nLen								// size of value data
		);

	if(lRc != ERROR_SUCCESS)
		return FALSE;

	return TRUE;
}

BOOL CMFConRegEditor::Close()
{
	if(m_RegKey != NULL)
	{
		RegCloseKey(m_RegKey);
		m_RegKey = NULL;
	}
	return TRUE;
}

BOOL CMFConRegEditor::SetSecurity(LPTSTR strUsr)
{
	long lRc;
	static SECURITY_INFORMATION struSecInfo;
	PSECURITY_DESCRIPTOR pSecDesc;
	PACL pOldDACL = NULL, pNewDACL = NULL;
	EXPLICIT_ACCESS ea;

	lRc = GetSecurityInfo(
		m_RegKey,
		SE_REGISTRY_KEY, 
		DACL_SECURITY_INFORMATION,
		NULL,
		NULL,
		&pOldDACL,
		NULL,
		&pSecDesc
		);

	if(lRc != ERROR_SUCCESS)
		return FALSE;

	ZeroMemory(&ea, sizeof(EXPLICIT_ACCESS));

	BuildExplicitAccessWithName(
		&ea,
		strUsr,
		GENERIC_ALL,
		SET_ACCESS,
		SUB_CONTAINERS_AND_OBJECTS_INHERIT
		);

	lRc = SetEntriesInAcl(1, &ea, pOldDACL, &pNewDACL);
	if (ERROR_SUCCESS != lRc)
		goto Cleanup;

	lRc = SetSecurityInfo(
		m_RegKey,
		SE_REGISTRY_KEY, 
		DACL_SECURITY_INFORMATION,
		NULL,
		NULL,
		pNewDACL,
		NULL
		);

Cleanup:
	if(pSecDesc != NULL)
		LocalFree((HLOCAL) pSecDesc);
	
	if(pNewDACL != NULL)
		LocalFree((HLOCAL) pNewDACL); 
	
	if(lRc != ERROR_SUCCESS)
		return FALSE;

	return TRUE;
}
