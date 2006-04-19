#pragma once
#include "afxwin.h"


// CPropPage1 dialog

class CPropPage1 : public CPropertyPage
{
	DECLARE_DYNAMIC(CPropPage1)

public:
	CPropPage1();
	virtual ~CPropPage1();

// Dialog Data
	enum { IDD = IDD_PROPPAGE_1 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	// Version List box
	CListBox m_ctrlVersionListBx;
	CString m_strDefaultCLR;

	void Populate(void);
	virtual BOOL OnInitDialog();
	afx_msg void OnLbnSelchangeVersionsList();
	virtual void OnOK();
};
