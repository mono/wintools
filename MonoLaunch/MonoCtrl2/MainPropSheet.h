#pragma once

#include "PropPage1.h"
#include "PropPage2.h"

// CMainPropSheet

class CMainPropSheet : public CPropertySheet
{
	DECLARE_DYNAMIC(CMainPropSheet)

public:
	CPropPage1 m_Page1;
	CPropPage2 m_Page2;

	CMainPropSheet(UINT nIDCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
	CMainPropSheet(LPCTSTR pszCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
	virtual ~CMainPropSheet();

protected:
	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
};


