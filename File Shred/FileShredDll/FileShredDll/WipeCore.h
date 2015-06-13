#pragma once
#include "Define.h"

typedef UINT32 E_RESULT;

typedef struct _PROGRESS_WIPE_INFO_
{
	UINT32  nTotalWipeProgress;
	WCHAR   Tips[64];
	BOOL	bFinished;
}PROGRESS_WIPE_INFO, *PPROGRESS_WIPE_INFO;

typedef struct _MFT_INFO_
{
	//起始簇和数量
	UINT64 Startcluster;
	UINT64 ClusterCount;
}MFT_INFO, *PMFT_INFO;


class CDiskWipe
{
public:
	CDiskWipe();
	~CDiskWipe();
	//清理磁盤
	E_RESULT CleanDiskFreeSpace(__in WCHAR *DrivePath,__in HWND hWnd);
	//取消
	E_RESULT Cancel(__in BOOL bFlag);
	//设定覆盖次数
	E_RESULT SetWipeCount(__in size_t n);
	//暫停
	E_RESULT Pause(__in BOOL bFlag);

private:
	E_RESULT CleanBufferVirtualAlloc();
	E_RESULT WipeDisk();
	BOOLEAN CleanWrite(__in HANDLE FileHandle,__in ULONGLONG Length);
	E_RESULT CleanMFT();
	static DWORD WINAPI WipeThreadProc(LPVOID lpParameter);
	void ProcessWipeProgress(__in PROGRESS_WIPE_INFO &info);

	DWORD m_dwThreadId;
	HANDLE m_hThread;
	HWND m_hWnd;
	UINT32 m_nWipeCount;
	UINT64 m_nTotalSize;
	UINT64 m_nTotalShrededSize;
	BOOLEAN m_bContinue;
	std::wstring m_strWipePath;
	std::wstring m_strRoot;
	std::wstring m_strCurrentFile;
	PBYTE m_cleanBuffer[5];
};

class CDiskWipeHelper
{
public:
	friend class CDiskWipe;
private:
	static int GetRandNumber(__in int range_min, __in int range_max);
	static E_RESULT GetMFTinfo(__in WCHAR *DrivePath, std::vector<MFT_INFO> &vecMFTinfo);
};

