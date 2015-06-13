#pragma once
#include "NtApi.h"
#include "Define.h"

#define LOWMENMORY -1 

typedef struct _NM_INFO  //存储线程查询参数
{
	HANDLE  hFile;
	PPUBLIC_OBJECT_NAME_INFORMATION pObjectNameInfo;
	ULONG ulObjectNameInfoSize;
} NM_INFO, *PNM_INFO;

typedef UINT32 E_RESULT;

typedef struct _FILE_LOCKED_INFO_
{
	DWORD dwProcessID;
	HANDLE hFile;
	WCHAR FilePath[MAX_PATH];
}FILE_LOCKED_INFO, *PFILE_LOCKED_INFO;

class CFileUnlocker
{
public:
	CFileUnlocker(const std::wstring& strFilePath);
	~CFileUnlocker();
	E_RESULT		Unlock(const PFILE_LOCKED_INFO lplockedInfo, size_t nInfoCount);
	E_RESULT		UnlockAll();
	const std::wstring&	GetFileName() const;
	const std::wstring&	GetFilePath() const;
private:
	std::wstring m_strFileName;
	std::wstring m_strFilePath;
	std::wstring m_strDevicePath;
};

class CFileUnlockerHelper
{
public:
	friend class CFileUnlocker;
	E_RESULT	Initialize(UINT32 nRsvd);
	CFileUnlocker* GetFileUnlocker(const std::wstring& strFilePath);  //感觉这个归为静态方法也不错啊

	//静态方法
	static E_RESULT	GetLockedInfo(PFILE_LOCKED_INFO lpLockedInfo, size_t& nInfoCount, const std::wstring& strFilePath);
	static E_RESULT SetLocale(int category, char *oldlocale, size_t nSize, const char *locale);   //可返回原locale信息，不返回则参数为NULL，自动忽略nSize值
private:
	static DWORD WINAPI GetFileNameThreadFunc(PVOID lpParameter);
	static E_RESULT GetDevicePath(std::wstring strFilePath, std::wstring &strDevicePath);
};

