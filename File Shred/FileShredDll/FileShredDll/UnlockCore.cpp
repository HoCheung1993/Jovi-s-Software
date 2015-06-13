#include "stdafx.h"
#include "UnlockCore.h"

CFileUnlocker::CFileUnlocker(const std::wstring& strFilePath)
{
	m_strFilePath = strFilePath;
	CFileUnlockerHelper::GetDevicePath(m_strFilePath, m_strDevicePath);
	size_t nIndex = m_strFilePath.find_last_of('\\') + 1;
	m_strFileName = m_strFilePath.substr(nIndex, m_strFilePath.length() - nIndex);
}

CFileUnlocker::~CFileUnlocker()
{

}

E_RESULT CFileUnlocker::Unlock(const PFILE_LOCKED_INFO lplockedInfo, size_t nInfoCount)
{
	size_t nSucessedCount = 0;
	NTDUPLICATEOBJECT NtDuplicateObject = (NTDUPLICATEOBJECT)GetProcAddress(GetModuleHandle(L"ntdll.dll"), "NtDuplicateObject");
	HANDLE hProcess = INVALID_HANDLE_VALUE;
	PFILE_LOCKED_INFO p = lplockedInfo;
	if (NtDuplicateObject == NULL)
	{
		return FAILE;
	}
	if (p == NULL)
	{
		return FAILE;
	}
	for (size_t i = 0; i < nInfoCount; i++)
	{
		hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, p[i].dwProcessID);
		if (hProcess == INVALID_HANDLE_VALUE)
		{
			continue;
		}
		if (!NT_SUCCESS(NtDuplicateObject(hProcess, p->hFile, NULL, NULL, GENERIC_ALL, 0, DUPLICATE_SAME_ACCESS | DUPLICATE_CLOSE_SOURCE)))  //关闭源句柄
		{
			CloseHandle(hProcess);
			continue;
		}
		nSucessedCount++;
		CloseHandle(hProcess);
	}
	return nSucessedCount;  //返回成功数
}

E_RESULT CFileUnlocker::UnlockAll()
{
	E_RESULT result = 0;
	ULONG nSize = 0x1000;
	PFILE_LOCKED_INFO lpLockedInfo = (PFILE_LOCKED_INFO)malloc(nSize);
	size_t n = 0;
	while (CFileUnlockerHelper::GetLockedInfo(lpLockedInfo, n, m_strFilePath) == LOWMENMORY)  //空间不够
	{
		lpLockedInfo = (PFILE_LOCKED_INFO)realloc(lpLockedInfo, nSize * 2);
	}
	result = Unlock(lpLockedInfo, n);
	if (lpLockedInfo != NULL)
	{
		free(lpLockedInfo);
		lpLockedInfo = NULL;
	}
	return result; //返回解锁个数
}

const std::wstring& CFileUnlocker::GetFileName()const
{
	return m_strFileName;
}

const std::wstring& CFileUnlocker::GetFilePath()const
{
	return m_strFilePath;
}

E_RESULT CFileUnlockerHelper::Initialize(UINT32 nRsvd)
{
	//预留空间
	return SUCCESS;
}

CFileUnlocker* CFileUnlockerHelper::GetFileUnlocker(const std::wstring& strFilePath)
{
	if (strFilePath.length() > 3 && strFilePath[1] == ':' && strFilePath[2] == '\\')  //判断文件路径是否合法
	{
		CFileUnlocker *pFileUnlocker = new CFileUnlocker(strFilePath);
		return pFileUnlocker;
	}
	return NULL;
}

E_RESULT CFileUnlockerHelper::GetLockedInfo(PFILE_LOCKED_INFO lpLockedInfo, size_t& nInfoCount, const std::wstring &strFilePath)
{
	std::wstring strDevicePath = L"";
	PFILE_LOCKED_INFO p = lpLockedInfo;
	nInfoCount = 0;
	NTQUERYSYSTEMINFORMATION NtQuerySystemInformation = (NTQUERYSYSTEMINFORMATION)GetProcAddress(GetModuleHandle(L"ntdll.dll"), "NtQuerySystemInformation");
	NTDUPLICATEOBJECT NtDuplicateObject = (NTDUPLICATEOBJECT)GetProcAddress(GetModuleHandle(L"ntdll.dll"), "NtDuplicateObject");
	NTQUERYOBJECT NtQueryObject = (NTQUERYOBJECT)GetProcAddress(GetModuleHandle(L"ntdll.dll"), "NtQueryObject");
	NTSTATUS status = 0;
	HANDLE hProcess = INVALID_HANDLE_VALUE;  //源进程句柄
	HANDLE hDupHandle = INVALID_HANDLE_VALUE; //复制的句柄
	HANDLE hThread = INVALID_HANDLE_VALUE;  //线程句柄
	PSYSTEM_HANDLE_INFORMATION pSystemHandleInfo = NULL;   //句柄信息
	ULONG ulSystemHandleInfoSize = 0x10000;  //64K 
	ULONG ulObjectNameInfoSize = 0x1000;
	PPUBLIC_OBJECT_NAME_INFORMATION pObjectNameInfo = NULL;
	PNM_INFO pNminfo = NULL;

	GetDevicePath(strFilePath, strDevicePath);
	if (strDevicePath == L"")
	{
		return FALSE;
	}

	if (p == NULL)
	{
		return FAILE;
	}

	if (NtQueryObject == NULL || NtDuplicateObject == NULL || NtQuerySystemInformation == NULL)
	{
		return FAILE;
	}

	pSystemHandleInfo = (PSYSTEM_HANDLE_INFORMATION)malloc(ulSystemHandleInfoSize);
	while ((status = NtQuerySystemInformation(SystemHandleInformation, pSystemHandleInfo, ulSystemHandleInfoSize, &ulSystemHandleInfoSize)) == STATUS_INFO_LENGTH_MISMATCH)  //获取句柄信息
	{
		pSystemHandleInfo = (PSYSTEM_HANDLE_INFORMATION)realloc(pSystemHandleInfo, ulSystemHandleInfoSize);
	}
	if (!NT_SUCCESS(status))
	{
		if (pSystemHandleInfo != NULL)
		{
			free(pSystemHandleInfo);
			pSystemHandleInfo = NULL;
		}
		return FAILE;
	}

	for (ULONG i = 0; i < pSystemHandleInfo->NumberOfHandles; i++)
	{
		SYSTEM_HANDLE_TABLE_ENTRY_INFO handle_info = pSystemHandleInfo->Handles[i];

		if (handle_info.UniqueProcessId == 4)  //跳过system
		{
			continue;
		}

		if (handle_info.ObjectTypeIndex != 30)  //只遍历FILE类型 
		{
			continue;
		}

		if (handle_info.UniqueProcessId == GetCurrentProcessId())  //本进程跳过
		{
			continue;
		}

		hProcess = OpenProcess(PROCESS_DUP_HANDLE, FALSE, handle_info.UniqueProcessId);
		if (hProcess == INVALID_HANDLE_VALUE)    //源进程句柄
		{
			continue;
		}

		if (!NT_SUCCESS(NtDuplicateObject(hProcess, (HANDLE)handle_info.HandleValue, GetCurrentProcess(), &hDupHandle, 0, 0, 0)))  //Dump句柄到当前进程
		{
			CloseHandle(hDupHandle);
			continue;
		}

		CloseHandle(hProcess);
		ulObjectNameInfoSize = 0x1000;
		pObjectNameInfo = (PPUBLIC_OBJECT_NAME_INFORMATION)malloc(ulObjectNameInfoSize);
		pObjectNameInfo->Name.Length = 0;
		pNminfo = new NM_INFO{ hDupHandle, pObjectNameInfo, ulObjectNameInfoSize };

		//线程查询防止死锁
		hThread = CreateThread(NULL, 0, GetFileNameThreadFunc, pNminfo, 0, NULL);
		if (WaitForSingleObject(hThread, 100) == WAIT_TIMEOUT)
		{
			TerminateThread(hThread, 0);
			CloseHandle(hThread);
			continue;
		}

		if (pObjectNameInfo->Name.Length)
		{
			std::wstring name(pObjectNameInfo->Name.Buffer);
			if (name.find(strDevicePath) != std::string::npos)    //static const size_t npos = -1;  find返回值是最大整数，所以比较与npos关系
			{
				p->dwProcessID = handle_info.UniqueProcessId;
				p->hFile = (HANDLE)handle_info.HandleValue;
				wcscpy_s(p->FilePath, name.c_str());
				nInfoCount++;
				p = p + 1;

				if (p == NULL)   //判断地址是否有效
				{
					if (pSystemHandleInfo != NULL)
					{
						free(pSystemHandleInfo);
						pSystemHandleInfo = NULL;
					}
					if (pObjectNameInfo != NULL)
					{
						free(pObjectNameInfo);
						pObjectNameInfo = NULL;
					}
					if (pNminfo != NULL)
					{
						delete(pNminfo);
						pNminfo = NULL;
					}
					CloseHandle(hThread);
					CloseHandle(hDupHandle);
					return LOWMENMORY;
				}
				CloseHandle(hThread);
				CloseHandle(hDupHandle);
				continue;
			}
		}
	}
	if (pSystemHandleInfo != NULL)
	{
		free(pSystemHandleInfo);
		pSystemHandleInfo = NULL;
	}
	if (pObjectNameInfo != NULL)
	{
		free(pObjectNameInfo);
		pObjectNameInfo = NULL;
	}
	if (pNminfo != NULL)
	{
		delete(pNminfo);
		pNminfo = NULL;
	}
	if (p != NULL)
	{
		p = NULL;
	}
	return nInfoCount == 0 ? FAILE : SUCCESS;
}

DWORD CFileUnlockerHelper::GetFileNameThreadFunc(PVOID lpParameter)
{
	NTQUERYOBJECT NtQueryObject = (NTQUERYOBJECT)GetProcAddress(GetModuleHandle(_T("ntdll.dll")), "NtQueryObject");
	PNM_INFO pNMInfo = (PNM_INFO)lpParameter;
	if (NtQueryObject == NULL)
	{
		std::cout << "Loading NtQueryObject Failed!" << std::endl;
		return FALSE;
	}
	while ((NtQueryObject(pNMInfo->hFile, ObjectNameInformation, pNMInfo->pObjectNameInfo, pNMInfo->ulObjectNameInfoSize, &pNMInfo->ulObjectNameInfoSize)) == STATUS_INFO_LENGTH_MISMATCH)  //进行查询
	{
		pNMInfo->pObjectNameInfo = (PPUBLIC_OBJECT_NAME_INFORMATION)realloc(pNMInfo->pObjectNameInfo, pNMInfo->ulObjectNameInfoSize);
	}
	return SUCCESS;
}

E_RESULT  CFileUnlockerHelper::GetDevicePath(std::wstring strFilePath, std::wstring& strDevicePath)
{
	strDevicePath = strFilePath;
	std::wstring temp = strDevicePath.substr(0, 2);
	strDevicePath = strDevicePath.substr(2, strDevicePath.length());
	const wchar_t *cDiskSymbol = temp.c_str();
	wchar_t szBuf[MAX_PATH];
	QueryDosDeviceW(cDiskSymbol, szBuf, MAX_PATH);
	strDevicePath = strDevicePath.insert(0, szBuf);
	return SUCCESS;
}

E_RESULT CFileUnlockerHelper::SetLocale(int category, char *oldlocale, size_t nSize, const char *locale)
{
	if (oldlocale != NULL)
	{
		memset(oldlocale, 0, nSize);
		if (strcpy_s(oldlocale, nSize, ::setlocale(category, NULL)) != 0)
		{
			return LOWMENMORY;
		}
	}
	return ::setlocale(category, locale) == NULL ? FAILE : SUCCESS;
}
