#include "stdafx.h"
#include "StockInformationCtrl.h"
#pragma comment(lib,"wininet.lib")



CStockInformationCtrl::CStockInformationCtrl(PDATEINFO pDateInfo,BOOL bIsInitialize)
{
	if (pDateInfo != NULL)
	{
		m_strDATESTART = pDateInfo->strDateStart;
		m_strDATEEND = pDateInfo->strDateEnd;
		delete pDateInfo;
		pDateInfo = NULL;
	}
	else
	{
		CTime t;
		t = CTime::GetCurrentTime();
		m_strDATESTART = t.Format(L"%Y-%m-%d");
		m_strDATEEND = t.Format(L"%Y-%m-%d");
	}
	if (GetModuleFileName(NULL, m_strSavePath.GetBuffer(MAX_PATH + 1), MAX_PATH))
	{
		m_strSavePath.ReleaseBuffer();
		int nPos = m_strSavePath.ReverseFind('\\') + 1;
		m_strSavePath = m_strSavePath.Left(nPos);
		m_strSavePath.Append(L"TempData\\");
	}
	m_bContinue = TRUE;
	m_hDownloadThread = INVALID_HANDLE_VALUE;
	for (size_t i = 0; i < THREADCOUNT; i++)
	{
		m_hSingleDownloadThread[i] = INVALID_HANDLE_VALUE;
		m_hEvent[i] = CreateEvent(NULL, TRUE, TRUE, NULL);
	}
	m_bIsInitialize = bIsInitialize;
	m_pWnd = NULL;
	m_pConnection = NULL;
	InitializeAccessConnection();
}

CStockInformationCtrl::~CStockInformationCtrl()
{
	Pause(FALSE);
	for (size_t i = 0; i < THREADCOUNT; i++)
	{
		if (WaitForSingleObject(m_hEvent[i], 3000) == WAIT_TIMEOUT)
		{
			::TerminateThread(m_hSingleDownloadThread[i], 0);
		}
	}
	if (m_hDownloadThread != INVALID_HANDLE_VALUE)
	{
		if (WaitForSingleObject(m_hDownloadThread, 3000) == WAIT_TIMEOUT)
		{
			::TerminateThread(m_hDownloadThread, 0);
		}
	}
	CloseAccessConnection();
}

E_RESULT CStockInformationCtrl::InitializeStocksList()
{
	CString strFilePath;
	if (!GetModuleFileName(NULL, strFilePath.GetBuffer(MAX_PATH + 1), MAX_PATH))
	{
		return FAIL;
	}
	strFilePath.ReleaseBuffer();
	int nPos = strFilePath.ReverseFind('\\') + 1;
	strFilePath = strFilePath.Left(nPos);
	strFilePath.Append(L"StocksList.ini");

	CStdioFile SFile;
	if (!SFile.Open(strFilePath, CStdioFile::modeRead | CStdioFile::typeBinary, NULL))
	{
		MessageBox(NULL, L"未找到StocksList.ini配置文件!", L"提示", MB_OK | MB_ICONINFORMATION);
		return FAIL;
	}
	else
	{
		CString strResult;
		SFile.SeekToBegin();
		//读Unicode第一行
		SFile.ReadString(strResult);
		if (strResult.GetAt(0) != 0xFEFF)
		{
			MessageBox(NULL, L"以Unicode读取StocksList.ini配置文件错误!", L"提示", MB_OK | MB_ICONINFORMATION);
			return FAIL;
		}
		while (SFile.ReadString(strResult))
		{
			CString id;
			CString name;
			CString type;
			int pos = -1;
			pos = strResult.Find('\t');
			if (pos != -1)
			{
				id = strResult.Mid(0,pos);
				strResult = strResult.Mid(pos + 1);
				pos = strResult.Find('\r');
				if (pos != -1)
				{
					name = strResult.Mid(0, pos);
				}
				else
				{
					name = strResult.Mid(0, strResult.GetLength());
				}

				type = CStockInformationCtrlHelper::GetStockType(id);
				if (type != L"")
				{
					STOCK_INFO stock_info = { id, name, type };
					m_vecStocksInfoList.push_back(stock_info);
				}
			}
		}
		SFile.Close();
	}
	ReportDownloadInfo(m_pWnd->GetSafeHwnd(), L"初始化成功！", 0);
	return SUCCESS;
}

E_RESULT CStockInformationCtrl::DownLoadStockData()
{
	CWinThread * pThread = AfxBeginThread((AFX_THREADPROC)DownloadFileThread, (LPVOID)this);
	if (pThread != NULL)
	{
		m_hDownloadThread = pThread->m_hThread;
	}
	ReportDownloadInfo(m_pWnd->GetSafeHwnd(), L"准备下载数据！", 0);
	return SUCCESS;
}

E_RESULT CStockInformationCtrl::SetParentWnd(CWnd *pWnd)
{
	if (IsWindow(pWnd->m_hWnd))
	{
		m_pWnd = pWnd;
		return SUCCESS;
	}
	return FAIL;
}

E_RESULT CStockInformationCtrl::Pause(BOOL bFlag)
{
	if (m_hDownloadThread == INVALID_HANDLE_VALUE)
	{
		return FAIL;
	}
	if (bFlag)
	{
		for (size_t i = 0; i < THREADCOUNT; i++)
		{
			if (WaitForSingleObject(m_hEvent[i], 1) == WAIT_TIMEOUT)
			{
				SuspendThread(m_hSingleDownloadThread[i]);
			}
		}
		if (m_hDownloadThread != INVALID_HANDLE_VALUE)
		{
			if (WaitForSingleObject(m_hDownloadThread, 1) == WAIT_TIMEOUT)
			{
				SuspendThread(m_hDownloadThread);
			}
		}
	}
	else
	{
		for (size_t i = 0; i < THREADCOUNT; i++)
		{
			ResumeThread(m_hSingleDownloadThread[i]);
		}
		ResumeThread(m_hDownloadThread);
	}
	return TRUE;
}

E_RESULT CStockInformationCtrl::Cancel()
{
	m_bContinue = FALSE;
	return SUCCESS;
}

DWORD CStockInformationCtrl::DownloadFileThread(LPVOID lpvoid)
{
	CStockInformationCtrl *pStockInfoCtrl = (CStockInformationCtrl *)lpvoid;
	if (!PathFileExists(pStockInfoCtrl->m_strSavePath))
	{
		if (!CreateDirectory(pStockInfoCtrl->m_strSavePath, 0))
		{
			ReportDownloadInfo(pStockInfoCtrl->m_pWnd->GetSafeHwnd(), L"创建文件失败！", 0);
			delete pStockInfoCtrl;
			pStockInfoCtrl = NULL;
			return 0;
		}
	}
	//初始化进度条
	size_t n = 0 , vecSize = pStockInfoCtrl->m_vecStocksInfoList.size();
	SendMessage(pStockInfoCtrl->m_pWnd->GetSafeHwnd(), WM_INITIAL_DOWNLOADINFO, (WPARAM)vecSize, NULL);
	for (std::vector<STOCK_INFO>::iterator it = pStockInfoCtrl->m_vecStocksInfoList.begin();pStockInfoCtrl->m_bContinue && it != pStockInfoCtrl->m_vecStocksInfoList.end(); ++it)
	{
		PSINGLE_DOWNLOAD_INFO pStockdownloadinfo = new SINGLE_DOWNLOAD_INFO{ it->ID, it->NAME, it->TYPE, pStockInfoCtrl , 0};
		//等待线程
		DWORD dwRet = 0;
		int nIndex = 0;
		BOOL bContinue = TRUE;
		while (pStockInfoCtrl->m_bContinue && bContinue)
		{
			dwRet = WaitForMultipleObjects(THREADCOUNT, pStockInfoCtrl->m_hEvent, FALSE, INFINITE);
			switch (dwRet)
			{
			case WAIT_TIMEOUT:
				break;
			case WAIT_FAILED:
				return 1;
			default:
			{
				nIndex = dwRet - WAIT_OBJECT_0;
				//保存线程的index，用于SetEvent
				pStockdownloadinfo->nIndex = nIndex;

				ResetEvent(pStockInfoCtrl->m_hEvent[nIndex]);
				CWinThread *pThread = AfxBeginThread((AFX_THREADPROC)SingleDownloadFileThread, pStockdownloadinfo);
				if (pThread)
				{
					pStockInfoCtrl->m_hSingleDownloadThread[nIndex] = pThread->m_hThread;
					bContinue = FALSE;
					break;
				}
				SetEvent(pStockInfoCtrl->m_hEvent[nIndex]);

			}
			}
		}
	}
	if (pStockInfoCtrl->m_bContinue)
	{
		ReportDownloadInfo(pStockInfoCtrl->m_pWnd->GetSafeHwnd(), L"全部数据获取成功！", 1);
		CLog::GetInstance().AddLog(L"全部数据获取成功！", NORMAL_LOG);
		::MessageBox(NULL, L"获取全部数据成功！", L"提示", MB_ICONINFORMATION | MB_OK);
		SendMessage(pStockInfoCtrl->m_pWnd->GetSafeHwnd(), WM_CLOSE, NULL, NULL);
	}
	return 1;
}

DWORD CStockInformationCtrl::SingleDownloadFileThread(LPVOID lpvoid)
{
	PSINGLE_DOWNLOAD_INFO pStockdownloadinfo = (PSINGLE_DOWNLOAD_INFO)lpvoid;
	ReportDownloadInfo(pStockdownloadinfo->pStockInfoCtrl->m_pWnd->GetSafeHwnd(), pStockdownloadinfo->ID + L" 处理中！", 0);
	int nRry = 0;
	while (pStockdownloadinfo->pStockInfoCtrl->m_bContinue)
	{
		CString strUrl = L"http://table.finance.yahoo.com/table.csv?s=";
		strUrl.Append(pStockdownloadinfo->ID);
		strUrl.Append(L".");
		strUrl.Append(pStockdownloadinfo->TYPE);
		//PPROGRESS_INFO pi = new PROGRESS_INFO{ pStock_info->ID + L" " + pStock_info->NAME, (int)(n * 100.0 / vecSize) };
		//SendMessage(pStockInfoCtrl->m_pWnd->GetSafeHwnd(), WM_UPDATE_DOWNLOADINFO, (WPARAM)pi, NULL);
		DWORD byteread = 0;
		char buffer[128];
		memset(buffer, 0, 128);
		HINTERNET internetopen;
		internetopen = InternetOpen(L"StocksDownload" + pStockdownloadinfo->ID, INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, 0);
		if (internetopen == NULL)
		{
			ReportDownloadInfo(pStockdownloadinfo->pStockInfoCtrl->m_pWnd->GetSafeHwnd(), pStockdownloadinfo->ID + L" 数据连接失败！", 0);
			//if (pStockInfoCtrl->m_bContinue && ::MessageBox(NULL, it->ID + L" " + it->NAME + L" 数据连接失败，是否重试？", L"提示", MB_ICONINFORMATION | MB_YESNO) != IDYES)
			//{
			//	continue;
			//}
			if (nRry < 2)
			{
				std::wstring temp(pStockdownloadinfo->ID + L" 数据连接失败！");
				CLog::GetInstance().AddLog(temp, WARNNING_LOG);
				nRry++;
				Sleep(5000);
				continue;
			}
			else
			{
				break;
			}
		}
		HINTERNET internetopenurl;
		internetopenurl = InternetOpenUrl(internetopen, strUrl, NULL, 0, INTERNET_FLAG_TRANSFER_BINARY | INTERNET_FLAG_NO_CACHE_WRITE | INTERNET_FLAG_RELOAD, 0);
		if (internetopenurl == NULL)
		{
			ReportDownloadInfo(pStockdownloadinfo->pStockInfoCtrl->m_pWnd->GetSafeHwnd(), pStockdownloadinfo->ID + L" 数据连接失败！", 0);
			InternetCloseHandle(internetopen);
			//if (pStockInfoCtrl->m_bContinue && ::MessageBox(NULL, it->ID + L" " + it->NAME + L" 数据连接失败，是否重试？", L"提示", MB_ICONINFORMATION | MB_YESNO) == IDYES)
			//{
			//	continue;
			//}
			if (nRry < 3)
			{
				std::wstring temp(pStockdownloadinfo->ID + L" 数据连接失败！");
				CLog::GetInstance().AddLog(temp, WARNNING_LOG);
				nRry++;
				Sleep(5000);
				continue;
			}
			else
			{
				break;
			}
		}

		DWORD written;
		HANDLE hfile;
		CString strFile;
		strFile = pStockdownloadinfo->pStockInfoCtrl->m_strSavePath;
		strFile.Append(pStockdownloadinfo->ID);
		strFile.Append(L"_");
		strFile.Append(pStockdownloadinfo->TYPE);
		hfile = CreateFile(strFile, GENERIC_ALL, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
		if (hfile == INVALID_HANDLE_VALUE)
		{
			InternetCloseHandle(internetopenurl);
			InternetCloseHandle(internetopen);
			ReportDownloadInfo(pStockdownloadinfo->pStockInfoCtrl->m_pWnd->GetSafeHwnd(), pStockdownloadinfo->ID + L" 创建文件失败！", 0);
			//if (pStockInfoCtrl->m_bContinue && ::MessageBox(NULL, it->ID + L" " + it->NAME + L"  创建文件失败，是否重试？", L"提示", MB_ICONINFORMATION | MB_YESNO) == IDYES)
			//{
			//	continue;
			//}
			//break;
			if (nRry < 4)
			{
				std::wstring temp(pStockdownloadinfo->ID + L" 创建文件失败！");
				CLog::GetInstance().AddLog(temp, WARNNING_LOG);
				nRry++;
				continue;
			}
			else
			{
				break;
			}
		}

		while (pStockdownloadinfo->pStockInfoCtrl->m_bContinue)
		{
			BOOL bRet = InternetReadFile(internetopenurl, buffer, sizeof(buffer), &byteread);
			if (byteread == 0 && bRet)
				break;
			WriteFile(hfile, buffer, byteread, &written, NULL);
		}
		CloseHandle(hfile);
		InternetCloseHandle(internetopenurl);
		InternetCloseHandle(internetopen);

		while (pStockdownloadinfo->pStockInfoCtrl->m_bContinue)
		{
			if (!pStockdownloadinfo->pStockInfoCtrl->ExcuteSQL(L"Select * from " + pStockdownloadinfo->ID) || pStockdownloadinfo->pStockInfoCtrl->m_bIsInitialize)
			{
				pStockdownloadinfo->pStockInfoCtrl->InitializeAccessTable(pStockdownloadinfo->ID, L"Dt DATETIME,OpenP CURRENCY,HighP CURRENCY,LowP CURRENCY,CloseP CURRENCY,Volume INTEGER,AdjCloseP CURRENCY");
			}			
			if (pStockdownloadinfo->pStockInfoCtrl->WriteToAccessTable(strFile, pStockdownloadinfo->ID, pStockdownloadinfo->pStockInfoCtrl->m_strDATESTART, pStockdownloadinfo->pStockInfoCtrl->m_strDATEEND))
			{
				break;
			}
			//if (::MessageBox(NULL, it->ID + L" " + it->NAME + L"  写入数据库失败，是否重试？", L"提示", MB_ICONINFORMATION | MB_YESNO) == IDNO)
			//{
			//	break;
			//}
			std::wstring temp(pStockdownloadinfo->ID + L" 写入数据库失败！");
			CLog::GetInstance().AddLog(temp, WARNNING_LOG);
			break;
		}
		break;
	}
	if (pStockdownloadinfo->pStockInfoCtrl->m_bContinue)
	{
		ReportDownloadInfo(pStockdownloadinfo->pStockInfoCtrl->m_pWnd->GetSafeHwnd(), pStockdownloadinfo->ID + L" 获取成功！", 1);
	}
	SetEvent(pStockdownloadinfo->pStockInfoCtrl->m_hEvent[pStockdownloadinfo->nIndex]);
	delete pStockdownloadinfo;
	pStockdownloadinfo = NULL;
	return 1;
}

void CStockInformationCtrl::ReportDownloadInfo(HWND hWnd ,CString strTIP, size_t nProgress)
{
	PPROGRESS_INFO pi = new PROGRESS_INFO{ strTIP, nProgress };
	SendMessage(hWnd, WM_UPDATE_DOWNLOADINFO, (WPARAM)pi, NULL);
}

E_RESULT CStockInformationCtrl::InitializeAccessConnection()
{
	CStringA strPath;
	if (GetModuleFileNameA(NULL, strPath.GetBuffer(MAX_PATH + 1), MAX_PATH))
	{
		strPath.ReleaseBuffer();
		int nPos = strPath.ReverseFind('\\') + 1;
		strPath = strPath.Left(nPos);
	}
	CStringA strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
	strConnection.Append(strPath);
	strConnection.Append("data_1.mdb");
	////////////连接数据库//////////////
	HRESULT hr;
	try
	{
		hr = m_pConnection.CreateInstance("ADODB.Connection");///创建Connection对象
		if (SUCCEEDED(hr))
		{
			hr = m_pConnection->Open(strConnection.GetBuffer(), "", "", adModeUnknown);///连接数据库
			///上面一句中连接字串中的Provider是针对ACCESS2000环境的，对于ACCESS97,需要改为:Provider=Microsoft.Jet.OLEDB.3.51;  }
		}
	}
	catch (_com_error e)///捕捉异常
	{
		CString errormessage;
		errormessage.Format(L"连接数据库失败!\r\n错误信息:%s", e.ErrorMessage());
		AfxMessageBox(errormessage);///显示错误信息
		return FALSE;
	}
	return TRUE;
}

E_RESULT CStockInformationCtrl::CloseAccessConnection()
{
	try
	{
		if (m_pConnection != NULL && m_pConnection->State == adStateOpen)
		{
		m_pConnection->Close();
		}
	}
	catch (_com_error )
	{
//		AfxMessageBox(e.Description());
	}
	return SUCCESS;
}

E_RESULT CStockInformationCtrl::ExcuteSQL(CString strCmd)
{
	_variant_t RecordsAffected;
	try
	{
		m_pConnection->Execute(strCmd.GetBuffer(), &RecordsAffected, adCmdText);
	}
	catch (_com_error &e)
	{
//		AfxMessageBox(L"SQL语句执行错误：" + e.Description());
		std::wstring temp(e.Description());
		CLog::GetInstance().AddLog(temp, WARNNING_LOG);
		return FALSE;
	}
	return SUCCESS;
}

E_RESULT CStockInformationCtrl::InitializeAccessTable(CString strTableName, CString strInitializeCommmd)
{
	CString strSql = L"DROP TABLE ";
	strSql.Append(strTableName);
	_variant_t RecordsAffected;
	try
	{
		m_pConnection->Execute(strSql.GetBuffer(), &RecordsAffected, adCmdText);
	}
	catch (_com_error )
	{
		//判断表是否存在
		//		AfxMessageBox(e.Description());
	}
	strSql = L"CREATE TABLE ";
	strSql.Append(strTableName);
	strSql.Append(L"(");
	strSql.Append(strInitializeCommmd);
	strSql.Append(L")");
	try
	{
		m_pConnection->Execute(strSql.GetBuffer(), &RecordsAffected, adCmdText);
	}
	catch (_com_error &e)
	{
		AfxMessageBox(L"表初始化失败：" + e.Description());
	}
	return SUCCESS;
}

E_RESULT CStockInformationCtrl::WriteToAccessTable(CString strFilepath, CString strTableName, CString strStartTime, CString strEndTime)
{
	CStdioFile sfile;
	if (!sfile.Open(strFilepath, CStdioFile::modeRead | CStdioFile::typeText))
	{
		std::wstring temp(L"打开数据文件 " + strFilepath + L" 失败!");
		CLog::GetInstance().AddLog(temp, WARNNING_LOG);
		return FAIL;
	}
	CString temp;
	sfile.ReadString(temp);
	if (temp != L"Date,Open,High,Low,Close,Volume,Adj Close")
	{
		//非正常数据文件
		std::wstring temp(L"数据文件 " + strFilepath + L" 未包含相关数据!");
		CLog::GetInstance().AddLog(temp, WARNNING_LOG);
		return FAIL;
	}
	int n = 0;
	while (m_bContinue && sfile.ReadString(temp))
	{
		if (n % 100 == 0)
		{
			ReportDownloadInfo(m_pWnd->GetSafeHwnd(), strTableName + L" 写入数据库中！", 0);
		}	
		CString date;
		float open, high, low, close, adjclose;
		LONG volume;
		int pos = temp.Find(',');
		if (pos == -1)
		{
			std::wstring temp(L"数据文件 " + strFilepath + L" 索引数据失败!");
			CLog::GetInstance().AddLog(temp, WARNNING_LOG);
			return FAIL;
		}
		date = temp.Mid(0, pos);
		if (!CStockInformationCtrlHelper::IsValidTime(date))
		{
			std::wstring temp(L"数据文件 " + strFilepath + L" 索引数据失败!");
			CLog::GetInstance().AddLog(temp, WARNNING_LOG);
			return FAIL;
		}
		//判断时间是否在开始期之前
		if (!CStockInformationCtrlHelper::TimeCompare(strStartTime, date))
		{
			break;
		}
		//判断时间范围
		if (!CStockInformationCtrlHelper::IsBetweenRangeTime(date, strStartTime, strEndTime))
		{
			continue;
		}
		temp = temp.Mid(pos);
		if (swscanf_s(temp.GetBuffer(temp.GetLength()), L",%f,%f,%f,%f,%ld,%f", &open, &high, &low, &close, &volume, &adjclose) != 6)
		{
			std::wstring temp(L"数据文件 " + strFilepath + L" 索引数据失败!");
			CLog::GetInstance().AddLog(temp, WARNNING_LOG);
			return FAIL;
		}
		CString sql;
		sql.Format(L"Insert into " + strTableName + L"(Dt,OpenP,HighP,LowP,CloseP,Volume,AdjCloseP)values('" + date + "',%f,%f,%f,%f,%ld,%f)", open, high, low, close, volume, adjclose);
		ExcuteSQL(sql);
		n++;
	}
	return SUCCESS;
}


CString CStockInformationCtrlHelper::GetStockType(CString strStock_id)
{
	//沪市代码600、601、603开头， 深市股票代码，000、001、002、300开头。 创业板是深圳交易所的一个子版块，300开头的股票。
	if (strStock_id.GetAt(0) == '6')
	{
		return L"ss";
	}
	if (strStock_id.GetAt(0) == '0' || strStock_id.GetAt(0) == '3')
	{
		return L"sz";
	}
	return L"";
}

BOOL CStockInformationCtrlHelper::IsBetweenRangeTime(CString strCheckTime, CString strStartTime, CString strEndTime)
{
	if (TimeCompare(strStartTime, strCheckTime) && TimeCompare(strCheckTime, strEndTime))
	{
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

BOOL CStockInformationCtrlHelper::IsValidTime(CString time)
{
	int nYear, nMonth, nDay;
	if (swscanf_s(time.GetBuffer(time.GetLength()), L"%d-%d-%d", &nYear, &nMonth, &nDay) != 3)
	{
		return FALSE;
	}
	COleDateTime dt;
	if (dt.SetDate(nYear, nMonth, nDay))
	{
		//判断日期合法
		return FALSE;
	}
	return TRUE;
}

BOOL CStockInformationCtrlHelper::TimeCompare(CString strTimeStart, CString strTimeEnd)
{
	int nYear, nMonth, nDay;
	if (swscanf_s(strTimeStart.GetBuffer(strTimeStart.GetLength()), L"%d-%d-%d", &nYear, &nMonth, &nDay) != 3)
	{
		return FALSE;
	}
	CTime  time1(nYear, nMonth, nDay, 0, 0, 0);
	if (swscanf_s(strTimeEnd.GetBuffer(strTimeEnd.GetLength()), L"%d-%d-%d", &nYear, &nMonth, &nDay) != 3)
	{
		return FALSE;
	}
	CTime  time2(nYear, nMonth, nDay, 0, 0, 0);
	CTimeSpan ts = time2 - time1;
	if (ts < 0)
	{
		return FALSE;
	}
	else
	{
		return TRUE;
	}
}