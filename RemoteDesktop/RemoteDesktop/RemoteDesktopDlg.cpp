
// RemoteDesktopDlg.cpp : implementation file
//

#include "stdafx.h"
#include "RemoteDesktop.h"
#include "RemoteDesktopDlg.h"
#include <string>
#include "CMsRdpClientAdvancedSettings6.h"
#include "CMsRdpClientSecuredSettings2.h"
#include <WS2tcpip.h>
#include "ServerInfoDlg.h"
#include <Wincrypt.h>
#pragma comment(lib, "Crypt32.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// CRemoteDesktopDlg dialog
HANDLE g_hWriteThread;

CRemoteDesktopDlg::CRemoteDesktopDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CRemoteDesktopDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CRemoteDesktopDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_MSTSCAX, m_rdp);
	DDX_Control(pDX, IDC_LIST_SERVERS, m_list_servers);
}

BEGIN_MESSAGE_MAP(CRemoteDesktopDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BTN_CONNECT, &CRemoteDesktopDlg::OnBnClickedBtnConnect)
	ON_BN_CLICKED(IDC_BUTTON_DISCONNECT, &CRemoteDesktopDlg::OnBnClickedButtonDisconnect)
	ON_WM_TIMER()
	ON_MESSAGE(WM_INITIAL_SERVERS, &CRemoteDesktopDlg::OnInitialServers)
	ON_BN_CLICKED(IDC_BTN_ADD_SERVER, &CRemoteDesktopDlg::OnBnClickedBtnAddServer)
	ON_BN_CLICKED(IDC_BTN_MSTSC, &CRemoteDesktopDlg::OnBnClickedBtnMstsc)
	ON_MESSAGE(WM_ADD_SERVER, &CRemoteDesktopDlg::OnAddServer)
	ON_NOTIFY(NM_CLICK, IDC_LIST_SERVERS, &CRemoteDesktopDlg::OnNMClickListServers)
	ON_BN_CLICKED(IDC_BTN_MODIFY_SERVER, &CRemoteDesktopDlg::OnBnClickedBtnModifyServer)
	ON_BN_CLICKED(IDC_BTN_CONNECT_DEL_SERVER, &CRemoteDesktopDlg::OnBnClickedBtnConnectDelServer)
	ON_MESSAGE(WM_MODIFY_SERVER, &CRemoteDesktopDlg::OnModifyServer)
	ON_NOTIFY(NM_DBLCLK, IDC_LIST_SERVERS, &CRemoteDesktopDlg::OnNMDblclkListServers)
	ON_WM_CLOSE()
	ON_COMMAND(ID_32773, &CRemoteDesktopDlg::OnOK)
	ON_COMMAND(ID_32772, &CRemoteDesktopDlg::BackupConfig)
	ON_COMMAND(ID_32776, &CRemoteDesktopDlg::ImportConfig)
	ON_COMMAND(ID_32777, &CRemoteDesktopDlg::About)
END_MESSAGE_MAP()


// CRemoteDesktopDlg message handlers

BOOL CRemoteDesktopDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	SetDlgSize(FALSE);
	((CButton *)GetDlgItem(IDC_RADIO_FASTMODE))->SetCheck(TRUE);
	((CComboBox *)GetDlgItem(IDC_COMBO_SCREENMODE))->AddString(_T("窗口"));
	((CComboBox *)GetDlgItem(IDC_COMBO_SCREENMODE))->AddString(_T("全屏"));
	((CComboBox *)GetDlgItem(IDC_COMBO_SCREENMODE))->SetCurSel(0);

	m_list_servers.SetColumnHeader(L"主机名, 150 ;主机地址,110;端口, 70;登录帐号, 120;网络状态,50");
	m_list_servers.SetGridLines(FALSE); 
	m_list_servers.SetEditable(FALSE); 
	m_list_servers.SortItems(0, TRUE); 
	CNetStatusHelper::NetInitial();

	//初始化信息线程
	AfxBeginThread((AFX_THREADPROC)InitThread, this);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.
void CRemoteDesktopDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CRemoteDesktopDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CRemoteDesktopDlg::OnBnClickedBtnConnect()
{
	// TODO: Add your control notification handler code here
	POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
	if (pos == NULL)
	{
		MessageBox(L"请选择一条列表信息！", L"提示", MB_OK | MB_ICONINFORMATION);
		return;
	}
	if (((CButton *)GetDlgItem(IDC_RADIO_FASTMODE))->GetCheck())
	{
		if (m_rdp.get_Connected() != 0)
		{
			if (MessageBox(L"已存在连接，是否断开？", L"警告", MB_YESNO | MB_ICONWARNING) == IDYES)
			{
				m_rdp.Disconnect();
			}
			else
			{
				return;
			}
		}
		else
		{
			FastMode_RemoteDesktop();
			SetDlgSize(TRUE);
		}
		return;
	}
	else
	{
		CString strScreenmode;
		GetDlgItemText(IDC_COMBO_SCREENMODE, strScreenmode);
		if (strScreenmode == L"全屏")
		{
			STDMode_RemoteDesktop(TRUE);
		}
		else
		{
			STDMode_RemoteDesktop(FALSE);
		}
		return;
	}
}

void CRemoteDesktopDlg::OnBnClickedButtonDisconnect()
{
	// TODO: Add your control notification handler code here
	if (m_rdp.get_Connected() != 0)
	{
		m_rdp.Disconnect();
	}
}

void CRemoteDesktopDlg::SetDlgSize(BOOL bFlag)
{
	CRect rect;
	GetWindowRect(rect);
	int nHeightMin = (rect.Height() < 300) ? rect.Height() : rect.Height() - 245;
	if (bFlag)
	{
		MoveWindow(rect.left, rect.top, rect.Width(), nHeightMin + 245);
	}
	else
	{
		MoveWindow(rect.left, rect.top, rect.Width(), nHeightMin);
	}
}

void CRemoteDesktopDlg::FastMode_RemoteDesktop()
{
	CMsRdpClientAdvancedSettings6 m_MsRdpClientAdvancedSettings(m_rdp.get_AdvancedSettings());
	CMsRdpClientSecuredSettings2 m_MsRdpClientSecuredSettings(m_rdp.get_SecuredSettings());
	POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
	int nIdex = m_list_servers.GetNextSelectedItem(pos);
	SERVER server;
	server.name = m_list_servers.GetItemText(nIdex, 0);
	server.ip = m_list_servers.GetItemText(nIdex, 1);
	server.port = m_list_servers.GetItemText(nIdex, 2);
	server.user = m_list_servers.GetItemText(nIdex, 3);
	INFO info;
	GetServer_Info(server, info);

	m_rdp.put_Server(server.ip);
	m_rdp.put_UserName(server.user);
	m_rdp.put_DesktopWidth(610);
	m_rdp.put_DesktopHeight(600);
	m_rdp.put_ColorDepth(32);
	m_MsRdpClientAdvancedSettings.put_EnableAutoReconnect(FALSE);
	m_MsRdpClientAdvancedSettings.put_Compress(1);
	m_MsRdpClientAdvancedSettings.put_BitmapPeristence(1);
	m_MsRdpClientAdvancedSettings.put_ClearTextPassword(info.password);
	m_MsRdpClientAdvancedSettings.put_singleConnectionTimeout(1);
	m_MsRdpClientAdvancedSettings.put_RDPPort(3389);
	m_MsRdpClientAdvancedSettings.put_singleConnectionTimeout(100);
	m_MsRdpClientSecuredSettings.put_KeyboardHookMode(1);
	m_MsRdpClientAdvancedSettings.put_EnableMouse(1);
	m_rdp.put_ConnectingText(_T("请稍后......"));
	m_rdp.Connect();
}BEGIN_EVENTSINK_MAP(CRemoteDesktopDlg, CDialogEx)
ON_EVENT(CRemoteDesktopDlg, IDC_MSTSCAX, 4, CRemoteDesktopDlg::OnDisconnectedMstscax, VTS_I4)
END_EVENTSINK_MAP()

void CRemoteDesktopDlg::OnDisconnectedMstscax(long discReason)
{
	// TODO: Add your message handler code here
	SetDlgSize(FALSE);
	switch (discReason)
	{
	case 0x3:
		MessageBox(_T("该用户在别处登录，您已经被强制下线！"), _T("提示"), MB_OK | MB_ICONWARNING);
		break;
	case 0x204:
		MessageBox( _T("无法连接到远程计算机，请确认IP地址无误！"), _T("提示"), MB_OK | MB_ICONWARNING);
		break;
	case 0x904:
		MessageBox(_T("与远程计算机断开通讯，请检查网络！"), _T("提示"), MB_OK | MB_ICONWARNING);
		break;
	default:
		MessageBox( _T("远程连接已断开！"), _T("提示"), MB_OK | MB_ICONWARNING);
		break;
	}
}

void CRemoteDesktopDlg::STDMode_RemoteDesktop(BOOL bFullScreen)
{
	CStdioFile Sfile;
	CString strPath;
	GetModuleFileName(NULL, strPath.GetBuffer(MAX_PATH), MAX_PATH);
	strPath.ReleaseBuffer();
	strPath = strPath.Mid(0, strPath.ReverseFind('\\'));
	strPath.Append(_T("\\MSTSCTEMP"));
	if (!_waccess_s(strPath.GetBuffer(), 0))
	{
		if (!SetFileAttributes(strPath.GetBuffer(), 0))
		{
			MessageBox(L"远程桌面初始化失败!", L"提示", MB_OK | MB_ICONINFORMATION);
			return;
		}
	}
	if (!Sfile.Open(strPath, CStdioFile::modeCreate | CStdioFile::modeReadWrite | CStdioFile::typeUnicode))
	{
		MessageBox(L"远程桌面初始化失败!", L"提示", MB_OK | MB_ICONINFORMATION);
		return;
	}
	else
	{
		POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
		int nIdex = m_list_servers.GetNextSelectedItem(pos);
		SERVER server;
		server.name = m_list_servers.GetItemText(nIdex, 0);
		server.ip = m_list_servers.GetItemText(nIdex, 1);
		server.port = m_list_servers.GetItemText(nIdex, 2);
		server.user = m_list_servers.GetItemText(nIdex, 3);
		INFO info;
		GetServer_Info(server, info);
		CString encodepassword; 
		if (!GetRDPPassword(info.password, encodepassword))
		{
			MessageBox(L"远程桌面初始化失败!", L"提示", MB_OK | MB_ICONINFORMATION);
			Sfile.Close();
			return;
		}
		Sfile.SeekToBegin();
		WORD sign = 0xfeff; // unicode文档 标志  
		Sfile.Write(&sign, 2);
		Sfile.WriteString(L"screen mode id:i:2\r\n");
		Sfile.WriteString(L"desktopwidth:i:1280\r\n");
		Sfile.WriteString(L"desktopheight:i:800\r\n");
		Sfile.WriteString(L"winposstr:s:0,3,0,0,800,600\r\n");
		Sfile.WriteString(L"session bpp:i:16\r\n");
		Sfile.WriteString(L"full address:s:" + server.ip + L":" + server.port + L"\r\n");
		Sfile.WriteString(L"compression:i:1\r\n");
		Sfile.WriteString(L"keyboardhook:i:2\r\n");
		Sfile.WriteString(L"audiomode:i:0\r\n");
		Sfile.WriteString(L"redirectdrives:i:0\r\n");
		Sfile.WriteString(L"redirectprinters:i:0\r\n");
		Sfile.WriteString(L"redirectcomports:i:0\r\n");
		Sfile.WriteString(L"redirectsmartcards:i:1\r\n");
		Sfile.WriteString(L"displayconnectionbar:i:1\r\n");
		Sfile.WriteString(L"autoreconnection enabled:i:0\r\n");	
		Sfile.WriteString(L"startfullscreen:i:0\r\n");
		Sfile.WriteString(L"alternate shell:s:\r\n");
		Sfile.WriteString(L"shell working directory:s:\r\n");
		Sfile.WriteString(L"username:s:" + server.user + L"\r\n");
		Sfile.WriteString(L"password 51:b:" + encodepassword + L"\r\n");
		Sfile.WriteString(L"disable wallpaper:i:1\r\n");
		Sfile.WriteString(L"disable full window drag:i:1\r\n");
		Sfile.WriteString(L"disable menu anims:i:1\r\n");
		Sfile.WriteString(L"disable themes:i:0\r\n");
		Sfile.WriteString(L"disable cursor setting:i:0\r\n");
		Sfile.WriteString(L"bitmapcachepersistenable:i:1\r\n");
		Sfile.WriteString(L"use multimon:i:0");
		Sfile.WriteString(L"audiocapturemode:i:0\r\n");
		Sfile.WriteString(L"videoplaybackmode:i:1\r\n");
		Sfile.WriteString(L"connection type:i:2\r\n");
		Sfile.WriteString(L"allow font smoothing:i:0\r\n");
		Sfile.WriteString(L"allow desktop composition:i:00\r\n");
		Sfile.WriteString(L"redirectclipboard:i:1\r\n");
		Sfile.WriteString(L"redirectposdevices:i:0\r\n");
		Sfile.WriteString(L"redirectdirectx:i:1\r\n");
		Sfile.WriteString(L"drivestoredirect:s:\r\n");
		Sfile.WriteString(L"authentication level:i:0\r\n");
		Sfile.WriteString(L"prompt for credentials:i:0\r\n");
		Sfile.WriteString(L"negotiate security layer:i:1\r\n");
		Sfile.WriteString(L"remoteapplicationmode:i:0\r\n");
		Sfile.WriteString(L"gatewayusagemethod:i:4\r\n");
		Sfile.WriteString(L"gatewaycredentialssource:i:4\r\n");
		Sfile.WriteString(L"gatewayprofileusagemethod:i:0\r\n");
		Sfile.WriteString(L"promptcredentialonce:i:1\r\n");
		Sfile.WriteString(L"use redirection server name:i:0");
		Sfile.Close();
		if (bFullScreen)
		{
			ShellExecute(NULL, L"open", L"mstsc.exe",strPath + L" /admin /f", L"", SW_SHOW);
		}
		else
		{
			ShellExecute(NULL, L"open", L"mstsc.exe",strPath + L" /admin /w:800 /h:600", L"", SW_SHOW);
		}
		return;
	}
}

E_RESULT CRemoteDesktopDlg::GetRDPPassword(CString &strPassword1, CString &strPassword2)
{
	DATA_BLOB DataIn;
	DATA_BLOB DataOut;
	// mstsc.exe中使用的是unicode,所以必须做宽字符转换
	BYTE *pbDataInput = (BYTE *)strPassword1.GetBuffer(0);
	DWORD cbDataInput = wcslen(strPassword1.GetBuffer(0))*sizeof(WCHAR);
	DataIn.pbData = pbDataInput;
	DataIn.cbData = cbDataInput;
	if (CryptProtectData(&DataIn, L"pwd", NULL, NULL, NULL, 0, &DataOut))
	{
		strPassword2 = L"";
		int nCount = 0;
		while (nCount < (int)DataOut.cbData)
		{
			WCHAR buffer[5];
			swprintf_s(buffer, 5, L"%02x", DataOut.pbData[nCount]);
			nCount++;
			strPassword2 += buffer;
		}
		return SUCCESS;
	}
	else
	{
		return FAIL;
	}
}

UINT CRemoteDesktopDlg::InitThread(LPVOID pParam)
{
	CRemoteDesktopDlg *dlg = (CRemoteDesktopDlg *)pParam;
	CIniCore inicore;
	CString temp , strPath;
	GetModuleFileName(NULL, strPath.GetBuffer(MAX_PATH), MAX_PATH);
	strPath.ReleaseBuffer();
	strPath = strPath.Mid(0, strPath.ReverseFind('\\'));
	strPath.Append(_T("\\Mstsc.ini"));
	inicore.ReadIni(strPath, temp ,dlg);
	return 1;
}
UINT CRemoteDesktopDlg::WriteThread(LPVOID pParam)
{
	CRemoteDesktopDlg *dlg = (CRemoteDesktopDlg *)pParam;
	CIniCore inicore;
	CString temp = L"", strPath ,strValue = L"" ,strBuffer = L"";
	GetModuleFileName(NULL, strPath.GetBuffer(MAX_PATH), MAX_PATH);
	strPath.ReleaseBuffer();
	strPath = strPath.Mid(0, strPath.ReverseFind('\\'));
	strPath.Append(_T("\\Mstsc.ini"));
	for (std::map<SERVER, INFO>::iterator it = dlg->m_mapServerInfo.begin(); it != dlg->m_mapServerInfo.end(); ++it)
	{
		temp = L"";
		temp += '{';
		temp += L"<_NAME_:\"";
		temp += it->first.name;
		temp += "\">_";
		temp += L"<_IP_:\"";
		temp += it->first.ip;
		temp += "\">_";
		temp += L"<_PORT_:\"";
		temp += it->first.port;
		temp += "\">_";
		temp += L"<_USER_:\"";
		temp += it->first.user;
		temp += "\">_";
		temp += L"<_PASSWORD_:\"";
		temp += it->second.password;
		temp += "\">_";
		temp += L"<_NOTE_:\"";
		temp += it->second.note;
		temp += "\">};";
		CIniHelper::Summarize(temp, strBuffer);
		strValue += strBuffer;
//		strValue += temp;
		strValue += L"\r\n";
	}
	inicore.WriteIni(strPath, strValue);
	return 1;
}

afx_msg LRESULT CRemoteDesktopDlg::OnInitialServers(WPARAM wParam, LPARAM lParam)
{
	PSERVERINFO pServerinfo = (PSERVERINFO)wParam;
	m_list_servers.InsertServerIterm(pServerinfo->server.name, pServerinfo->server.ip, pServerinfo->server.port, pServerinfo->server.user, L"...");

	SERVER server;
	server.ip = pServerinfo->server.ip;
	server.name = pServerinfo->server.name;
	server.port = pServerinfo->server.port;
	server.user = pServerinfo->server.user;
	INFO info;
	info.password = pServerinfo->info.password;
	info.note = pServerinfo->info.note;
	m_mapServerInfo.insert(std::make_pair(server, info));

	delete pServerinfo;
	pServerinfo = NULL;
	return 0;
}

E_RESULT CRemoteDesktopDlg::GetServer_Info(SERVER &server, INFO &info)
{
	info.note = L"";
	info.password = L"";
	if (m_mapServerInfo.size()== 0)
		return FALSE;
	std::map<SERVER, INFO>::iterator it;
	it = m_mapServerInfo.find(server);
	if (it == m_mapServerInfo.end())
	{
		return FALSE;
	}
	//找到元素
	info.password = it->second.password;
	info.note = it->second.note;
	return SUCCESS;
}

void CRemoteDesktopDlg::OnBnClickedBtnAddServer()
{
	// TODO: Add your control notification handler code here
	DIALOGPASSDATA dialogpassdata;
	SERVER server;
	INFO info;
	dialogpassdata.bModify = false;
	dialogpassdata.server = server;
	dialogpassdata.info = info;
	CServerInfoDlg dlg(dialogpassdata, this);
	dlg.DoModal();
}

void CRemoteDesktopDlg::OnBnClickedBtnMstsc()
{
	// TODO: Add your control notification handler code here
	if (ShellExecute(NULL, L"open", L"mstsc.exe", NULL, NULL, SW_SHOW) < (HANDLE)32)
	{
		MessageBox(L"无法打开mstsc.exe", L"提示", MB_OK | MB_ICONASTERISK);
	}
}

afx_msg LRESULT CRemoteDesktopDlg::OnAddServer(WPARAM wParam, LPARAM lParam)
{
	PSERVER pServer = (PSERVER)wParam;
	PINFO pInfo = (PINFO)lParam;
	if (m_mapServerInfo.count(*pServer))
	{
		MessageBox(L"已存在服务器无法重复添加！", L"提示", MB_OK | MB_ICONINFORMATION);
	}
	else
	{
		//添加到列表
		m_mapServerInfo.insert(std::make_pair(*pServer, *pInfo));
		//更新列表
		m_list_servers.InsertServerIterm(pServer->name, pServer->ip, pServer->port, pServer->user, L"...");

		PNETSTATUSDATA pNetstatusdata = new NETSTATUSDATA;
		pNetstatusdata->dlg = this;
		pNetstatusdata->ipAddress = pServer->ip;
		pNetstatusdata->port = pServer->port;
		CNetStatusHelper::CheckIPandPort(pNetstatusdata);
	}
	delete pServer;
	pServer = NULL;
	delete pInfo;
	pInfo = NULL;
	if (g_hWriteThread != INVALID_HANDLE_VALUE)
	{
		if (WaitForSingleObject(g_hWriteThread, 200) == WAIT_TIMEOUT)
		{
			::TerminateThread(g_hWriteThread, 0);
			g_hWriteThread = NULL;
		}
	}
	CWinThread * thread = AfxBeginThread((AFX_THREADPROC)WriteThread, this);
	if (thread != NULL)
	{
		g_hWriteThread = thread->m_hThread;
	}
	return 0;
}

void CRemoteDesktopDlg::OnNMClickListServers(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMITEMACTIVATE pNMItemActivate = reinterpret_cast<LPNMITEMACTIVATE>(pNMHDR);
	// TODO: Add your control notification handler code here
	NMLISTVIEW* pList = (NMLISTVIEW*)pNMHDR;
	int iItem = pList->iItem;
	if (-1 != iItem)
	{
		SERVER server;
		INFO info;
		server.name = m_list_servers.GetItemText(iItem, 0);
		server.ip = m_list_servers.GetItemText(iItem, 1);
		server.port = m_list_servers.GetItemText(iItem, 2);
		server.user = m_list_servers.GetItemText(iItem, 3);
		GetServer_Info(server, info);
		SetDlgItemText(IDC_EDIT_NOTE, info.note);
	}
	else
	{
		SetDlgItemText(IDC_EDIT_NOTE, L"");
	}
	*pResult = 0;
}

void CRemoteDesktopDlg::OnBnClickedBtnModifyServer()
{
	// TODO: Add your control notification handler code here
	POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
	if (pos == NULL)
	{
		MessageBox(L"请选择一个修改项！", L"提示", MB_OK | MB_ICONINFORMATION);
		return;
	}
	int nItem = m_list_servers.GetNextSelectedItem(pos);
	SERVER server;
	server.name = m_list_servers.GetItemText(nItem, 0);
	server.ip = m_list_servers.GetItemText(nItem, 1);
	server.port = m_list_servers.GetItemText(nItem, 2);
	server.user = m_list_servers.GetItemText(nItem, 3);
	INFO info;
	GetServer_Info(server, info);
	DIALOGPASSDATA dialogpassdata;
	dialogpassdata.bModify = true;
	dialogpassdata.server = server;
	dialogpassdata.info = info;
	CServerInfoDlg dlg(dialogpassdata, this);
	dlg.DoModal();
}

void CRemoteDesktopDlg::OnBnClickedBtnConnectDelServer()
{
	// TODO: Add your control notification handler code here
	POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
	if (pos == NULL)
	{
		MessageBox(L"请选择一个删除项！", L"提示", MB_OK | MB_ICONINFORMATION);
		return;
	}
	int nItem = m_list_servers.GetNextSelectedItem(pos);
	SERVER server;
	server.name = m_list_servers.GetItemText(nItem, 0);
	server.ip = m_list_servers.GetItemText(nItem, 1);
	server.port = m_list_servers.GetItemText(nItem, 2);
	server.user = m_list_servers.GetItemText(nItem, 3);
	if (IDNO == MessageBox(L"确定要删除吗？", L"提示", MB_YESNO | MB_ICONINFORMATION))
	{
		return;
	}
	//在map中删除记录
	m_mapServerInfo.erase(server);
	//clistctrl移除元素
	m_list_servers.DeleteItem(nItem);
	SetDlgItemText(IDC_EDIT_NOTE, L"");
	CWinThread * thread = AfxBeginThread((AFX_THREADPROC)WriteThread, this);
	if (thread != NULL)
	{
		g_hWriteThread = thread->m_hThread;
	}
	return ;
}

afx_msg LRESULT CRemoteDesktopDlg::OnModifyServer(WPARAM wParam, LPARAM lParam)
{
	POSITION pos = m_list_servers.GetFirstSelectedItemPosition();
	PSERVER pServer = (PSERVER)wParam;
	PINFO pInfo = (PINFO)lParam;
	if (pos == NULL)
	{
		MessageBox(L"未选中修改项！", L"提示", MB_OK | MB_ICONINFORMATION);
	}
	else
	{
		int nItem = m_list_servers.GetNextSelectedItem(pos);
		SERVER server;
		INFO info;
		server.name = m_list_servers.GetItemText(nItem, 0);
		server.ip = m_list_servers.GetItemText(nItem, 1);
		server.port = m_list_servers.GetItemText(nItem, 2);
		server.user = m_list_servers.GetItemText(nItem, 3);
		GetServer_Info(server, info);
		m_mapServerInfo.erase(server);

		if (m_mapServerInfo.count(*pServer))
		{
			m_mapServerInfo.insert(std::make_pair(server, info));
			MessageBox(L"已存在服务器无法进行修改！", L"提示", MB_OK | MB_ICONINFORMATION);
		}
		else
		{
			//更新到列表
			m_list_servers.SetItemImage(nItem, 0, 0);
			m_list_servers.SetItemText(nItem, 0, pServer->name);
			m_list_servers.SetItemText(nItem, 1, pServer->ip);
			m_list_servers.SetItemText(nItem, 2, pServer->port);
			m_list_servers.SetItemText(nItem, 3, pServer->user);
			m_list_servers.SetItemText(nItem, 4, L"...");
			m_mapServerInfo.insert(std::make_pair(*pServer, *pInfo));

			PNETSTATUSDATA pNetstatusdata = new NETSTATUSDATA;
			pNetstatusdata->dlg = this;
			pNetstatusdata->ipAddress = pServer->ip;
			pNetstatusdata->port = pServer->port;
			CNetStatusHelper::CheckIPandPort(pNetstatusdata);
		}
	}
	delete pServer;
	pServer = NULL;
	delete pInfo;
	pInfo = NULL;
	if (g_hWriteThread != INVALID_HANDLE_VALUE)
	{
		if (WaitForSingleObject(g_hWriteThread, 200) == WAIT_TIMEOUT)
		{
			::TerminateThread(g_hWriteThread, 0);
			g_hWriteThread = INVALID_HANDLE_VALUE;
		}
	}
	CWinThread * thread = AfxBeginThread((AFX_THREADPROC)WriteThread, this);
	if (thread != NULL)
	{
		g_hWriteThread = thread->m_hThread;
	}
	return 0;
}

void CRemoteDesktopDlg::OnNMDblclkListServers(NMHDR *pNMHDR, LRESULT *pResult)
{
	LPNMITEMACTIVATE pNMItemActivate = reinterpret_cast<LPNMITEMACTIVATE>(pNMHDR);
	// TODO: Add your control notification handler code here
	if (m_mapServerInfo.size() == 0)
	{
		return;
	}
	OnBnClickedBtnModifyServer();
	*pResult = 0;
}

BOOL CRemoteDesktopDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_RETURN)
	{
		OnBnClickedBtnModifyServer();
		return true;
	}
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_ESCAPE)
	{
		return true;
	}
	return CDialogEx::PreTranslateMessage(pMsg);
}

void CRemoteDesktopDlg::OnClose()
{
	// TODO: Add your message handler code here and/or call default
	CString strPath;
	GetModuleFileName(NULL, strPath.GetBuffer(MAX_PATH), MAX_PATH);
	strPath.ReleaseBuffer();
	strPath = strPath.Mid(0, strPath.ReverseFind('\\'));
	strPath.Append(_T("\\MSTSCTEMP"));
	DeleteFile(strPath);
	if (g_hWriteThread != INVALID_HANDLE_VALUE)
	{
		if (WaitForSingleObject(g_hWriteThread, 100) == WAIT_TIMEOUT)
		{
			MessageBox(L"当前系统正在保存配置信息，稍后将自动退出！", L"提示", MB_ICONQUESTION | MB_OK);
			WaitForSingleObject(g_hWriteThread, INFINITE);
		}
	}
	CDialogEx::OnClose();
}

void CRemoteDesktopDlg::OnOK()
{
	// TODO: Add your specialized code here and/or call the base class
	CString strPath;
	GetModuleFileName(NULL, strPath.GetBuffer(MAX_PATH), MAX_PATH);
	strPath.ReleaseBuffer();
	strPath = strPath.Mid(0, strPath.ReverseFind('\\'));
	strPath.Append(_T("\\MSTSCTEMP"));
	DeleteFile(strPath);
	if (g_hWriteThread != INVALID_HANDLE_VALUE)
	{
		if (WaitForSingleObject(g_hWriteThread, 200) == WAIT_TIMEOUT)
		{
			MessageBox(L"当前系统正在保存配置信息，稍后将自动退出！", L"提示", MB_ICONQUESTION | MB_OK);
			WaitForSingleObject(g_hWriteThread, INFINITE);
		}
	}
	CDialogEx::OnOK();
}

void CRemoteDesktopDlg::BackupConfig()
{
	BOOL isOpen = FALSE;     
	CString strFilePathName = L"";         
	CString filter = L"配置文件 (*.ini)|*.ini||";
	CFileDialog openFileDlg(isOpen, L".ini", NULL, OFN_HIDEREADONLY | OFN_READONLY, filter, NULL);
	if (openFileDlg.DoModal() == IDOK)
	{
		strFilePathName = openFileDlg.GetPathName();
		CString strEXEPath;
		GetModuleFileName(NULL, strEXEPath.GetBuffer(MAX_PATH), MAX_PATH);
		strEXEPath.ReleaseBuffer();
		strEXEPath = strEXEPath.Mid(0, strEXEPath.ReverseFind('\\'));
		strEXEPath.Append(_T("\\Mstsc.ini"));
		if (CopyFile(strEXEPath, strFilePathName, FALSE))
		{
			MessageBox(L"备份成功！", L"提示", MB_OK | MB_ICONINFORMATION);
		}
	}
}

void CRemoteDesktopDlg::ImportConfig()
{
	BOOL isOpen = TRUE;     
	CString strFilePathName = L"";
	CString filter = L"配置文件 (*.ini)|*.ini||";
	CFileDialog openFileDlg(isOpen, NULL, NULL, OFN_HIDEREADONLY | OFN_READONLY, filter, NULL);
	INT_PTR result = openFileDlg.DoModal();
	if (result == IDOK)
	{
		strFilePathName = openFileDlg.GetPathName();
		CString strEXEPath;
		GetModuleFileName(NULL, strEXEPath.GetBuffer(MAX_PATH), MAX_PATH);
		strEXEPath.ReleaseBuffer();
		strEXEPath = strEXEPath.Mid(0, strEXEPath.ReverseFind('\\'));
		strEXEPath.Append(_T("\\Mstsc.ini"));
		if(CopyFile(strFilePathName, strEXEPath, FALSE))
		{
			MessageBox(L"导入成功！\n即将加载新配置文件！", L"提示", MB_OK | MB_ICONINFORMATION);
			AfxBeginThread((AFX_THREADPROC)InitThread, this);
		}
	}
}

void CRemoteDesktopDlg::About()
{
	MessageBox(L"Designed by Jovi. 2015 @Sicau", L"关于", MB_OK | MB_ICONINFORMATION);
}

CIniCore::CIniCore() :m_OpenFlags((CStdioFile::modeRead |CStdioFile::typeBinary))
{

}

E_RESULT CIniCore::ReadIni(CString strFilepath, CString &strResult, CRemoteDesktopDlg *dlg)
{
	CString strPath = strFilepath;
	CStdioFile Sfile;
	if (!Sfile.Open(strPath,m_OpenFlags, NULL))
	{
		MessageBox(NULL,L"未找到Mstsc.ini配置文件!", L"提示", MB_OK | MB_ICONINFORMATION);
		return FAIL;
	}
	else
	{
		Sfile.SeekToBegin();
		while (Sfile.ReadString(strResult))
		{
			CIniHelper::Analyse(strResult, dlg);
		}
		Sfile.Close();
	}
	return SUCCESS;
}

E_RESULT CIniCore::WriteIni(CString strFilepath, CString &strValue)
{
	CString strPath = strFilepath;
	if (!_waccess_s(strPath.GetBuffer(), 0))
	{
		if (!SetFileAttributes(strPath.GetBuffer(), 0))
		{
			MessageBox(NULL, L"Mstsc.ini配置文件保存失败!", L"提示", MB_OK | MB_ICONINFORMATION);
			return FAIL;
		}
	}
	CStdioFile Sfile;
	Sfile.Open(strPath, CStdioFile::modeReadWrite | CStdioFile::shareDenyWrite | CStdioFile::modeCreate |CStdioFile ::typeUnicode, NULL);
	WORD sign = 0xfeff; // unicode文档 标志  
	Sfile.Write(&sign, 2);
	Sfile.WriteString(strValue);
	Sfile.Flush();
	Sfile.Close();
	return SUCCESS;
}

E_RESULT CIniCore::SetOpenFlags(int flags)
{
	m_OpenFlags = flags;
	return SUCCESS;
}

E_RESULT CIniHelper::Analyse(CString &strSource, CRemoteDesktopDlg *pDlg)
{
	CString strIni;
	CString strPerRecord;
	CString name = L"";
	CString ip = L"";
	CString port = L"";
	CString user = L"";
	CString password = L"";
	CString note = L"";

	int nStart = -1;
	int nEnd = -1;
	int nPos = -1;

	//出去unicode的开头
	if (strSource.GetAt(0) == 0xFEFF)
	{		
		strSource = strSource.Mid(1);
	}


	if (!Decrypt(strSource, strIni, 1314))
	{
		return FAIL;
	}
//	strIni = strSource;

	nStart = strIni.Find('{');
	if (nStart == -1 )
	{
		return FAIL;
	}
	nEnd = strIni.Find('}', nStart);
	if (nEnd == -1)
	{
		return FAIL;
	}
	strPerRecord = strIni.Mid(nStart, nEnd - nStart);

	if ((strPerRecord.Find(L"_NAME_:") != -1) && (strPerRecord.Find(L"_IP_:") != -1) && (strPerRecord.Find(L"_PORT_:") != -1) && (strPerRecord.Find(L"_USER_:") != -1) && (strPerRecord.Find(L"_PASSWORD_:") != -1 && (strPerRecord.Find(L"_NOTE_:") != -1)))
	{
		name = L"";
		ip = L"";
		port = L"";
		user = L"";
		password = L"";
		note = L"";
		nPos = nEnd;

		//name
		nStart = strPerRecord.Find(L"_NAME_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		name = strPerRecord.Mid(nStart, nEnd - nStart);
		//ip
		nStart = strPerRecord.Find(L"_IP_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		ip = strPerRecord.Mid(nStart, nEnd - nStart);
		//PORT
		nStart = strPerRecord.Find(L"_PORT_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		port = strPerRecord.Mid(nStart, nEnd - nStart);
		//USER
		nStart = strPerRecord.Find(L"_USER_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		user = strPerRecord.Mid(nStart, nEnd - nStart);
		//PASSWORD
		nStart = strPerRecord.Find(L"_PASSWORD_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		password = strPerRecord.Mid(nStart, nEnd - nStart);
		//NOTE
		nStart = strPerRecord.Find(L"_NOTE_:") + 1;
		nStart = strPerRecord.Find('"', nStart) + 1;
		nEnd = strPerRecord.Find('"', nStart);
		note = strPerRecord.Mid(nStart, nEnd - nStart);

		//更新列表
		PSERVERINFO pServerinfo = new SERVERSINFO;
		pServerinfo->server.ip = ip;
		pServerinfo->server.name = name;
		pServerinfo->info.note = note;
		pServerinfo->info.password = password;
		pServerinfo->server.port = port;
		pServerinfo->server.user = user;
		SendMessage(pDlg->m_hWnd, WM_INITIAL_SERVERS, (WPARAM)pServerinfo, NULL);
	
		PNETSTATUSDATA pNetstatusdata = new NETSTATUSDATA;
		pNetstatusdata->dlg = pDlg;
		pNetstatusdata->ipAddress = ip;
		pNetstatusdata->port = port;
		CNetStatusHelper::CheckIPandPort(pNetstatusdata);
	}
	return SUCCESS;
}

E_RESULT CIniHelper::Summarize(CString &strSource, CString &strResult)
{
	Encrypt(strSource, strResult, 1314);
	return SUCCESS;
}

E_RESULT CIniHelper::Encrypt(CString &strSource, CString &strResult, DWORD Key) // 加密函数
{
	int C1 = 52845;
	int C2 = 22719;
	CString Result, str;
	int i, j;
	Result = strSource; // 初始化结果字符串
	for (i = 0; i < strSource.GetLength(); i++) // 依次对字符串中各字符进行操作
	{
		Result.SetAt(i, (WCHAR)(Result.GetAt(i) ^ (Key >> 8))); // 将密钥移位后与字符异或
		Key = ((DWORD)Result.GetAt(i) + Key)*C1 + C2; // 产生下一个密钥
	}
	strSource = Result; // 保存结果
	Result.Empty(); // 清除结果
	for (i = 0; i < strSource.GetLength(); i++) // 对加密结果进行转换
	{
		j = (WCHAR)strSource.GetAt(i); // 提取字符
		// 将字符转换为两个字母保存
		str = "12"; // 设置str长度为2
		str.SetAt(0, 256 + j / 26);//这里将65改大点的数例如256，密文就会变乱码，效果更好，相应的，解密处要改为相同的数
		str.SetAt(1, 256 + j % 26);
		Result += str;
	}
	strResult = Result;
	return SUCCESS;
}

E_RESULT CIniHelper::Decrypt(CString &strSource, CString &strResult, DWORD Key) // 解密函数
{
	CString Result, str;
	int i, j;
	int C1 = 52845;
	int C2 = 22719;
	Result.Empty(); // 清除结果
	for (i = 0; i < strSource.GetLength() / 2; i++) // 将字符串两个字母一组进行处理
	{
		j = (((DWORD)strSource.GetAt(2 * i) - 256) * 26);//相应的，解密处要改为相同的数

		j += (DWORD)strSource.GetAt(2 * i + 1) - 256;
		str = "1"; // 设置str长度为1
		str.SetAt(0, j);
		Result += str; // 追加字符，还原字符串
	}
	strSource = Result; // 保存中间结果
	for (i = 0; i < strSource.GetLength(); i++) // 依次对字符串中各字符进行操作
	{
		Result.SetAt(i, (WCHAR)(strSource.GetAt(i) ^ (DWORD)(Key >> 8))); // 将密钥移位后与字符异或
		Key = ((DWORD)strSource.GetAt(i) + Key)*C1 + C2; // 产生下一个密钥
	}
	strResult = Result;
	return SUCCESS;
}

E_RESULT CNetStatusHelper::CheckIPandPort(PNETSTATUSDATA pNetstatusdata)
{
	AfxBeginThread((AFX_THREADPROC)Checkthread, pNetstatusdata);
	return SUCCESS;
}

E_RESULT CNetStatusHelper::NetInitial()
{
	WSADATA wsaData;
	if (WSAStartup(0x0002, &wsaData))
	{
		AfxMessageBox( L"初始化TCP/IP错误!");
		return FAIL;
	}
	if (wsaData.wVersion != 0x0002)
	{
		AfxMessageBox(L"Winsock版本不正确!");
		WSACleanup();
		return FAIL;
	}
	return SUCCESS;
}

UINT CNetStatusHelper::Checkthread(LPVOID pParam)
{
	PNETSTATUSDATA data = (PNETSTATUSDATA)pParam;
	CString ip = data->ipAddress;
	CString ipport = data->port;
	CRemoteDesktopDlg * dlg = data->dlg;

	struct sockaddr_in sin;
	SOCKET sd;
	// Create the local socket
	if ((sd = socket(AF_INET, SOCK_STREAM, IPPROTO_IP)) == INVALID_SOCKET)
	{
		//初始化错误
		for (int i = 0; i < dlg->m_list_servers.GetItemCount(); i++)
		{
			if (dlg->m_list_servers.GetItemText(i, 1) == ip && dlg->m_list_servers.GetItemText(i, 2) == ipport)
			{
				dlg->m_list_servers.SetItemImage(i, 0, 0);
				dlg->m_list_servers.SetItemText(i, 4, L"未知");
			}
		}

		delete data;
		data = NULL;
		return 1;
	}

	int IpPort = _ttoi(ipport);

	int nLength = ip.GetLength();
	int nBytes = WideCharToMultiByte(CP_ACP, 0, ip, nLength, NULL, 0, NULL, NULL);
	char* IpAddr = new char[nBytes + 1];
	memset(IpAddr, 0, nLength + 1);
	WideCharToMultiByte(CP_OEMCP, 0, ip, nLength, IpAddr, nBytes, NULL, NULL);
	IpAddr[nBytes] = 0;

	// Connect to the victim IP Address
	sin.sin_family = AF_INET;
	inet_pton(AF_INET, IpAddr, (PVOID)&sin.sin_addr.s_addr);
	sin.sin_port = htons((short)IpPort);
	if (connect(sd, (struct sockaddr *)&sin, sizeof(sin)) == SOCKET_ERROR)
	{
		closesocket(sd);
//		AfxMessageBox(L"远程IP未开启端口！");
		for (int i = 0; i < dlg->m_list_servers.GetItemCount(); i++)
		{
			if (dlg->m_list_servers.GetItemText(i, 1) == ip && dlg->m_list_servers.GetItemText(i, 2) == ipport)
			{
				dlg->m_list_servers.SetItemImage(i, 0, 0);
				dlg->m_list_servers.SetItemText(i, 4, L"离线");
			}
		}
		delete data;
		data = NULL;
		delete[] IpAddr;
		return 1;
	}
	else
	{
//		AfxMessageBox(L"端口已打开！");
		for (int i = 0; i < dlg->m_list_servers.GetItemCount(); i++)
		{
			if (dlg->m_list_servers.GetItemText(i, 1) == ip && dlg->m_list_servers.GetItemText(i, 2) == ipport)
			{
				dlg->m_list_servers.SetItemImage(i, 0, 1);
				dlg->m_list_servers.SetItemText(i, 4, L"在线");
			}
		}
	}
	closesocket(sd);
	delete data;
	data = NULL;
	delete[] IpAddr;
	return 1;
}

