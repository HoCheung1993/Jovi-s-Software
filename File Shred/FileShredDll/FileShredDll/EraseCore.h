#pragma once
#include "Define.h"
#include "NtApi.h"

typedef UINT32 E_RESULT;

enum FILE_STATUS
{
	INITIAL = 0x2,
	NOTEXIST = 0x4,
	ONLYREAD = 0x8,
	WRITINGERROR = 0x10,
	DELETINGERROR = 0x20,
	RENAMEERROR = 0x40,
	GETFILEINFOERROR = 0x80,
	UNLOCKERROR = 0x100
};

enum FILE_TYPE
{
	TYPE_FILE,
	TYPE_FOLDER,
	TYPE_UNKNOWN
};

typedef struct _FILE_ERASE_INFO_
{
	WCHAR strFilePath[MAX_PATH + 1];
	UINT32 StatusCode;
	FILE_TYPE Filetype;
}FILE_ERASE_INFO, *PFILE_ERASE_INFO;


typedef struct _PROGRESS_INFO_
{
	WCHAR	wszFileName[MAX_PATH + 1];
	UINT64  lFilesize;  //當前文件大小
	UINT64  lFileShredSize;  //當前粉碎大小
	UINT32	nFileProgress;  //當前文件進度
	UINT32	nTotalProgress;  //所有文件總進度
	BOOL	bFinished;
}PROGRESS_INFO, *PPROGRESS_INFO;

typedef struct _SHRED_RESULT_INFO_
{
	size_t nTotalCount;
	size_t nFailedCount;
	UINT64 nTotalShredSize;
	std::vector<FILE_ERASE_INFO> m_VecFailedFiles;
}SHRED_RESULT_INFO, *PSHRED_RESULT_INFO;

class CFileErase
{
public:
	CFileErase();
	~CFileErase();
	//根据路径粉碎
	E_RESULT ShredPath(__in WCHAR *DirName, __in HWND hWnd);
	//取消
	E_RESULT Cancel(BOOL bFlag);
	//设定覆盖次数
	E_RESULT SetEraseCount(size_t n);
	//暫停
	E_RESULT Pause(BOOL bFlag);

private:
	E_RESULT CleanBufferVirtualAlloc();
	E_RESULT SafeWrite(__in const HANDLE &hFile, __in const LONG &length);
	E_RESULT SafeDelete(__in const std::wstring& FileName);
	E_RESULT SafeRename(__in const std::wstring& oldFileNamePath, __in std::wstring& newFileNamePath);
	E_RESULT ShredFiles(__in std::vector<FILE_ERASE_INFO> &VecFiles);
	E_RESULT Shred();
	static DWORD WINAPI ShredThreadProc(__in  LPVOID lpParameter);

	void ProcessShredProgress(PROGRESS_INFO &info);
	void ReportShredResult();

	DWORD m_dwThreadId;
	HANDLE m_hThread;
	HWND m_hWnd;
	UINT32 m_nEraseCount;
	UINT64 m_nTotalSize;
	UINT64 m_nTotalShrededSize;
	BOOLEAN m_bContinue;
	std::wstring m_strRoot;
	std::wstring m_strCurrentFile;
	std::vector<FILE_ERASE_INFO> m_VecFailedFiles;
	std::vector<FILE_ERASE_INFO> m_VecFiles;
	PBYTE m_cleanBuffer[5];
	PROGRESS_INFO	m_infoProgress;
	SHRED_RESULT_INFO m_infoShredRusult;
};

class CFileEraseHelper
{
public:
	friend class CFileErase;
	static E_RESULT	GetEraseInfo(__in WCHAR *wszPath, __inout std::vector<FILE_ERASE_INFO> &VecFiles, __inout UINT64 &nTotalFilesSize);
	static E_RESULT KillProcess(__in std::wstring ApplicationPath);

private:
	static int GetRandNumber(__in int range_min, __in int range_max);
	static E_RESULT GetRadomName(__out std::wstring &strRandname);
	static BOOL IsDirectory(__in const WCHAR *Dir);
	static BOOL FindFiles(__in const WCHAR *DirName, __out std::vector<FILE_ERASE_INFO> &VecFiles, __inout UINT64 &nTotalFilesSize);
	static BOOL DeleteDirectory(__in const WCHAR *DirName);
	static UINT64 GetShredFileSize(__in const std::wstring& strFile);
};
