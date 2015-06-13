
// WlanTestToolDlg.h : header file
//

#pragma once
#include "afxwin.h"


// CWlanTestToolDlg dialog
class CWlanTestToolDlg : public CDialogEx
{
// Construction
public:
	CWlanTestToolDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_WLANTESTTOOL_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CStatic m_lb_channel;
	CStatic m_lb_freqencybond;
	CStatic m_lb_ip;
	CStatic m_lb_mac;
	CStatic m_lb_signal;
	CStatic m_lb_speed;
	CStatic m_lb_ssid;
	afx_msg void OnBnClickedButtonGetinfo();
	afx_msg void OnBnClickedButtonReleaseip();
	afx_msg void OnBnClickedButtonRenewip();
	afx_msg void OnBnClickedButtonOutputresult();
};
