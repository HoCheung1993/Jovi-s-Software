#pragma once
#include <vector>
#include <wininet.h>

//线程数
#define THREADCOUNT 8
class CStockInformationCtrl;

typedef struct _STOCK_INFO_
{
	CString ID;
	CString NAME;
	CString TYPE;
}STOCK_INFO ,*PSTOCK_INFO;

typedef struct _SINGLE_DOWNLOAD_INFO_
{
	CString ID;
	CString NAME;
	CString TYPE;
	CStockInformationCtrl* pStockInfoCtrl;
	int nIndex;  //句柄ID
}SINGLE_DOWNLOAD_INFO, *PSINGLE_DOWNLOAD_INFO;

#define WM_INITIAL_DOWNLOADINFO WM_USER + 101
#define WM_UPDATE_DOWNLOADINFO WM_USER + 102
typedef struct _PROGRESS_INFO_
{
	CString strTIPS;
	size_t nPROGRESS;
}PROGRESS_INFO, *PPROGRESS_INFO;


class CStockInformationCtrl
{
public:
	CStockInformationCtrl(PDATEINFO pDateInfo, BOOL bIsInitialize);
	~CStockInformationCtrl();
	E_RESULT InitializeStocksList();
	E_RESULT DownLoadStockData();
	E_RESULT SetParentWnd(CWnd *pWnd);
	E_RESULT Pause(BOOL bFlag);
	E_RESULT Cancel();

private:
	BOOL m_bIsInitialize;
	BOOL m_bContinue;
	CWnd *m_pWnd;
	std::vector<STOCK_INFO> m_vecStocksInfoList;
	CString m_strSavePath;
	HANDLE m_hEvent[THREADCOUNT];
	HANDLE m_hSingleDownloadThread[THREADCOUNT];
	HANDLE m_hDownloadThread;
	_ConnectionPtr m_pConnection;
	CString m_strDATESTART;
	CString m_strDATEEND;
	static void ReportDownloadInfo(HWND hWnd, CString TIP, size_t nProgress);
	static DWORD CStockInformationCtrl::DownloadFileThread(LPVOID lpvoid);
	static DWORD CStockInformationCtrl::SingleDownloadFileThread(LPVOID lpvoid);

	E_RESULT ExcuteSQL(CString strCmd);
	E_RESULT InitializeAccessTable(CString strTableName, CString strInitializeCommmd);
	E_RESULT WriteToAccessTable(CString strFilepath, CString strTableName, CString strStartTime, CString strEndTime);

	E_RESULT InitializeAccessConnection();
	E_RESULT CloseAccessConnection();
};

class CStockInformationCtrlHelper
{
public:
	//深市和上市识别
	static CString GetStockType(CString strStock_id);
	static BOOL IsValidTime(CString time);
	static BOOL TimeCompare(CString strTimeStart, CString strTimeEnd);
	static BOOL IsBetweenRangeTime(CString strCheckTime, CString strStartTime, CString strEndTime);
};