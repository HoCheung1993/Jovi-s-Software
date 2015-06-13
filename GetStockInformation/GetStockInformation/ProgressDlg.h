#pragma once
#include "afxcmn.h"
#include "StockInformationCtrl.h"


// CProgressDlg dialog

class CProgressDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CProgressDlg)

public:
	CProgressDlg(PDATEINFO pDateInfo, BOOL bIsInitialize = FALSE , CWnd* pParent = NULL);   // standard constructor
	virtual ~CProgressDlg();

// Dialog Data
	enum { IDD = IDD_PROGRESS_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
protected:
	afx_msg LRESULT OnUpdateDownloadinfo(WPARAM wParam, LPARAM lParam);
public:
	CProgressCtrl m_progress;
	afx_msg void OnBnClickedBtnPause();
	afx_msg void OnBnClickedBtnCancel();
	CStockInformationCtrl *m_pStockctrl;
	virtual void OnCancel();
protected:
	int m_nCount;
	int m_TotalCount;
	afx_msg LRESULT OnInitialDownloadinfo(WPARAM wParam, LPARAM lParam);
};