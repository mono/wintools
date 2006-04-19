// The MIT License
// Copyright (c) 2004 Francisco "Paco" Martinez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#pragma once
#define INITGUID
#include <guiddef.h>
#include "fusion.h"

typedef HRESULT (__stdcall *CreateAsmCache)(IAssemblyCache **ppAsmCache, DWORD dwReserved);
typedef HRESULT (__stdcall *CreateAsmNameObj)(LPASSEMBLYNAME *ppAssemblyNameObj, LPCWSTR szAssemblyName, DWORD dwFlags, LPVOID pvReserved);
typedef HRESULT (__stdcall *CreateAsmEnum)(IAssemblyEnum **pEnum, IUnknown *pAppCtx, IAssemblyName *pName, DWORD dwFlags, LPVOID pvReserved);

class CGacHelper
{
public:
	// Global declarations
	HMODULE g_FusionDll;
	CreateAsmCache g_pfnCreateAssemblyCache;
	CreateAsmNameObj g_pfnCreateAssemblyNameObject;
	CreateAsmEnum g_pfnCreateAssemblyEnum;

	// Installs an Assembly in the GAC without any trace information
	HRESULT InstallAssemblyNoTrace(LPCTSTR szAssemblyFileName);

	// Installs an Assembly in the GAC with trace information
	HRESULT InstallAssemblyWithTrace(LPCTSTR szAssemblyFileName, LPCTSTR szAppID, LPCTSTR szDescription);
	
	// UnInstalls an Assembly from the GAC with trace information
	HRESULT UnInstallAssemblyWithTrace(LPCTSTR szAssemblyFullyQualifiedName, LPCTSTR szAppID, LPCTSTR szDescription);

	// Queries the GAC for the specified assembly and if found returns the path to it
	HRESULT QueryAssemblyInGac(LPCTSTR szFullySpecifiedAssamblyName, LPTSTR szAssemblyPath);

	// List all of the matches for the specified assembly
	HRESULT EnumerateAssemblyInGac(LPCTSTR szAssamblyName);
public:
	CGacHelper(void);
	~CGacHelper(void);
private:
	// Initializes and instantiates the Fusion API components
	void Initialize(void);
};
