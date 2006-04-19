// PropPage1.cpp : implementation file
//

#include "stdafx.h"
#include "MonoCtrl2.h"
#include "PropPage1.h"
#include ".\proppage1.h"


// CPropPage1 dialog

IMPLEMENT_DYNAMIC(CPropPage1, CPropertyPage)

CPropPage1::CPropPage1() : CPropertyPage(CPropPage1::IDD), m_strDefaultCLR(_T(""))
{
}

CPropPage1::~CPropPage1()
{
}

void CPropPage1::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_VERSIONS_LIST, m_ctrlVersionListBx);
	DDX_Text(pDX, IDC_SELECTEDCLR_EDT, m_strDefaultCLR);
}

BEGIN_MESSAGE_MAP(CPropPage1, CPropertyPage)
	ON_LBN_SELCHANGE(IDC_VERSIONS_LIST, OnLbnSelchangeVersionsList)
END_MESSAGE_MAP()


// CPropPage1 message handlers

BOOL CPropPage1::OnInitDialog()
{
	CPropertyPage::OnInitDialog();

	// TODO:  Add extra initialization here
	Populate();

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CPropPage1::OnLbnSelchangeVersionsList()
{
	CString strSelected;
	
	m_ctrlVersionListBx.GetText(m_ctrlVersionListBx.GetCurSel(), strSelected);
	m_strDefaultCLR = strSelected;
	
	UpdateData(FALSE);
}

void CPropPage1::Populate(void)
{
	LRESULT lRc = 0;
	HKEY hkMonoKey = NULL;
	DWORD dwType = REG_SZ;
	DWORD dwcbValLen = REGBUFFLEN;
	BYTE cbVal[REGBUFFLEN];
	CString strDefaultCLR;
	DWORD dwIdx = 0;
	TCHAR szName[REGBUFFLEN];
	FILETIME theFileTime;

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
		return;
	}

	do
	{
		ZeroMemory(szName, REGBUFFLEN);
		dwcbValLen = REGBUFFLEN;
		
		lRc = ::RegEnumKeyEx(
			hkMonoKey,
			dwIdx++,
			szName,
			&dwcbValLen,
			NULL,
			NULL,
			NULL,
			&theFileTime
			);
		
		if(lRc == ERROR_NO_MORE_ITEMS)
		{
			break;
		}
		
		this->m_ctrlVersionListBx.AddString(szName);

	}
	while(lRc == ERROR_SUCCESS);

	if(hkMonoKey != NULL)
	{
		::RegCloseKey(hkMonoKey);
	}
}


void CPropPage1::OnOK()
{
	UpdateData(TRUE);

	CPropertyPage::OnOK();
}
