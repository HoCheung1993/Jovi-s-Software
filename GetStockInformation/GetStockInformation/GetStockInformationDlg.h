
// GetStockInformationDlg.h : header file
//

#pragma once

// CGetStockInformationDlg dialog
class CGetStockInformationDlg : public CDialogEx
{
// Construction
public:
	CGetStockInformationDlg(CWnd* pParent = NULL);	// standard constructor
// Dialog Data
	enum { IDD = IDD_GETSTOCKINFORMATION_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;
	CString m_strDateStart;
	CString m_strDateEnd;
	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedBtnInitialize();
	afx_msg void OnBnClickedBtnChoosedate();
protected:
	afx_msg LRESULT OnDateChanged(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnBnClickedButtonUpdate();
};
