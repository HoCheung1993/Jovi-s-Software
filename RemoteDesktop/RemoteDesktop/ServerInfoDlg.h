#pragma once
#include "afxwin.h"
#include "Define.h"


// CServerInfoDlg dialog

class CServerInfoDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CServerInfoDlg)

public:
	CServerInfoDlg(DIALOGPASSDATA dlgdata,CWnd* pParent = NULL);   // standard constructor
	virtual ~CServerInfoDlg();

// Dialog Data
	enum { IDD = IDD_EDIT_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	DIALOGPASSDATA m_dialogpassdata;
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedBtnOk();
	CEdit m_tb_ip;
	CEdit m_tb_port;
	CEdit m_tb_user;
	CEdit m_tb_name;
	CEdit m_tb_password;
	CEdit m_tb_note;
	afx_msg void OnBnClickedButtonCancel();
	CButton m_btn_ok;
	virtual BOOL OnInitDialog();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
};
