#pragma once
#include "afxcmn.h"
#include "ReportCtrl.h"
#include "afxwin.h"

// CShredDlg 对话框

class CShredDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CShredDlg)

public:
	CShredDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CShredDlg();

// 对话框数据
	enum { IDD = IDD_DLL_SHRED_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	LRESULT	OnAddFiles(WPARAM wParam, LPARAM lParam);
	LRESULT	OnUpdateProcess(WPARAM wParam, LPARAM lParam);
	LRESULT	OnUpdateResult(WPARAM wParam, LPARAM lParam);
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonAddFiles();
	CReportCtrl m_wndList;
	virtual BOOL OnInitDialog();
	virtual void OnCancel();
	afx_msg void OnBnClickedButtonDelFile();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	int FindNextItermForErase(int start);
	afx_msg void OnBnClickedButtonShredFile();
	afx_msg void OnBnClickedButtonPause();
	CButton m_btnADD;
	CButton m_btnDELETE;
	CButton m_btnERASE;
	CButton m_btnPAUSE;
	CComboBox m_combox_nCount;
	CStatusBarCtrl m_statusbar;
	CProgressCtrlEx m_progress;
	afx_msg void OnBnClickedButtonWipe();
};
