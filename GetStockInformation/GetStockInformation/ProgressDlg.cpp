// ProgressDlg.cpp : implementation file
//

#include "stdafx.h"
#include "GetStockInformation.h"
#include "ProgressDlg.h"
#include "afxdialogex.h"



// CProgressDlg dialog


IMPLEMENT_DYNAMIC(CProgressDlg, CDialogEx)

CProgressDlg::CProgressDlg(PDATEINFO pDateInfo,BOOL bIsInitialize , CWnd* pParent /*=NULL*/)
	: CDialogEx(CProgressDlg::IDD, pParent)
{
	m_pStockctrl = new CStockInformationCtrl(pDateInfo, bIsInitialize);
	m_nCount = 0;
	m_TotalCount = 0;
}

CProgressDlg::~CProgressDlg()
{
	if (m_pStockctrl != NULL)
	{
		delete m_pStockctrl;
		m_pStockctrl = NULL;
	}
}

void CProgressDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_PROGRESS, m_progress);
}


BEGIN_MESSAGE_MAP(CProgressDlg, CDialogEx)
	ON_MESSAGE(WM_UPDATE_DOWNLOADINFO, &CProgressDlg::OnUpdateDownloadinfo)
	ON_BN_CLICKED(IDC_BTN_PAUSE, &CProgressDlg::OnBnClickedBtnPause)
	ON_BN_CLICKED(IDC_BTN_CANCEL, &CProgressDlg::OnBnClickedBtnCancel)
	ON_MESSAGE(WM_INITIAL_DOWNLOADINFO, &CProgressDlg::OnInitialDownloadinfo)
END_MESSAGE_MAP()



BOOL CProgressDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	m_pStockctrl->SetParentWnd(this);
	// TODO:  Add extra initialization here
	if (m_pStockctrl->InitializeStocksList())
	{
		m_pStockctrl->DownLoadStockData();
	}

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}


afx_msg LRESULT CProgressDlg::OnUpdateDownloadinfo(WPARAM wParam, LPARAM lParam)
{
	PPROGRESS_INFO pi = (PPROGRESS_INFO)wParam;
	SetDlgItemText(IDC_STATIC_TIPS, pi->strTIPS);
	m_nCount += pi->nPROGRESS;
	if (m_TotalCount != 0)
	{
		int progress = (int)(100.0 * m_nCount / m_TotalCount);
		m_progress.SetPos(progress);
	}
	delete pi;
	pi = NULL;
	return 0;
}


void CProgressDlg::OnBnClickedBtnPause()
{
	// TODO: Add your control notification handler code here
	CString temp;
	GetDlgItemText(IDC_BTN_PAUSE, temp);
	if (temp == L"ÔÝÍ£")
	{
		m_pStockctrl->Pause(TRUE);
		SetDlgItemText(IDC_BTN_PAUSE, L"¼ÌÐø");
	}
	else
	{
		m_pStockctrl->Pause(FALSE);
		SetDlgItemText(IDC_BTN_PAUSE, L"ÔÝÍ£");
	}

}


void CProgressDlg::OnBnClickedBtnCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}



void CProgressDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	m_pStockctrl->Cancel();
	CDialogEx::OnCancel();
}


afx_msg LRESULT CProgressDlg::OnInitialDownloadinfo(WPARAM wParam, LPARAM lParam)
{
	m_TotalCount = (int)wParam;
	m_nCount = 0;
	return 0;
}
