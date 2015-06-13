#pragma once
#include "WipeCore.h"
#include "afxwin.h"
#include "afxcmn.h"


// CDiskWipeDlg dialog

class CDiskWipeDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CDiskWipeDlg)

public:
	CDiskWipeDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDiskWipeDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG_CLEANFREESPACE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	LRESULT OnUpdateWipeProcess(WPARAM wParam, LPARAM lParam);
	DECLARE_MESSAGE_MAP()
public:
	CDiskWipe m_Wiper;
	CComboBox m_combox_disk;
	CProgressCtrlEx m_progress_wipe;
	CButton m_btn_pause;
	CButton m_btn_start;
	virtual BOOL OnInitDialog();
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	CStatic m_lable_info;
	afx_msg void OnBnClickedButtonWipe();
	afx_msg void OnBnClickedButtonPause();
	afx_msg void OnClose();
};
