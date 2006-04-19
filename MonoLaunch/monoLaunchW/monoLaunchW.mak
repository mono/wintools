# The MIT License
# Copyright (c) 2004 Francisco "Paco" Martinez
# Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
# The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#
# Microsoft Developer Studio Generated NMAKE File, Based on monoLaunchW.dsp
!IF "$(CFG)" == ""
CFG=monoLaunchW - Win32 Debug
!MESSAGE No configuration specified. Defaulting to monoLaunchW - Win32 Debug.
!ENDIF 

!IF "$(CFG)" != "monoLaunchW - Win32 Release" && "$(CFG)" != "monoLaunchW - Win32 Debug"
!MESSAGE Invalid configuration "$(CFG)" specified.
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "monoLaunchW.mak" CFG="monoLaunchW - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "monoLaunchW - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "monoLaunchW - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE 
!ERROR An invalid configuration is specified.
!ENDIF 

!IF "$(OS)" == "Windows_NT"
NULL=
!ELSE 
NULL=nul
!ENDIF 

CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "monoLaunchW - Win32 Release"

OUTDIR=.\Release
INTDIR=.\Release
# Begin Custom Macros
OutDir=.\Release
# End Custom Macros

ALL : "$(OUTDIR)\monoLaunchW.exe"


CLEAN :
	-@erase "$(INTDIR)\MFConRegEditor.obj"
	-@erase "$(INTDIR)\monoLaunchW.obj"
	-@erase "$(INTDIR)\monoLaunchW.pch"
	-@erase "$(INTDIR)\monoLaunchW.res"
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(OUTDIR)\monoLaunchW.exe"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /ML /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\monoLaunchW.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
MTL_PROJ=/nologo /D "NDEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\monoLaunchW.res" /d "NDEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\monoLaunchW.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=shlwapi.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /incremental:no /pdb:"$(OUTDIR)\monoLaunchW.pdb" /machine:I386 /out:"$(OUTDIR)\monoLaunchW.exe" 
LINK32_OBJS= \
	"$(INTDIR)\monoLaunchW.obj" \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\monoLaunchW.res" \
	"$(INTDIR)\MFConRegEditor.obj"

"$(OUTDIR)\monoLaunchW.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ELSEIF  "$(CFG)" == "monoLaunchW - Win32 Debug"

OUTDIR=.\Debug
INTDIR=.\Debug
# Begin Custom Macros
OutDir=.\Debug
# End Custom Macros

ALL : "$(OUTDIR)\monoLaunchW.exe"


CLEAN :
	-@erase "$(INTDIR)\MFConRegEditor.obj"
	-@erase "$(INTDIR)\monoLaunchW.obj"
	-@erase "$(INTDIR)\monoLaunchW.pch"
	-@erase "$(INTDIR)\monoLaunchW.res"
	-@erase "$(INTDIR)\StdAfx.obj"
	-@erase "$(INTDIR)\vc60.idb"
	-@erase "$(INTDIR)\vc60.pdb"
	-@erase "$(OUTDIR)\monoLaunchW.exe"
	-@erase "$(OUTDIR)\monoLaunchW.ilk"
	-@erase "$(OUTDIR)\monoLaunchW.pdb"

"$(OUTDIR)" :
    if not exist "$(OUTDIR)/$(NULL)" mkdir "$(OUTDIR)"

CPP_PROJ=/nologo /MLd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\monoLaunchW.pch" /Yu"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
MTL_PROJ=/nologo /D "_DEBUG" /mktyplib203 /win32 
RSC_PROJ=/l 0x409 /fo"$(INTDIR)\monoLaunchW.res" /d "_DEBUG" 
BSC32=bscmake.exe
BSC32_FLAGS=/nologo /o"$(OUTDIR)\monoLaunchW.bsc" 
BSC32_SBRS= \
	
LINK32=link.exe
LINK32_FLAGS=shlwapi.lib kernel32.lib user32.lib gdi32.lib winspool.lib comdlg32.lib advapi32.lib shell32.lib ole32.lib oleaut32.lib uuid.lib odbc32.lib odbccp32.lib /nologo /subsystem:windows /incremental:yes /pdb:"$(OUTDIR)\monoLaunchW.pdb" /debug /machine:I386 /out:"$(OUTDIR)\monoLaunchW.exe" /pdbtype:sept 
LINK32_OBJS= \
	"$(INTDIR)\monoLaunchW.obj" \
	"$(INTDIR)\StdAfx.obj" \
	"$(INTDIR)\monoLaunchW.res" \
	"$(INTDIR)\MFConRegEditor.obj"

"$(OUTDIR)\monoLaunchW.exe" : "$(OUTDIR)" $(DEF_FILE) $(LINK32_OBJS)
    $(LINK32) @<<
  $(LINK32_FLAGS) $(LINK32_OBJS)
<<

!ENDIF 

.c{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.obj::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.c{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cpp{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<

.cxx{$(INTDIR)}.sbr::
   $(CPP) @<<
   $(CPP_PROJ) $< 
<<


!IF "$(NO_EXTERNAL_DEPS)" != "1"
!IF EXISTS("monoLaunchW.dep")
!INCLUDE "monoLaunchW.dep"
!ELSE 
!MESSAGE Warning: cannot find "monoLaunchW.dep"
!ENDIF 
!ENDIF 


!IF "$(CFG)" == "monoLaunchW - Win32 Release" || "$(CFG)" == "monoLaunchW - Win32 Debug"
SOURCE=.\MFConRegEditor.cpp

"$(INTDIR)\MFConRegEditor.obj" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\monoLaunchW.pch"


SOURCE=.\monoLaunchW.cpp

"$(INTDIR)\monoLaunchW.obj" : $(SOURCE) "$(INTDIR)" "$(INTDIR)\monoLaunchW.pch"


SOURCE=.\monoLaunchW.rc

"$(INTDIR)\monoLaunchW.res" : $(SOURCE) "$(INTDIR)"
	$(RSC) $(RSC_PROJ) $(SOURCE)


SOURCE=.\StdAfx.cpp

!IF  "$(CFG)" == "monoLaunchW - Win32 Release"

CPP_SWITCHES=/nologo /ML /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\monoLaunchW.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\monoLaunchW.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ELSEIF  "$(CFG)" == "monoLaunchW - Win32 Debug"

CPP_SWITCHES=/nologo /MLd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_MBCS" /Fp"$(INTDIR)\monoLaunchW.pch" /Yc"stdafx.h" /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 

"$(INTDIR)\StdAfx.obj"	"$(INTDIR)\monoLaunchW.pch" : $(SOURCE) "$(INTDIR)"
	$(CPP) @<<
  $(CPP_SWITCHES) $(SOURCE)
<<


!ENDIF 


!ENDIF 

