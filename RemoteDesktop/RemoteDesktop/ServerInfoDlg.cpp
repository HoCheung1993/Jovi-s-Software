// ServerInfoDlg.cpp : implementation file
//

#include "stdafx.h"
#include "RemoteDesktop.h"
#include "ServerInfoDlg.h"
#include "afxdialogex.h"

// CServerInfoDlg dialog

IMPLEMENT_DYNAMIC(CServerInfoDlg, CDialogEx)

CServerInfoDlg::CServerInfoDlg(DIALOGPASSDATA dlgdata, CWnd* pParent /*=NULL*/)
	: CDialogEx(CServerInfoDlg::IDD, pParent)
{
	m_dialogpassdata = dlgdata;
}

CServerInfoDlg::~CServerInfoDlg()
{
}

void CServerInfoDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_EDIT_IP, m_tb_ip);
	DDX_Control(pDX, IDC_EDIT_PORT, m_tb_port);
	DDX_Control(pDX, IDC_EDIT_USER, m_tb_user);
	DDX_Control(pDX, IDC_EDIT_NAME, m_tb_name);
	DDX_Control(pDX, IDC_EDIT_PASSWORD, m_tb_password);
	DDX_Control(pDX, IDC_EDIT_NOTE, m_tb_note);
	DDX_Control(pDX, IDC_BTN_OK, m_btn_ok);
}


BEGIN_MESSAGE_MAP(CServerInfoDlg, CDialogEx)
	ON_BN_CLICKED(IDC_BTN_OK, &CServerInfoDlg::OnBnClickedBtnOk)
	ON_BN_CLICKED(IDC_BUTTON_CANCEL, &CServerInfoDlg::OnBnClickedButtonCancel)
END_MESSAGE_MAP()


// CServerInfoDlg message handlers


void CServerInfoDlg::OnBnClickedBtnOk()
{ 
	// TODO: Add your control notification handler code here
	if (m_tb_name.GetWindowTextLength() == 0 || m_tb_ip.GetWindowTextLength() == 0 || m_tb_port.GetWindowTextLength() == 0 || m_tb_user.GetWindowTextLength() == 0 )
	{
		MessageBox(L"请提交完整信息！", L"提示", MB_OK | MB_ICONINFORMATION);
		return;
	}
	if (m_dialogpassdata.bModify)
	{
		PSERVER pServer = new SERVER;
		PINFO pInfo = new INFO;
		m_tb_name.GetWindowText(pServer->name);
		m_tb_ip.GetWindowText(pServer->ip);
		m_tb_port.GetWindowText(pServer->port);
		m_tb_user.GetWindowText(pServer->user);
		m_tb_password.GetWindowText(pInfo->password);
		m_tb_note.GetWindowText(pInfo->note);

		::PostMessage(GetParent()->m_hWnd, WM_MODIFY_SERVER, (WPARAM)pServer, (LPARAM)pInfo);
	}
	else
	{
		PSERVER pServer = new SERVER;
		PINFO pInfo = new INFO;
		m_tb_name.GetWindowText(pServer->name);
		m_tb_ip.GetWindowText(pServer->ip);
		m_tb_port.GetWindowText(pServer->port);
		m_tb_user.GetWindowText(pServer->user);
		m_tb_password.GetWindowText(pInfo->password);
		m_tb_note.GetWindowText(pInfo->note);

		::PostMessage(GetParent()->m_hWnd, WM_ADD_SERVER, (WPARAM)pServer, (LPARAM)pInfo);
	}
	this->OnOK();
}


void CServerInfoDlg::OnBnClickedButtonCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}


BOOL CServerInfoDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  Add extra initialization here
	if (m_dialogpassdata.bModify)
	{
		m_tb_name.SetWindowText(m_dialogpassdata.server.name);
		m_tb_ip.SetWindowText(m_dialogpassdata.server.ip);
		m_tb_port.SetWindowText(m_dialogpassdata.server.port);
		m_tb_user.SetWindowText(m_dialogpassdata.server.user);
		m_tb_password.SetWindowText(m_dialogpassdata.info.password);
		m_tb_note.SetWindowText(m_dialogpassdata.info.note);
		SetWindowText(L"修改服务器");
		m_btn_ok.SetWindowText(L"修改");
	}
	else
	{
		m_tb_name.SetWindowText(L"");
		m_tb_ip.SetWindowText(L"");
		m_tb_port.SetWindowText(L"");
		m_tb_user.SetWindowText(L"");
		m_tb_password.SetWindowText(L"");
		m_tb_note.SetWindowText(L"");
		SetWindowText(L"添加服务器");
		m_btn_ok.SetWindowText(L"添加");
	}

	m_tb_name.SetLimitText(10);
	m_tb_ip.SetLimitText(40);
	m_tb_port.SetLimitText(5);
	m_tb_user.SetLimitText(20);
	m_tb_password.SetLimitText(15);
	m_tb_note.SetLimitText(15);
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}


BOOL CServerInfoDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_RETURN)
	{
		OnBnClickedBtnOk();
	}
	return CDialogEx::PreTranslateMessage(pMsg);
}
