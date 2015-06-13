#pragma once
#include "Define.h"

// CDatePickDlg dialog

class CDatePickDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CDatePickDlg)

public:
	CDatePickDlg(PDATEINFO pDateInfo, CWnd* pParent = NULL);   // standard constructor
	virtual ~CDatePickDlg();

// Dialog Data
	enum { IDD = IDD_DATEPICKER_DIALOG };

protected:	
	CString m_strDATESTART;
	CString m_strDATEEND;
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	virtual BOOL OnInitDialog();

	BOOL IsValidTime(CString time);
	BOOL TimeCompare(CString TimeStart, CString TimeEnd);
};
