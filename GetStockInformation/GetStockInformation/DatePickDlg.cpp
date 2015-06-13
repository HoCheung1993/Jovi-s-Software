// DatePickDlg.cpp : implementation file
//

#include "stdafx.h"
#include "GetStockInformation.h"
#include "DatePickDlg.h"
#include "afxdialogex.h"


// CDatePickDlg dialog

IMPLEMENT_DYNAMIC(CDatePickDlg, CDialogEx)

CDatePickDlg::CDatePickDlg(PDATEINFO pDateInfo ,CWnd* pParent /*=NULL*/)
	: CDialogEx(CDatePickDlg::IDD, pParent)
{
	if (pDateInfo != NULL)
	{
		m_strDATESTART = pDateInfo->strDateStart;
		m_strDATEEND = pDateInfo->strDateEnd;
		delete pDateInfo;
		pDateInfo = NULL;
	}
}

CDatePickDlg::~CDatePickDlg()
{
}

void CDatePickDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CDatePickDlg, CDialogEx)
	ON_BN_CLICKED(IDOK, &CDatePickDlg::OnBnClickedOk)
END_MESSAGE_MAP()


// CDatePickDlg message handlers


void CDatePickDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here

	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_START))->GetWindowText(m_strDATESTART);
	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_END))->GetWindowText(m_strDATEEND);
	if (!TimeCompare(m_strDATESTART,m_strDATEEND))
	{
		::MessageBox(NULL, L"结束日期应大于起始日期！", L"错误", MB_OK | MB_ICONINFORMATION);
		return;
	}
	PDATEINFO pDATEINFO = new DATEINFO{m_strDATESTART,m_strDATEEND};
	::PostMessage(GetParent()->m_hWnd, WM_DATE_CHANGED, (WPARAM)pDATEINFO, NULL);
	CDialogEx::OnOK();
}


BOOL CDatePickDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// TODO:  Add extra initialization here
	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_START))->SetFormat(L"yyyy-MM-dd");
	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_END))->SetFormat(L"yyyy-MM-dd");
	CTime tCurrentTime = CTime::GetCurrentTime();
	CTimeSpan timespanOneMonth(0, 0, 0, 0); //推后？天：(？, 0, 0, 0)
	CTime tEndTime = tCurrentTime + timespanOneMonth;
	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_START))->SetRange(NULL, &tEndTime);
	((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_END))->SetRange(NULL, &tEndTime);

	if (IsValidTime(m_strDATESTART) && IsValidTime(m_strDATEEND))
	{
		int a, b, c;
		swscanf_s(m_strDATESTART.GetBuffer(m_strDATESTART.GetLength()), L"%d-%d-%d", &a, &b, &c);
		CTime  time1(a, b, c, 0, 0, 0);
		swscanf_s(m_strDATEEND.GetBuffer(m_strDATEEND.GetLength()), L"%d-%d-%d", &a, &b, &c);
		CTime  time2(a, b, c, 0, 0, 0);
		((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_START))->SetTime(&time1);
		((CDateTimeCtrl *)GetDlgItem(IDC_DATETIMEPICKER_END))->SetTime(&time2);
	}	
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

BOOL CDatePickDlg::TimeCompare(CString strTimeStart, CString strTimeEnd)
{
	int a, b, c;
	swscanf_s(strTimeStart.GetBuffer(strTimeStart.GetLength()), L"%d-%d-%d", &a, &b, &c);
	CTime  time1(a, b, c, 0, 0, 0);
	swscanf_s(strTimeEnd.GetBuffer(strTimeEnd.GetLength()), L"%d-%d-%d", &a, &b, &c);
	CTime  time2(a, b, c, 0, 0, 0);
	CTimeSpan ts = time2 - time1;
	if (ts < 0)
	{
		return FALSE;
	}
	else
	{
		return TRUE;
	}
}

BOOL CDatePickDlg::IsValidTime(CString time)
{
	int nYear, nMonth, nDay;
	if (swscanf_s(time.GetBuffer(time.GetLength()), L"%d-%d-%d", &nYear, &nMonth, &nDay) != 3)
	{
		return FALSE;
	}
	COleDateTime dt;
	if (dt.SetDate(nYear, nMonth, nDay))
	{
		//判断日期合法
		return FALSE;
	}
	return TRUE;
}
