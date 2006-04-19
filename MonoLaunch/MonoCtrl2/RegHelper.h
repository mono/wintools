#pragma once

class CRegHelper
{
public:
	// Retrieves from the registry the default CLR
	static LRESULT GetDefaultCLR(LPTSTR szBuffer, DWORD dwBuffLen);

	// Retrieves from the registry the default CLR
	static LRESULT SetDefaultCLR(LPCTSTR szBuffer);

	CRegHelper(void);
	~CRegHelper(void);
};
