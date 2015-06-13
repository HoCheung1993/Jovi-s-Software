#include "stdafx.h"
#include "WipeCore.h"

CDiskWipe::CDiskWipe()
	:m_hThread(INVALID_HANDLE_VALUE),
	m_nTotalShrededSize(0),
	m_nTotalSize(0),
	m_bContinue(TRUE),
	m_strRoot(L""),
	m_strCurrentFile(L""),
	m_strWipePath(L""),
	m_nWipeCount(1)
{
	m_dwThreadId = GetCurrentThreadId();
	CleanBufferVirtualAlloc();
}

CDiskWipe::~CDiskWipe()
{
	if (WAIT_OBJECT_0 != ::WaitForSingleObject(m_hThread, 1000)){
		::TerminateThread(m_hThread, -1);
	}
	for (size_t i = 0; i < 5; i++)
	{
		VirtualFree(m_cleanBuffer[i], 0, MEM_RELEASE);
		m_cleanBuffer[i] = NULL;
	}
}

E_RESULT CDiskWipe::CleanDiskFreeSpace(WCHAR *DrivePath, HWND hWnd)
{
	std::wstring temp(DrivePath);
	if (!(temp.length() == 3 && temp[1] == ':' && temp[2] == '\\'))
	{
		return FAILE;
	}
	m_strWipePath = temp.substr(0, 3);
	m_hWnd = hWnd;
	m_hThread = ::CreateThread(NULL
	, 0
	, WipeThreadProc
	, this
	, 0
	, NULL);
	if (m_hThread == INVALID_HANDLE_VALUE)
	{
	return GetLastError();
	}
	return SUCCESS;
}

E_RESULT CDiskWipe::Cancel(BOOL bFlag)
{
	Pause(FALSE);
	m_bContinue = bFlag;
	return SUCCESS;
}

E_RESULT CDiskWipe::SetWipeCount(size_t n)
{
	if (n > 0 && n < 8)
	{
		m_nWipeCount = n;
		return SUCCESS;
	}
	return FAILE;
}

E_RESULT CDiskWipe::Pause(BOOL nFlag)
{
	if (m_hThread == INVALID_HANDLE_VALUE)
	{
		return FAILE;
	}
	if (nFlag)
	{
		//掛起線程
		SuspendThread(m_hThread);
	}
	else
	{
		ResumeThread(m_hThread);
	}
	return SUCCESS;
}

E_RESULT CDiskWipe::CleanBufferVirtualAlloc()
{
	srand((unsigned)time(0));
	for (size_t i = 0; i < 5; i++)
	{
		m_cleanBuffer[i] = (PBYTE)VirtualAlloc(NULL, CLEAN_BUF_SIZE, MEM_COMMIT, PAGE_READWRITE);
		if (m_cleanBuffer[i] == NULL)
		{
			for (size_t j = 0; j < i; j++)
			{
				VirtualFree(m_cleanBuffer[j], 0, MEM_RELEASE);
				m_cleanBuffer[j] = NULL;
			}
			return FAILE;
		}
		//第一次00 第二次FF 第三次以后随机数 
		switch (i)
		{
		case 0:
			memset(m_cleanBuffer[i], 0, CLEAN_BUF_SIZE);
			break;
		case 1:
			memset(m_cleanBuffer[i], 0xFF, CLEAN_BUF_SIZE);
			break;
		case 2:
			for (int j = 0; j < CLEAN_BUF_SIZE; j++)
				m_cleanBuffer[i][j] = (BYTE)rand();
			break;
		case 3:
			for (int j = 0; j < CLEAN_BUF_SIZE; j++)
				m_cleanBuffer[i][j] = (BYTE)rand();
			break;
		case 4:
			for (int j = 0; j < CLEAN_BUF_SIZE; j++)
				m_cleanBuffer[i][j] = (BYTE)rand();
			break;
		}
	}
	return SUCCESS;
}

//磁盤置0
E_RESULT CDiskWipe::WipeDisk()
{
	WCHAR       DrivePath[MAX_PATH];
	TCHAR		tempFileName[MAX_PATH];
	ULARGE_INTEGER bytesAvail, totalBytes, freeBytes;
	DWORD		sectorsPerCluster, bytesPerSector, totalClusters, freeClusters;
	ULONGLONG	tempSize = 0, cleanSize;
	HANDLE		hTempFile;
	DWORD		percent;
	DWORD		prevPercent = 0;
	USHORT Compression = COMPRESSION_FORMAT_NONE;
	DWORD BytesReturned;
	PROGRESS_WIPE_INFO pwi;
	std::vector<HANDLE> vecMFTHANDLE;

	ZeroMemory(&pwi, sizeof(PROGRESS_WIPE_INFO));
	wcscpy_s(DrivePath, m_strWipePath.c_str());

	////检查分区格式是否为NTFS
	WCHAR szSYSTEMNAME[16];
	GetVolumeInformation(m_strWipePath.c_str(), NULL, 0, NULL, NULL, NULL, szSYSTEMNAME, 16);
	if (wcscmp(szSYSTEMNAME, L"NTFS"))
	{
		//若分区不是NTFS
		wcscpy_s(pwi.Tips, L"");
		pwi.nTotalWipeProgress = 0;
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		MessageBox(NULL, L"不支持非NTFS分區！", L"", MB_OK | MB_ICONSTOP);
		return FAILE;
	}

	pwi.nTotalWipeProgress = 0;
	wcscpy_s(pwi.Tips, L"準備清理...");
	pwi.bFinished = FALSE;
	ProcessWipeProgress(pwi);

	if ((DrivePath[1] != ':' || DrivePath[2] != '\\') && (m_bContinue))
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"此磁盤無法清理！");
		pwi.nTotalWipeProgress = 0;
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FALSE;
	}
	DrivePath[3] = 0;

	if (!GetDiskFreeSpace(DrivePath, &sectorsPerCluster, &bytesPerSector, &freeClusters, &totalClusters) && (m_bContinue))
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"無法獲得磁盤簇大小！");
		pwi.nTotalWipeProgress = 0;
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FAILE;
	}
	bytesAvail.QuadPart = sectorsPerCluster * freeClusters * bytesPerSector;
	freeBytes.QuadPart = bytesAvail.QuadPart;
	if (!GetDiskFreeSpaceEx(DrivePath, &bytesAvail, &totalBytes, &freeBytes) && (m_bContinue))
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"無法獲得磁盤大小！");
		pwi.nTotalWipeProgress = 0;
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FAILE;
	}

	if ((bytesAvail.QuadPart != freeBytes.QuadPart) && (m_bContinue))
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"無法獲得磁盤清理權限！");
		pwi.nTotalWipeProgress = 0;
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FALSE;
	}

	if (m_bContinue)
	{
		_stprintf_s(tempFileName, _T("%sCLEANTEMP"), DrivePath);
		hTempFile = CreateFile(tempFileName, GENERIC_WRITE | GENERIC_READ,
			0, NULL, CREATE_ALWAYS,
			FILE_FLAG_NO_BUFFERING | FILE_FLAG_SEQUENTIAL_SCAN |
			FILE_FLAG_DELETE_ON_CLOSE | FILE_ATTRIBUTE_HIDDEN, NULL);

		if (hTempFile == INVALID_HANDLE_VALUE)
		{
			pwi.nTotalWipeProgress = 0;
			wcscpy_s(pwi.Tips, L"權限不足，無法清理！");
			pwi.bFinished = TRUE;
			ProcessWipeProgress(pwi);
			CloseHandle(hTempFile);
			return FALSE;
		}
		vecMFTHANDLE.push_back(hTempFile);
		DeviceIoControl(hTempFile, FSCTL_SET_COMPRESSION, &Compression,
			sizeof(Compression), NULL, 0, &BytesReturned, NULL);

		// 以簇大小申請清理的buf
		cleanSize = (ULONGLONG)sectorsPerCluster * (ULONGLONG)bytesPerSector * 128;

		// 增加大小直到失敗
		while ((cleanSize > bytesPerSector * sectorsPerCluster) && (m_bContinue))
		{
			if (CleanWrite(hTempFile, cleanSize))
			{
				tempSize += cleanSize;

				percent = (DWORD)((tempSize * 100) / freeBytes.QuadPart);

				if (percent != prevPercent)
				{
					pwi.nTotalWipeProgress = percent;
					wcscpy_s(pwi.Tips, L"清理中...");
					pwi.bFinished = FALSE;
					ProcessWipeProgress(pwi);
					prevPercent = percent;
				}
			}
			else
			{
				cleanSize -= bytesPerSector * sectorsPerCluster;
			}
		}
	}
	//清理小於一個簇的空間
	if (m_bContinue)
	{
		_stprintf_s(tempFileName, _T("%sCLEANTEMP1"), DrivePath);
		hTempFile = CreateFile(tempFileName, GENERIC_WRITE,
			0, NULL, CREATE_NEW,
			FILE_FLAG_SEQUENTIAL_SCAN | FILE_FLAG_DELETE_ON_CLOSE |
			FILE_ATTRIBUTE_HIDDEN | FILE_FLAG_WRITE_THROUGH, NULL);

		if (hTempFile != INVALID_HANDLE_VALUE)
		{
			vecMFTHANDLE.push_back(hTempFile);
			while (cleanSize && m_bContinue)
			{
				if (!CleanWrite(hTempFile, cleanSize))
				{
					cleanSize--;
				}
			}
		}
	}

	for (std::vector<HANDLE>::iterator it = vecMFTHANDLE.begin(); it != vecMFTHANDLE.end(); ++it)
	{
		CloseHandle(*it);
	}

	//清理MFT
	if (m_bContinue)
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"開始清理MFT!");
		pwi.bFinished = FALSE;
		ProcessWipeProgress(pwi);
		CleanMFT();
	}
	if (m_bContinue)
	{
		pwi.nTotalWipeProgress = 100;
		wcscpy_s(pwi.Tips, L"已完成！");
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
	}
	return TRUE;
}

DWORD WINAPI CDiskWipe::WipeThreadProc(LPVOID lpParameter)
{
	CDiskWipe* p = (CDiskWipe *)lpParameter;
	p->WipeDisk();
	return 0;
}

BOOLEAN CDiskWipe::CleanWrite(HANDLE FileHandle, ULONGLONG Length)
{
	ULONGLONG	totalWritten;
	ULONG		bytesWritten, bytesToWrite;
	LARGE_INTEGER	seekLength;
	BOOLEAN		status;
	//覆蓋次數
	DWORD nCount = 1;
	for (DWORD i = 0; ((i < nCount) && (m_bContinue)); i++)
	{
		if (i != 0)
		{
			seekLength.QuadPart = -(LONGLONG)Length;
			SetFilePointer(FileHandle, seekLength.LowPart, &seekLength.HighPart, FILE_CURRENT);
		}
		totalWritten = 0;
		while ((totalWritten < Length) && (m_bContinue))
		{
			if (Length - totalWritten > 1024 * 1024)
			{
				bytesToWrite = 1024 * 1024;
			}
			else
			{
				bytesToWrite = (ULONG)(Length - totalWritten);
			}
			if (bytesToWrite > CLEAN_BUF_SIZE)
				bytesToWrite = CLEAN_BUF_SIZE;
			status = WriteFile(FileHandle, m_cleanBuffer[0], bytesToWrite, &bytesWritten, NULL);
			if (!status)
				return FALSE;
			totalWritten += bytesWritten;
		}
	}
	return TRUE;
}

E_RESULT CDiskWipe::CleanMFT()
{
	if ((m_strWipePath[1] != ':' || m_strWipePath[2] != '\\') && (m_bContinue))
	{
		return FAILE;
	}
	WCHAR WipePath[7] = L"\\\\.\\";
	WipePath[4] = m_strWipePath[0];
	WipePath[5] = m_strWipePath[1];
	WipePath[6] = 0;

	PROGRESS_WIPE_INFO pwi = { 0 };
	//检查分区格式是否为NTFS
	WCHAR szSYSTEMNAME[16];
	GetVolumeInformation(NULL, NULL, 0, NULL, NULL, NULL, szSYSTEMNAME, 16);
	if (wcscmp(szSYSTEMNAME, L"NTFS"))
	{
		//若分区不是NTFS
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"不支持非NTFS分區!");
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FAILE;
	}

	//获得系统盘符
	WCHAR szText[MAX_PATH] = L"";
	GetSystemDirectory(szText, MAX_PATH);
	//如果为系统盘则不清理MFT
	if (m_strWipePath[0] == szText[0])
	{
		pwi.nTotalWipeProgress = 100;
		wcscpy_s(pwi.Tips, L"不支持清理系統分區!");
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
		return FAILE;
	}

	if (m_bContinue)
	{
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"正在搜索已刪除文件表...");
		pwi.bFinished = FALSE;
		ProcessWipeProgress(pwi);
	}

	std::vector<MFT_INFO> vecMFTinfo;
	if ((m_bContinue) && (!CDiskWipeHelper::GetMFTinfo(WipePath, vecMFTinfo)))
	{
		return FAILE;
	}

	if (vecMFTinfo.size() == 0)
	{
		return FAILE;
	}

	//找到已删除文件的MFT
	BYTE CleanBuff[1024];
	BYTE BUFF[1024];
	std::vector<UINT64> vecRubbishMFT;
	ZeroMemory(&CleanBuff, 1024);
	HANDLE hFile = CreateFile(WipePath, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		return FAILE;
	}
	for (std::vector<MFT_INFO>::size_type i = 0; i < vecMFTinfo.size() && m_bContinue; ++i)
	{
		DWORD CB = 0;
		UINT64 nCount = 0;
		LARGE_INTEGER liOffset = { 0 };
		liOffset.QuadPart = vecMFTinfo[i].Startcluster * 8 * 512;
		//遍历MFT
		while ((nCount < vecMFTinfo[i].ClusterCount * 4) && m_bContinue)
		{
			SetFilePointer(hFile, liOffset.LowPart, &liOffset.HighPart, FILE_BEGIN);
			ZeroMemory(BUFF, 1024);
			ReadFile(hFile, &BUFF, 1024, &CB, NULL);
			if (BUFF[0] == 'F' && BUFF[1] == 'I' && BUFF[2] == 'L' && BUFF[3] == 'E' && BUFF[4] == '0')
			{
				if (BUFF[22] == 0)
				{
					if (BUFF[56] != 255 || BUFF[57] != 255 || BUFF[58] != 255 || BUFF[59] != 255)
					{
						//需要清理的offset
						vecRubbishMFT.push_back(liOffset.QuadPart);
					}
				}
			}
			liOffset.QuadPart += 1024;
			nCount++;
		}
	}

	//无法对系统盘操作
	if (m_bContinue)
	{
		DWORD dwOutBytes = 0;
		//一定要有参数接受返回的written值 不然程序崩溃
		BOOL bDISMOUT = DeviceIoControl(hFile, FSCTL_DISMOUNT_VOLUME, NULL, 0, NULL, 0, &dwOutBytes, NULL);
		if (!bDISMOUT)
		{
			CloseHandle(hFile);
			return FAILE;
		}
		pwi.nTotalWipeProgress = 0;
		wcscpy_s(pwi.Tips, L"開始清理MFT!");
		pwi.bFinished = FALSE;
		ProcessWipeProgress(pwi);
	}

	LARGE_INTEGER li = { 0 };
	for (std::vector<UINT64>::size_type i = 0; i < vecRubbishMFT.size() && m_bContinue; ++i)
	{
		DWORD CB = 0;
		li.QuadPart = vecRubbishMFT[i];
		SetFilePointer(hFile, li.LowPart, &li.HighPart, NULL);
		BOOL brtn = WriteFile(hFile, &CleanBuff, 1024,&CB, NULL);
		FlushFileBuffers(hFile);
		pwi.nTotalWipeProgress = (UINT32)((((double)(i + 1)  / (double)(vecRubbishMFT.size())) * 100));
		wcscpy_s(pwi.Tips, L"正在清理MFT！");
		pwi.bFinished = FALSE;
		ProcessWipeProgress(pwi);
	}
	if (m_bContinue)
	{
		pwi.nTotalWipeProgress = 100;
		wcscpy_s(pwi.Tips, L"清除MFT成功！");
		pwi.bFinished = TRUE;
		ProcessWipeProgress(pwi);
	}
	CloseHandle(hFile);
	return SUCCESS;
}

void CDiskWipe::ProcessWipeProgress(PROGRESS_WIPE_INFO &info)
{
	if (m_bContinue)
	{
		::SendMessage(m_hWnd, WM_UPDATE_WIPE_PROCESS, (WPARAM)&info, 0);
	}
}



int CDiskWipeHelper::GetRandNumber(int range_min, int range_max)
{
	int u = (int)((double)rand() / (RAND_MAX + 1) * (range_max - range_min) + range_min);
	return u;
}

E_RESULT CDiskWipeHelper::GetMFTinfo(WCHAR *DrivePath, std::vector<MFT_INFO> &vecMFTinfo)
{
	WCHAR driver[7];
	ZeroMemory(driver, 7);
	wcscpy_s(driver, 7, DrivePath);
	if (driver[0] != '\\' || driver[1] != '\\' || driver[2] != '.' || driver[3] != '\\' || driver[5] != ':')
	{
		return FAILE;
	}

	vecMFTinfo.clear();

	HANDLE hFile = CreateFile(driver, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
	if (hFile == INVALID_HANDLE_VALUE)
	{
		return FAILE;
	}

	//获取NTFS信息
	BYTE buffer[1024] = { 0 };
	BYTE CleanBuff[1024] = { 0 };
	UINT32 BytesPerSecter = 0;
	UINT32 SecterPerCluster = 0;
	UINT64 MftStart = 0;
	UINT64 MftMirrStart = 0;
	UINT64 nCount = 0;
	BOOL Has80H = FALSE;
	DWORD CB = 0;

	//定位$MFT
	SetFilePointer(hFile, 0, NULL, FILE_BEGIN);
	ReadFile(hFile, buffer, 1024, &CB, NULL);
	BytesPerSecter = buffer[11] + buffer[12] * 256;
	SecterPerCluster = buffer[13];
	MftStart = buffer[55] * 0x100000000000000 + buffer[54] * 0x1000000000000 + buffer[53] * 0x10000000000 + buffer[52] * 0x100000000 + buffer[51] * 0x1000000 + buffer[50] * 0x10000 + buffer[49] * 0x100 + buffer[48];
	MftMirrStart = buffer[63] * 0x100000000000000 + buffer[62] * 0x1000000000000 + buffer[61] * 0x10000000000 + buffer[60] * 0x100000000 + buffer[59] * 0x1000000 + buffer[58] * 0x10000 + buffer[57] * 0x100 + buffer[56];
	LARGE_INTEGER li = { 0 };
	li.QuadPart = MftStart * BytesPerSecter *SecterPerCluster;

	SetFilePointer(hFile, li.LowPart, &li.HighPart, FILE_BEGIN);
	ReadFile(hFile, &buffer, 1024, &CB, NULL);
	UINT16 offset = 0;
	LARGE_INTEGER totaloffset = { 0 };

	if (buffer[0] == 'F' && buffer[1] == 'I' && buffer[2] == 'L' && buffer[3] == 'E' && buffer[4] == '0')
	{
		//第一属性偏移
		offset = buffer[0x14];
		//寻找80H
		while (offset < 1024)
		{
			if (buffer[offset] == 0x80)
			{
				Has80H = TRUE;
				break;
			}
			offset += buffer[offset + 0x4];
		}
	}
	else
	{
		CloseHandle(hFile);
		return FAILE;
	}

	//没找到80H
	if (!Has80H)
	{
		CloseHandle(hFile);
		return FAILE;
	}

	//获得属性长度
	UINT16 attr_len = buffer[offset + 0x4];
	UINT16 DataRun_offset = buffer[offset + 0x20];
	UINT16 DataRun_start = offset + DataRun_offset;
	UINT16 DataRun_len = offset + attr_len - DataRun_start;

	//存储DataRun
	std::vector<BYTE> vecDataRun;
	for (size_t i = 0; i < DataRun_len; i++)
	{
		vecDataRun.push_back(buffer[DataRun_start + i]);
	}

	//分析Datarun
	UINT64 LatestStartCluster = 0x00;
	std::vector<BYTE>::size_type index = 0;

	while (index < vecDataRun.size() && vecDataRun[index] != 0)
	{
		//得到长度(低字节)和偏移簇序数(高字节)
		UINT16 high = HIGH_BYTE(vecDataRun[index]);
		UINT16 low = LOW_BYTE(vecDataRun[index]);

		//判断是否到尾部
		if (high == 0 && low == 0)
		{
			break;
		}
		if ((index + low + high) >= vecDataRun.size())
		{
			break;
		}

		//获取MFT区域长度
		UINT16 temp = low;
		UINT64 len = 0;
		while (temp)
		{
			len <<= 8;
			len += vecDataRun[index + temp];
			temp--;
		}

		////////计算VCN////////
		temp = high;
		INT64 startclusteroffser = 0;
		while (temp)
		{
			startclusteroffser <<= 8;
			startclusteroffser += vecDataRun[index + low + temp];
			temp--;
		}
		if (len == 0 || startclusteroffser == 0)
		{
			break;
		}

		//HighbitIndex表示有几个字节有效
		size_t HighbitIndex = high;
		//有效最高位
		UINT16 Highbit = (startclusteroffser >> (8 * HighbitIndex - 1)) & 1;

		//若最高位为1则取补码
		if (Highbit)
		{
			//取反，+1
			std::vector<char> vecHex;
			UINT64 temp = ((~startclusteroffser) << (8 * (8 - HighbitIndex)));
			for (size_t i = 0; i < HighbitIndex; i++)
			{
				vecHex.push_back(((temp) >> (8 * (8 - HighbitIndex + i))) & 0xff);
			}
			startclusteroffser = 0;
			for (std::vector<char>::size_type n = 0; n < vecHex.size(); ++n)
			{
				startclusteroffser <<= 8;
				startclusteroffser += vecHex[vecHex.size() - 1 - n];
			}
			startclusteroffser += 1;
			startclusteroffser = -startclusteroffser;
		}

		//保存MFT区域信息
		if (startclusteroffser <= 0 && (UINT64)abs(startclusteroffser) >= LatestStartCluster)
		{
			break;
		}
		LatestStartCluster += startclusteroffser;
		MFT_INFO MFTtemp{ LatestStartCluster, len };
		vecMFTinfo.push_back(MFTtemp);

		//下一循环
		index = ((index + high + low + 1) >= vecDataRun.size() ? vecDataRun.size() : (index + high + low + 1));
	}
	CloseHandle(hFile);
	return SUCCESS;
}