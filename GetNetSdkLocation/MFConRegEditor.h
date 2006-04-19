// MFConRegEditor.h: interface for the CMFConRegEditor class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_REGEDITOR_H__1CB02312_C3B5_11D2_B35D_00A0C936107F__INCLUDED_)
#define AFX_REGEDITOR_H__1CB02312_C3B5_11D2_B35D_00A0C936107F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

class CMFConRegEditor  
{
public:
	BOOL Close();
	BOOL Open(LPCTSTR strKeyName, int iAccMode = 1);
	BOOL SetSecurity(LPTSTR strUsr);
	LPCTSTR GetValue(LPCTSTR strKeyName);
	DWORD GetValueDw(LPCTSTR strKeyName);
	BOOL SetValue(LPCTSTR strKeyName, LPCTSTR szKeyValue);
	BOOL SetValue(LPCTSTR strKeyName, DWORD* lpdwKeyValue);
	HKEY m_RegKey;

	CMFConRegEditor(); // Default Constructor
	CMFConRegEditor(LPCTSTR strKeyName); 
	virtual ~CMFConRegEditor();

};

#endif // !defined(AFX_REGEDITOR_H__1CB02312_C3B5_11D2_B35D_00A0C936107F__INCLUDED_)
