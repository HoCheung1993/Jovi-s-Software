#include "stdafx.h"
#include "Log.h"


CLog::CLog()
{
	::InitializeCriticalSection(&m_csLock);
	WCHAR buff[MAX_PATH + 1];
	GetModuleFileName(NULL, buff, sizeof(buff));
	m_strFileName = buff;
	int pos = m_strFileName.find_last_of('\\');
	if (pos != std::wstring::npos)
	{
		m_strFileName = m_strFileName.substr(0, pos);
	}
	else
	{
		m_strFileName = L"";
	}
	m_strFileName.append(L"\\log.log");
	m_hFile = INVALID_HANDLE_VALUE;
}

CLog::~CLog()
{
	::DeleteCriticalSection(&m_csLock);
	Close();
}

CLog& CLog::GetInstance()
{
	static CLog log;
	return log;
}

void CLog::AddLog(std::wstring strLog, LOG_LEVEL Log_level )
{
	Lock();
	Open();

	time_t now;
	time(&now);
	struct tm tmTmp;
	localtime_s(&tmTmp, &now);

	char temp[21];
	DWORD dwWriteLength;
	if (IsOpen())
	{
		time(&now);
		strftime(temp, 20, "%Y-%m-%d %H:%M:%S", &tmTmp);
		WriteFile(m_hFile, temp, 19, &dwWriteLength, NULL);
		WriteFile(m_hFile, "  ", 2, &dwWriteLength, NULL);
		switch (Log_level)
		{
		case NORMAL_LOG:
			WriteFile(m_hFile, "NORMAL: ", 8, &dwWriteLength, NULL);
			break;
		case WARNNING_LOG:
			WriteFile(m_hFile, "WARNNING: ", 10, &dwWriteLength, NULL);
			break;
		case CRITICAL_LOG:
			WriteFile(m_hFile, "CRITICAL: ", 10, &dwWriteLength, NULL);
			break;
		}
		WriteWideChar(strLog);
		WriteFile(m_hFile, "\r\n", 2, &dwWriteLength, NULL);
		FlushFileBuffers(m_hFile);
	}
	Close();
	Unlock();
}

void CLog::SetLogPath(const std::wstring& strFile_path)
{
	if (IsOpen())
	{
		Close();
	}
	m_strFileName = strFile_path;
}

std::wstring CLog::GetLogPath()
{
	return m_strFileName;
}

BOOL CLog::Open()
{
	if (IsOpen())
	{
		return TRUE;
	}
	if (m_strFileName == L"")
	{
		return FALSE;
	}
	m_hFile = CreateFile(m_strFileName.c_str(), GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (!IsOpen() && GetLastError() == 2)
	{
		//文件不存在，创建
		m_hFile = CreateFile(m_strFileName.c_str(), GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	}
	if (IsOpen())
		SetFilePointer(m_hFile, 0, NULL, FILE_END);
	return IsOpen();
}

BOOL CLog::IsOpen()
{
	return m_hFile != INVALID_HANDLE_VALUE;
}

void CLog::Close()
{
	if (IsOpen())
	{
		CloseHandle(m_hFile);
		m_hFile = INVALID_HANDLE_VALUE;
	}
}

void  CLog::Lock() 
{ 
	::EnterCriticalSection(&m_csLock);
}

void  CLog::Unlock() 
{ 
	::LeaveCriticalSection(&m_csLock);
}

DWORD CLog::WriteWideChar(std::wstring strLog )
{
	DWORD dwWriteLength = 0;
	std::string strTemp;
	if (IsOpen())
	{
		char*  buffer;
		int    nlen;
		nlen = WideCharToMultiByte(CP_ACP, 0, strLog.c_str(), -1, NULL, 0, NULL, NULL);
		buffer = new char[nlen + 1];
		memset((void*)buffer, 0, sizeof(char) * (nlen + 1));
		::WideCharToMultiByte(CP_ACP, 0, strLog.c_str(), -1, buffer, nlen, NULL, NULL);
		strTemp = buffer;
		delete[] buffer;
		WriteFile(m_hFile, strTemp.c_str(), strTemp.length(), &dwWriteLength, NULL);
	}
	return dwWriteLength;
}