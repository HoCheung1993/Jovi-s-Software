#include "stdafx.h"
#include "VersionHelper.h"

BOOL CVersionHelper::IsWindowsXPOrGreater()
{
	BOOL IsXP = FALSE;
	OSVERSIONINFO OVS;
	OVS.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	if (::GetVersionEx(&OVS))
	{
		if (OVS.dwPlatformId == VER_PLATFORM_WIN32_NT && OVS.dwMajorVersion >= 5 && OVS.dwMinorVersion >= 1)
		{
			IsXP = TRUE;
		}
	}
	return IsXP;
}

BOOL CVersionHelper::IsWindowsVistaOrGreater()
{
	BOOL IsVista = FALSE;
	OSVERSIONINFO OVS;
	OVS.dwOSVersionInfoSize = sizeof(OSVERSIONINFO);
	if (::GetVersionEx(&OVS))
	{
		if (OVS.dwPlatformId == VER_PLATFORM_WIN32_NT && OVS.dwMajorVersion >= 6 && OVS.dwMinorVersion >= 0)
		{
			IsVista = TRUE;
		}
	}
	return IsVista;
}