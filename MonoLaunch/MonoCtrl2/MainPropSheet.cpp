// MainPropSheet.cpp : implementation file
//

#include "stdafx.h"
#include "MonoCtrl2.h"
#include "MainPropSheet.h"
#include ".\mainpropsheet.h"


// CMainPropSheet

IMPLEMENT_DYNAMIC(CMainPropSheet, CPropertySheet)

CMainPropSheet::CMainPropSheet(UINT nIDCaption, CWnd* pParentWnd, UINT iSelectPage)
	:CPropertySheet(nIDCaption, pParentWnd, iSelectPage)
{
}

CMainPropSheet::CMainPropSheet(LPCTSTR pszCaption, CWnd* pParentWnd, UINT iSelectPage)
	:CPropertySheet(pszCaption, pParentWnd, iSelectPage)
{
	this->AddPage(&m_Page1);
	// this->AddPage(&m_Page2);
}

CMainPropSheet::~CMainPropSheet()
{
}


BEGIN_MESSAGE_MAP(CMainPropSheet, CPropertySheet)
END_MESSAGE_MAP()


// CMainPropSheet message handlers

BOOL CMainPropSheet::OnInitDialog()
{
	BOOL bResult = CPropertySheet::OnInitDialog();
	// TODO: Add extra initialization here

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}
	
	return bResult;
}
