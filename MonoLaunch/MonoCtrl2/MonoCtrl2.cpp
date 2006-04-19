// MonoCtrl2.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "MonoCtrl2.h"
#include "MainPropSheet.h"
#include ".\monoctrl2.h"

#include "RegHelper.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CMonoCtrl2App

BEGIN_MESSAGE_MAP(CMonoCtrl2App, CWinApp)
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()


// CMonoCtrl2App construction

CMonoCtrl2App::CMonoCtrl2App()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CMonoCtrl2App object

CMonoCtrl2App theApp;


// CMonoCtrl2App initialization

BOOL CMonoCtrl2App::InitInstance()
{
	CString strDefaultCLR;

	// InitCommonControls() is required on Windows XP if an application
	// manifest specifies use of ComCtl32.dll version 6 or later to enable
	// visual styles.  Otherwise, any window creation will fail.
	InitCommonControls();

	CWinApp::InitInstance();

	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	// of your final executable, you should remove from the following
	// the specific initialization routines you do not need
	// Change the registry key under which our settings are stored
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization
	SetRegistryKey(_T("Novell"));

	// CMainPropSheet *pDlg = NULL;

	CMainPropSheet dlg(_T("Mono Settings"));

	m_pMainWnd = &dlg;

	CRegHelper::GetDefaultCLR(strDefaultCLR.GetBuffer(255), 255);
	strDefaultCLR.ReleaseBuffer();

	dlg.m_Page1.m_strDefaultCLR = strDefaultCLR;

	INT_PTR nResponse = dlg.DoModal();
	
	if (nResponse == IDOK)
	{
		CRegHelper::SetDefaultCLR(dlg.m_Page1.m_strDefaultCLR);
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}

int CMonoCtrl2App::ExitInstance()
{
	// TODO: Add your specialized code here and/or call the base class

	return CWinApp::ExitInstance();
}
