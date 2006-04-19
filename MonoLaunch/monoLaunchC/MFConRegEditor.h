// The MIT License
// Copyright (c) 2004 Francisco "Paco" Martinez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

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
