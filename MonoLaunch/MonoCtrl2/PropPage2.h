#pragma once



// CPropPage2 dialog

class CPropPage2 : public CPropertyPage
{
	DECLARE_DYNAMIC(CPropPage2)

public:
	CPropPage2();
	virtual ~CPropPage2();

// Dialog Data
	enum { IDD = IDD_PROPPAGE_2 };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
