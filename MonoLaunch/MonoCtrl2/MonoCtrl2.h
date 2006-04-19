// MonoCtrl2.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols
#include "afxwin.h"


// CMonoCtrl2App:
// See MonoCtrl2.cpp for the implementation of this class
//

class CMonoCtrl2App : public CWinApp
{
public:
	CMonoCtrl2App();

// Overrides
	public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
	// The DesktopWindow wrapper
	CWnd m_DeskTopWin;
	virtual int ExitInstance();
};

extern CMonoCtrl2App theApp;