
// WlanTestToolDlg.cpp : implementation file
//

#include "stdafx.h"
#include "WlanTestTool.h"
#include "WlanTestToolDlg.h"
#include "afxdialogex.h"
#include <Windows.h>
#include <wlanapi.h>
#include <Iphlpapi.h>  
#pragma comment (lib, "Iphlpapi")  
#pragma comment (lib, "ws2_32")  
#pragma comment(lib, "wlanapi.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CWlanTestToolDlg dialog

CString g_strSSID;
CString g_strChannel;
CString g_strSignal;
CString g_strIp;
CString g_strMac;
CString g_strSpeed;
CString g_strFreqencybond;


CWlanTestToolDlg::CWlanTestToolDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CWlanTestToolDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CWlanTestToolDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LABEL_CHANNEL, m_lb_channel);
	DDX_Control(pDX, IDC_LABEL_FREQENCY_BOND, m_lb_freqencybond);
	DDX_Control(pDX, IDC_LABEL_IP, m_lb_ip);
	DDX_Control(pDX, IDC_LABEL_MAC, m_lb_mac);
	DDX_Control(pDX, IDC_LABEL_SIGNAL, m_lb_signal);
	DDX_Control(pDX, IDC_LABEL_SPEED, m_lb_speed);
	DDX_Control(pDX, IDC_LABEL_SSID, m_lb_ssid);
}

BEGIN_MESSAGE_MAP(CWlanTestToolDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_GETINFO, &CWlanTestToolDlg::OnBnClickedButtonGetinfo)
	ON_BN_CLICKED(IDC_BUTTON_RELEASEIP, &CWlanTestToolDlg::OnBnClickedButtonReleaseip)
	ON_BN_CLICKED(IDC_BUTTON_RENEWIP, &CWlanTestToolDlg::OnBnClickedButtonRenewip)
	ON_BN_CLICKED(IDC_BUTTON_OUTPUTRESULT, &CWlanTestToolDlg::OnBnClickedButtonOutputresult)
END_MESSAGE_MAP()


// CWlanTestToolDlg message handlers

BOOL CWlanTestToolDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	return TRUE;  // return TRUE  unless you set the focus to a control
}


// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CWlanTestToolDlg::OnPaint()
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
HCURSOR CWlanTestToolDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CWlanTestToolDlg::OnBnClickedButtonGetinfo()
{
	// TODO: Add your control notification handler code here
	g_strSSID = L"";
	g_strChannel = L"";
	g_strSignal = L"";
	g_strIp = L"";
	g_strMac = L"";
	g_strSpeed = L"";
	g_strFreqencybond = L"";
	m_lb_signal.SetWindowText(L"");
	m_lb_ip.SetWindowText(L"");
	m_lb_mac.SetWindowText(L"");
	m_lb_speed.SetWindowText(L"");
	m_lb_ssid.SetWindowText(L"");
	m_lb_freqencybond.SetWindowText(L"");
	m_lb_channel.SetWindowText(L"");
	DWORD dwNegotiatedVersion = 0;
	HANDLE hClientHandle = INVALID_HANDLE_VALUE;
	PWLAN_INTERFACE_INFO_LIST pwlan_interface_info_list = NULL;
	PWLAN_CONNECTION_ATTRIBUTES pwlan_connection_attributes = NULL;
	PULONG pchannel_number = NULL;
	BOOL is5G = FALSE;
	WCHAR WLANGuid[39] = { 0 };
	DWORD Ds = 0;

	PIP_ADAPTER_INFO pIpAdaptTab = NULL;
	ULONG ulLen = 0;
	MIB_IFROW zSNMP;

	if (WlanOpenHandle(2, NULL, &dwNegotiatedVersion, &hClientHandle) != ERROR_SUCCESS)
	{
		MessageBox(L"初始化句柄失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	if (WlanEnumInterfaces(hClientHandle, NULL, &pwlan_interface_info_list) != ERROR_SUCCESS)
	{
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"初始化枚举失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	if (pwlan_interface_info_list->InterfaceInfo->isState != wlan_interface_state_connected)
	{
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"未找到已连接WLAN！",L"错误",  MB_ICONINFORMATION | MB_OK);
		return;
	}

	StringFromGUID2(pwlan_interface_info_list->InterfaceInfo->InterfaceGuid, (LPOLESTR)& WLANGuid,sizeof(WLANGuid) / sizeof(*WLANGuid));

	if (WlanQueryInterface(hClientHandle, &pwlan_interface_info_list->InterfaceInfo->InterfaceGuid, wlan_intf_opcode_channel_number, NULL, &Ds, (PVOID *)&pchannel_number, NULL) != ERROR_SUCCESS)
	{
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"查询WLAN信道失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	if (*pchannel_number > 0 && *pchannel_number < 15)
	{
		is5G = FALSE;
	}
	else
	{
		is5G = TRUE;
	}

	if (WlanQueryInterface(hClientHandle, &pwlan_interface_info_list->InterfaceInfo->InterfaceGuid, wlan_intf_opcode_current_connection, NULL, &Ds, (PVOID *)&pwlan_connection_attributes, NULL) != ERROR_SUCCESS)
	{
		WlanFreeMemory(pchannel_number);
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"查询WLAN基本信息失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	if (pwlan_connection_attributes->wlanAssociationAttributes.dot11Ssid.uSSIDLength == 0)
	{
		WlanFreeMemory(pwlan_connection_attributes);
		WlanFreeMemory(pchannel_number);
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"查询SSID名称失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return;
	}
	else
	{
		for (size_t k = 0; k < pwlan_connection_attributes->wlanAssociationAttributes.dot11Ssid.uSSIDLength; k++)
		{
			CString TEMP;
			TEMP.Format(L"%c", (int)pwlan_connection_attributes->wlanAssociationAttributes.dot11Ssid.ucSSID[k]);
			g_strSSID += TEMP;
		}
		m_lb_ssid.SetWindowText(g_strSSID);
	}
	if (is5G)
	{
		g_strFreqencybond = L"5G";
		m_lb_freqencybond.SetWindowText(L"5G");
	}
	else
	{
		g_strFreqencybond = L"2.4G";
		m_lb_freqencybond.SetWindowText(L"2.4G");
	}


	g_strChannel.Format(L"%u", *pchannel_number);
	m_lb_channel.SetWindowText(g_strChannel);

	g_strSignal.Format(L"%d%%", pwlan_connection_attributes->wlanAssociationAttributes.wlanSignalQuality);
	m_lb_signal.SetWindowText(g_strSignal);


	GetAdaptersInfo(pIpAdaptTab, &ulLen);
	if (ulLen == 0)
	{
		WlanFreeMemory(pwlan_connection_attributes);
		WlanFreeMemory(pchannel_number);
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"获取配置器失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	pIpAdaptTab = (PIP_ADAPTER_INFO)malloc(ulLen);
	if (pIpAdaptTab == NULL)
	{
		WlanFreeMemory(pwlan_connection_attributes);
		WlanFreeMemory(pchannel_number);
		WlanFreeMemory(pwlan_interface_info_list);
		WlanCloseHandle(hClientHandle, NULL);
		MessageBox(L"获取配置器失败！", L"错误", MB_ICONINFORMATION | MB_OK);
		return ;
	}

	GetAdaptersInfo(pIpAdaptTab, &ulLen);
	PIP_ADAPTER_INFO pTmp = pIpAdaptTab;

	char * pBUFFER = new char[260];
	WCHAR * pAdaptername = new WCHAR[260];
	WCHAR * pBUFF = new WCHAR[260];

	while (pTmp != NULL)
	{
		int len = WideCharToMultiByte(CP_ACP, 0, WLANGuid, wcslen(WLANGuid), NULL, 0, NULL, NULL);
		WideCharToMultiByte(CP_ACP, 0, WLANGuid, wcslen(WLANGuid), pBUFFER, len, NULL, NULL);
		pBUFFER[len] = '\0';

		int wlen = MultiByteToWideChar(CP_ACP, 0, pTmp->AdapterName, -1, NULL, 0);
		MultiByteToWideChar(CP_ACP, 0, pTmp->Description, -1, pAdaptername, 260);
		pAdaptername[wlen] = '\0';

		if ((!strcmp(pBUFFER, pTmp->AdapterName)) || (!wcscmp(pwlan_interface_info_list->InterfaceInfo->strInterfaceDescription, pAdaptername)))
		{
			int wcharlen = MultiByteToWideChar(CP_ACP, 0, pTmp->IpAddressList.IpAddress.String, -1, NULL, 0);
			MultiByteToWideChar(CP_ACP, 0, pTmp->IpAddressList.IpAddress.String, -1, pBUFF, 260);
			pBUFF[wcharlen] = '\0';
			g_strIp = pBUFF ;
			m_lb_ip.SetWindowText(g_strIp);

			g_strMac.Format(L"%02x-%02x-%02x-%02x-%02x-%02x", pTmp->Address[0],
				pTmp->Address[1],
				pTmp->Address[2],
				pTmp->Address[3],
				pTmp->Address[4],
				pTmp->Address[5]);
			m_lb_mac.SetWindowText(g_strMac);
	
			zSNMP.dwIndex = pTmp->Index;
			int iReturn = GetIfEntry(&zSNMP);
			if (iReturn != NO_ERROR)
			{
				WlanFreeMemory(pwlan_connection_attributes);
				WlanFreeMemory(pchannel_number);
				WlanFreeMemory(pwlan_interface_info_list);
				WlanCloseHandle(hClientHandle, NULL);
				free(pIpAdaptTab);
				if (pBUFF != NULL)
				{
					delete[] pBUFF;
					pBUFF = NULL;
				}
				if (pBUFFER != NULL)
				{
					delete[] pBUFFER;
					pBUFFER = NULL;
				}
				if (pAdaptername != NULL)
				{
					delete[] pAdaptername;
					pAdaptername = NULL;
				}
				MessageBox(L"获取协商速度失败！", L"错误", MB_ICONINFORMATION | MB_OK);
				return;
			}

			g_strSpeed.Format(L"%d Mbps", zSNMP.dwSpeed / (1000 * 1000));
			m_lb_speed.SetWindowText(g_strSpeed);
			WlanFreeMemory(pwlan_connection_attributes);
			WlanFreeMemory(pchannel_number);
			WlanFreeMemory(pwlan_interface_info_list);
			WlanCloseHandle(hClientHandle, NULL);
			free(pIpAdaptTab);
			if (pBUFF != NULL)
			{
				delete[] pBUFF;
				pBUFF = NULL;
			}
			if (pBUFFER != NULL)
			{
				delete[] pBUFFER;
				pBUFFER = NULL;
			}
			if (pAdaptername != NULL)
			{
				delete[] pAdaptername;
				pAdaptername = NULL;
			}
			return;
		}
		pTmp = pTmp->Next;
	}

	free(pIpAdaptTab);
	if (pBUFF != NULL)
	{
		delete[] pBUFF;
		pBUFF = NULL;
	}
	if (pBUFFER != NULL)
	{
		delete[] pBUFFER;
		pBUFFER = NULL;
	}
	if (pAdaptername != NULL)
	{
		delete[] pAdaptername;
		pAdaptername = NULL;
	}
	WlanFreeMemory(pwlan_connection_attributes);
	WlanFreeMemory(pchannel_number);
	WlanFreeMemory(pwlan_interface_info_list);
	WlanCloseHandle(hClientHandle, NULL);
	MessageBox(L"未能完全获取信息！", L"错误", MB_ICONINFORMATION | MB_OK);
}


void CWlanTestToolDlg::OnBnClickedButtonReleaseip()
{
	// TODO: Add your control notification handler code here
	system("ipconfig /release");
	OnBnClickedButtonGetinfo();
}


void CWlanTestToolDlg::OnBnClickedButtonRenewip()
{
	// TODO: Add your control notification handler code here
	system("ipconfig /renew");
	OnBnClickedButtonGetinfo();
}


void CWlanTestToolDlg::OnBnClickedButtonOutputresult()
{
	// TODO: Add your control notification handler code here
	if (MessageBox(L"是否要导出信息？", L"提示", MB_YESNO | MB_ICONINFORMATION) == IDNO)
	{
		return;
	}
	CFile output;
	CString strTemp;
	WCHAR pwcsCurPath[100] = { 0 };
	if (::GetModuleFileName(NULL, pwcsCurPath, 100) == 0)
	{
		MessageBox( L"输出文件失败，请管理员身份运行!", L"错误", MB_OK | MB_ICONERROR);
		return;
	}
	_tcsrchr(pwcsCurPath, _T('\\'))[1] = 0;
	strTemp.Format(L"%s", pwcsCurPath);
	strTemp += "Wlan_Test_Result.txt";
	
	if (output.Open(strTemp.GetBuffer(0), CFile::shareExclusive | CFile::modeWrite | CFile::modeCreate))
	 {
		 ULONGLONG dwFileLen = output.GetLength();
		 if (0 == dwFileLen) // 文件为空时写入UNICODE字节序标记
		 {
			 const unsigned char LeadBytes[] = { 0xff, 0xfe };
			 output.Write(LeadBytes, sizeof(LeadBytes));
		 }
		 CString buffer;
		 buffer = L"SSID:\t" + g_strSSID + L"\r\n频段:\t" + g_strFreqencybond + L"\r\n信道:\t" + g_strChannel + L"\r\nIP:\t" + g_strIp + L"\r\nMAC:\t" + g_strMac + L"\r\n信号:\t" + g_strSignal + L"\r\n协商速度:\t" + g_strSpeed
			 + L"\r\n\r\n请确保以上信息不为空且正确，谢谢！";
		 output.Write(buffer.GetBuffer(),buffer.GetLength()*sizeof(WCHAR));
		 output.Close();
	 }

	ShellExecute(NULL, L"open", strTemp, NULL, NULL, SW_SHOWNORMAL);
	return;
}
