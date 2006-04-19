// The MIT License
// Copyright (c) 2004 Francisco "Paco" Martinez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#include "StdAfx.h"
#include ".\gachelper.h"

CGacHelper::CGacHelper(void)
{
	g_FusionDll = NULL;
	g_pfnCreateAssemblyCache = NULL;
	g_pfnCreateAssemblyNameObject = NULL;
	g_pfnCreateAssemblyEnum = NULL;
	
	Initialize();
}

CGacHelper::~CGacHelper(void)
{
}

// UnInstalls an Assembly from the GAC with trace information
HRESULT CGacHelper::UnInstallAssemblyWithTrace(LPCTSTR szAssemblyFullyQualifiedName, LPCTSTR szAppID, LPCTSTR szDescription)
{
	HRESULT hr = E_NOTIMPL;
	FUSION_INSTALL_REFERENCE installReference;
	CString strAssemblyName;

	// Create an FUSION_INSTALL_REFERENCE struct and fill it with data
	installReference.cbSize = sizeof(FUSION_INSTALL_REFERENCE);
	installReference.dwFlags = 0;

	// We use opaque scheme here
	installReference.guidScheme = FUSION_REFCOUNT_OPAQUE_STRING_GUID;
	installReference.szIdentifier = szAppID;
	installReference.szNonCannonicalData = szDescription;

	// Get an IAssemblyCache interface
	IAssemblyCache* pCache = NULL;
	g_pfnCreateAssemblyCache(&pCache, 0);
	 
	// call IAssemblyCache::UninstallAssembly with reference
	// UninstallAssembly takes a fully specified (including processor architecture) assembly name as input.
	ULONG ulDisp = 0;
 
	hr = pCache->UninstallAssembly(0, (LPCTSTR)szAssemblyFullyQualifiedName, &installReference, &ulDisp);
	 
	pCache->Release();
	return hr;
}

// Installs an Assembly in the GAC with trace information
HRESULT CGacHelper::InstallAssemblyWithTrace(LPCTSTR szAssemblyFileName, LPCTSTR szAppID, LPCTSTR szDescription)
{
	HRESULT hr = E_NOTIMPL;
	FUSION_INSTALL_REFERENCE installReference;

	// Create an FUSION_INSTALL_REFERENCE struct and fill it with data
	::ZeroMemory(&installReference, sizeof(FUSION_INSTALL_REFERENCE));
	installReference.cbSize = sizeof(FUSION_INSTALL_REFERENCE);
	installReference.dwFlags = 0;

	// We use opaque scheme here
	installReference.guidScheme = FUSION_REFCOUNT_OPAQUE_STRING_GUID;
	installReference.szIdentifier = szAppID;
	installReference.szNonCannonicalData = szDescription;
	 
	// Get an IAssemblyCache interface
	IAssemblyCache* pCache = NULL;
	g_pfnCreateAssemblyCache(&pCache, 0);

	// call IAssemblyCache::InstallAssembly with reference
	hr = pCache->InstallAssembly(0, szAssemblyFileName, &installReference);

	pCache->Release();
	// Report result based on the return hr.
	return hr;
}

// Installs an Assembly in the GAC without any trace information
HRESULT CGacHelper::InstallAssemblyNoTrace(LPCTSTR szAssemblyFileName)
{
	HRESULT hr = E_NOTIMPL;

    // CreateAssemblyCache	
	IAssemblyCache* pCache = NULL;

	g_pfnCreateAssemblyCache(&pCache, 0);

	// call IAssemblyCache::InstallAssembly
	hr = pCache->InstallAssembly(0, szAssemblyFileName, NULL);

	pCache->Release();

	// Report result based on the return hr.
	return hr;
}

// Queries the GAC for the specified assembly and if found returns the path to it
HRESULT CGacHelper::QueryAssemblyInGac(LPCTSTR szFullySpecifiedAssamblyName, LPTSTR szAssemblyPath)
{
	HRESULT hrRetVal = E_NOTIMPL;
	ASSEMBLY_INFO info;

	ZeroMemory(&info, sizeof(ASSEMBLY_INFO));
	info.cbAssemblyInfo = sizeof(ASSEMBLY_INFO);
	info.pszCurrentAssemblyPathBuf = szAssemblyPath;
	info.cchBuf = MAX_PATH;

	// Get an IAssemblyCache interface
	IAssemblyCache*  pCache = NULL;
	g_pfnCreateAssemblyCache(&pCache, 0);

	// QueryAssemblyInfo takes a fully specified (including processor architecture) assembly name as input.
	// LPWSTR pszAssemblyName = L"MyAssembly, Version=1.0.0.0, Culture=Neutral, PublicKeyToken=0123456789abcdef, ProcessorArchitecture=MSIL";
	hrRetVal = pCache->QueryAssemblyInfo(0, szFullySpecifiedAssamblyName, &info);

	pCache->Release();

	return hrRetVal;
}
// List all of the matches for the specified assembly
HRESULT CGacHelper::EnumerateAssemblyInGac(LPCTSTR szAssamblyName)
{
	// Enumerate GAC for all the assembly szAssamblyName.
	// szAssamblyName can be any valid assembly display name.
	// If you want to see all assemblies, set pNameFilter to NULL

	HRESULT hrRetVal = S_FALSE;
	IAssemblyName* pNameFilter = NULL;
	IAssemblyEnum* pEnum = NULL;
	IAssemblyName* pAsmName = NULL;

	DWORD dwDisplayFlags = ASM_DISPLAYF_VERSION | ASM_DISPLAYF_CULTURE |
		ASM_DISPLAYF_PUBLIC_KEY_TOKEN | ASM_DISPLAYF_PROCESSORARCHITECTURE;

	DWORD dwLen = 0;
	LPWSTR szDisplayName = NULL;

	// First, create a filter for szAssamblyName
	g_pfnCreateAssemblyNameObject(&pNameFilter, szAssamblyName, CANOF_PARSE_DISPLAY_NAME, NULL);
	 
	// now create the IAssemblyEnum for GAC
	g_pfnCreateAssemblyEnum(&pEnum, NULL, pNameFilter, ASM_CACHE_GAC,  NULL);

	// Enumerating.
	// GetNextAssembly return S_OK when there are still assemblies exist for the enum,
	// and S_FALSE if there is nothing left.

	while (pEnum->GetNextAssembly(NULL, &pAsmName, 0) == S_OK)
	{
		// If we are here is that at least on match was found therefore
		hrRetVal = S_OK;

		// pAsmName is the assembly returned by the enum. Now lets get its display name
		dwLen = 0;

		// get the size first.
		pAsmName->GetDisplayName(NULL, &dwLen, dwDisplayFlags);

		// allocate memory
		szDisplayName = new WCHAR[dwLen];

		// re-try
		pAsmName->GetDisplayName(szDisplayName, &dwLen, dwDisplayFlags);

		// show it
		wprintf(L"%s\n", szDisplayName);

		delete[] szDisplayName;

		pAsmName->Release();
	}

	pEnum->Release();
	pNameFilter->Release();

	return hrRetVal;
}

// Initializes and instantiates the Fusion API components
void CGacHelper::Initialize(void)
{
	LoadLibraryShim(L"fusion.dll", 0, 0, &g_FusionDll);
	g_pfnCreateAssemblyCache = (CreateAsmCache)GetProcAddress(g_FusionDll, "CreateAssemblyCache");
	g_pfnCreateAssemblyNameObject = (CreateAsmNameObj)GetProcAddress(g_FusionDll, "CreateAssemblyNameObject");
	g_pfnCreateAssemblyEnum = (CreateAsmEnum)GetProcAddress(g_FusionDll, "CreateAssemblyEnum");
}
