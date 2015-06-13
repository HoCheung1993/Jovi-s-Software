
// GetStockInformationDlg.cpp : implementation file
//

#include "stdafx.h"
#include "GetStockInformation.h"
#include "GetStockInformationDlg.h"
#include "afxdialogex.h"
#include "DatePickDlg.h"
#include "ProgressDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CGetStockInformationDlg dialog


CGetStockInformationDlg::CGetStockInformationDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CGetStockInformationDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	CTime t;
	t = CTime::GetCurrentTime();
	m_strDateStart = t.Format(L"%Y-%m-%d");
	m_strDateEnd = t.Format(L"%Y-%m-%d");
}

void CGetStockInformationDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CGetStockInformationDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BTN_INITIALIZE, &CGetStockInformationDlg::OnBnClickedBtnInitialize)
	ON_BN_CLICKED(IDC_BTN_CHOOSEDATE, &CGetStockInformationDlg::OnBnClickedBtnChoosedate)
	ON_MESSAGE(WM_DATE_CHANGED, &CGetStockInformationDlg::OnDateChanged)
	ON_BN_CLICKED(IDC_BUTTON_UPDATE, &CGetStockInformationDlg::OnBnClickedButtonUpdate)
END_MESSAGE_MAP()


// CGetStockInformationDlg message handlers

BOOL CGetStockInformationDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	SetDlgItemText(IDC_STATIC_DATE_START, m_strDateStart);
	SetDlgItemText(IDC_STATIC_DATE_END, m_strDateEnd);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CGetStockInformationDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CGetStockInformationDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CGetStockInformationDlg::OnBnClickedBtnInitialize()
{
	// TODO: Add your control notification handler code here
	PDATEINFO pDateInfo = new DATEINFO{ L"1991-01-01", m_strDateEnd };
	CProgressDlg dlg(pDateInfo, TRUE, this);
	dlg.DoModal();
}


void CGetStockInformationDlg::OnBnClickedBtnChoosedate()
{
	// TODO: Add your control notification handler code here
	PDATEINFO pDateInfo = new DATEINFO{ m_strDateStart, m_strDateEnd };
	CDatePickDlg dlg(pDateInfo , this);
	dlg.DoModal();
}


afx_msg LRESULT CGetStockInformationDlg::OnDateChanged(WPARAM wParam, LPARAM lParam)
{
	PDATEINFO pDATEINFO = (PDATEINFO)wParam;
	m_strDateStart = pDATEINFO->strDateStart;
	m_strDateEnd = pDATEINFO->strDateEnd;
	SetDlgItemText(IDC_STATIC_DATE_START, m_strDateStart);
	SetDlgItemText(IDC_STATIC_DATE_END, m_strDateEnd);
	delete pDATEINFO;
	return 0;
}


void CGetStockInformationDlg::OnBnClickedButtonUpdate()
{
	// TODO: Add your control notification handler code here
	PDATEINFO pDateInfo = new DATEINFO{ m_strDateStart, m_strDateEnd };
	CProgressDlg dlg(pDateInfo, FALSE, this);
	dlg.DoModal();
}
