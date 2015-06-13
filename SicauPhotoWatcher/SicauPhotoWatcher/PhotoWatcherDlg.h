#pragma once
#include "afxwin.h"


// PhotoWatcherDlg 对话框

class PhotoWatcherDlg : public CDialog
{
	DECLARE_DYNAMIC(PhotoWatcherDlg)

public:
	PhotoWatcherDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~PhotoWatcherDlg();

// 对话框数据
	enum { IDD = IDD_PhotoWatcherDlg };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	HICON m_hIcon;

	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnBnClickedBtncx();
	CString m_sXh;
	HRESULT PhotoWatcherDlg::ShowPic(CString xh);
	void GetUserId(BOOL LorN);
	afx_msg void OnClose();
	afx_msg void OnBnClickedblast();
	afx_msg void OnBnClickedbnext();
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	BOOL PhotoWatcherDlg::PreTranslateMessage(MSG* pMsg);
	void OnOK();
	CComboBox m_comboxNj;
	afx_msg void OnBnClickedBtnrandom();
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedcwaterwave();
	void DrowWaterText(CString text, CRect rect);
	CString GetInfo();
	CStatic m_stacXh;
	CStatic m_stacXm;
	void GetNameAndUnit();
	CStatic m_stacXy;
	afx_msg void OnPaint();
	afx_msg void OnBnClickedbsave();
//	CEdit m_edtResult;
};
