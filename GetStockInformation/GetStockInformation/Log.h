#pragma once
#include <time.h>
#include <stdio.h>
#include <windows.h>
#include <string>


enum LOG_LEVEL
{
	NORMAL_LOG = 0,	
	WARNNING_LOG,	
	CRITICAL_LOG	
};

class CLog
{
public:
	static CLog& GetInstance();
	void AddLog(std::wstring strLog, LOG_LEVEL Log_level = NORMAL_LOG);

	void SetLogPath(const std::wstring& strFile_path);
	std::wstring GetLogPath();

private:
	CLog();
	~CLog();
	CLog& operator=(const CLog&);
	CLog(const CLog&);

private:
	BOOL Open();
	BOOL IsOpen();
	void Close();
	void Lock();
	void Unlock();
	DWORD WriteWideChar(std::wstring strLog);

protected:
	CRITICAL_SECTION m_csLock;
	std::wstring m_strFileName;
	HANDLE m_hFile;
};

