// ShredDlg.cpp : 实现文件
//
#include "stdafx.h"
#include "DllFileTreeDlg.h"
#include "FileCleanerDll.h"
#include "DllShredDlg.h"
#include "DllDiskWipeDlg.h"
#include "EraseCore.h"
#include "afxdialogex.h"

CFileErase g_erase;
CShredDlg *g_pDllDlg = NULL;
int g_nCurrentIndex = 0;
BOOL g_bEraseStart = FALSE;
// CShredDlg 对话框
//
IMPLEMENT_DYNAMIC(CShredDlg, CDialogEx)

CShredDlg::CShredDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CShredDlg::IDD, pParent)
{

}

CShredDlg::~CShredDlg()
{

}

void CShredDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_FILES, m_wndList);
	DDX_Control(pDX, IDC_BUTTON_ADD_FILE, m_btnADD);
	DDX_Control(pDX, IDC_BUTTON_DEL_FILE, m_btnDELETE);
	DDX_Control(pDX, IDC_BUTTON_PAUSE, m_btnPAUSE);
	DDX_Control(pDX, IDC_BUTTON_SHRED_FILE, m_btnERASE);
	DDX_Control(pDX, IDC_COMBO_nCount, m_combox_nCount);
}


BEGIN_MESSAGE_MAP(CShredDlg, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON_ADD_FILE, &CShredDlg::OnBnClickedButtonAddFiles)
	ON_WM_CLOSE()
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_BUTTON_DEL_FILE, &CShredDlg::OnBnClickedButtonDelFile)
	ON_MESSAGE(WM_ADD_FILES, &CShredDlg::OnAddFiles)
	ON_BN_CLICKED(IDC_BUTTON_SHRED_FILE, &CShredDlg::OnBnClickedButtonShredFile)
	ON_MESSAGE(WM_UPDATE_PROGRESS, &CShredDlg::OnUpdateProcess)
	ON_MESSAGE(WM_UPDATE_SHRED_RESULT, &CShredDlg::OnUpdateResult)
	ON_BN_CLICKED(IDC_BUTTON_PAUSE ,&CShredDlg::OnBnClickedButtonPause)
	ON_BN_CLICKED(IDC_BUTTON_WIPE, &CShredDlg::OnBnClickedButtonWipe)
END_MESSAGE_MAP()


// CShredDlg 消息处理程序


void CShredDlg::OnBnClickedButtonAddFiles()
{
	// TODO:  在此添加控件通知处理程序代码
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CFileTreeDlg dlg(this);
	dlg.DoModal();
}

BOOL CShredDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	// TODO:  在此添加额外的初始化
	CRect rect;
	CRect bar_rect;
	GetWindowRect(rect);
	//暫停按鈕隱藏
	m_btnPAUSE.ShowWindow(SW_HIDE);
	//Statusbar初始化
	if (!m_statusbar.Create(WS_CHILD | WS_VISIBLE | SBT_OWNERDRAW, CRect(0, 0, 0, 0), this , 0))
	{
		return FALSE;
	}
	int strPartDim[2] = { (int)(rect.Width() * 0.85), rect.Width() };
	m_statusbar.SetParts(3, strPartDim);
	m_statusbar.SetText(L"暫無處理任務", 0, 0);
	//進度條初始化
	m_statusbar.GetRect(1, &bar_rect);
	bar_rect.right--;
	bar_rect.bottom--;
	m_progress.Create(WS_VISIBLE | WS_CHILD, bar_rect, &m_statusbar, 1);
	m_progress.SetRange(0, 100);
	m_progress.SetStep(1);

	//Listview初始化
	WCHAR buf[256];
	swprintf_s(buf, L"[V] 名稱, 150 ;文件或文件夾路徑, %d;文件狀態, 155;", rect.Width() - 370 );
	m_wndList.SetColumnHeader(buf);
	m_wndList.SetGridLines(FALSE); // SHow grid lines
	m_wndList.SetCheckboxeStyle(RC_CHKBOX_NORMAL); // Enable checkboxes
	m_wndList.SetEditable(FALSE); // Allow sub-text edit
	m_wndList.SortItems(0, TRUE); // sort the 1st column, ascending

	for (size_t i = 3; i <= 7; i++)
	{
		char buf[2];
		sprintf_s(buf, "%d", i);
		CString temp(buf);
		m_combox_nCount.AddString(temp.GetBuffer(0));
	}
	m_combox_nCount.SetCurSel(0);

	UpdateData(FALSE);
	return TRUE;  // return TRUE unless you set the focus to a control
}

void CShredDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	CDialogEx::OnCancel();
	if (g_pDllDlg != NULL)
	{
		delete g_pDllDlg;
		g_pDllDlg = NULL;
	}
}

void CShredDlg::OnBnClickedButtonDelFile()
{
	// TODO: Add your control notification handler code here
	const int ITERM = m_wndList.GetItemCount();
	//倒着删除
	for (int i = ITERM - 1; i >= 0; i--)  
	{
		if (m_wndList.GetCheck(i))
		{
			m_wndList.DeleteItem(i,FALSE);
		}
	}
}

BOOL CShredDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN   &&   pMsg->wParam == VK_ESCAPE)     return   TRUE;
	if (pMsg->message == WM_KEYDOWN   &&   pMsg->wParam == VK_RETURN)   return   TRUE;
	else
		return CDialogEx::PreTranslateMessage(pMsg);
}

LRESULT CShredDlg::OnAddFiles(WPARAM wParam, LPARAM lParam)
{
	std::list<CString> *p = (std::list<CString> *)wParam;
	for each (CString var in *p)
	{
		if (var.GetLength() > 3)  //除去根目录
		{
			m_wndList.InsertFileIterm(var);
		}
	}
	return 0;
}

//返回下一个粉碎对象编号
int CShredDlg::FindNextItermForErase(int start)
{
	CString temp;
	const int N = m_wndList.GetItemCount();
	if (N <= 0 || start > N - 1)
	{
		return -1;
	}
	for (int i = start; i < N; i++)
	{
		temp = m_wndList.GetItemText(i, 2);
		//当文件成功粉碎时候，字段失败0
		int isFailed = temp.Find('0');
		if (isFailed == -1 && m_wndList.GetCheck(i))
		{
			return i;
		}
	}
	return -2;  //没有处理
}

void CShredDlg::OnBnClickedButtonShredFile()
{
	// TODO: Add your control notification handler code here
	if (!g_bEraseStart)
	{
		CString strCOUNT;
		m_combox_nCount.GetWindowText(strCOUNT);
		g_erase.SetEraseCount(_ttoi(strCOUNT));
		g_nCurrentIndex = FindNextItermForErase(0);
		if (g_nCurrentIndex != -1 && g_nCurrentIndex != -2)
		{
			g_erase.Cancel(TRUE);
			g_bEraseStart = TRUE;
			m_btnADD.EnableWindow(FALSE);
			m_btnDELETE.EnableWindow(FALSE);
			m_combox_nCount.EnableWindow(FALSE);
			m_btnPAUSE.ShowWindow(SW_SHOW);
			m_btnERASE.SetWindowText(L"取消粉碎");
			WCHAR buffer[16] = L"開始粉碎";
			m_wndList.SetItemText(g_nCurrentIndex, 2, buffer);
			CString temp;
			temp = m_wndList.GetItemText(g_nCurrentIndex, 1);
			WCHAR buf[MAX_PATH];
			CStringW strWide = CT2CW(temp);
			wcscpy_s(buf, strWide);
			if (g_erase.ShredPath(buf, m_hWnd) != SUCCESS)
			{
				WCHAR file_error[16];
				swprintf_s(file_error, L"錯誤代碼: %d", GetLastError());
				m_wndList.SetItemText(g_nCurrentIndex, 2, file_error);
				g_erase.Cancel(FALSE);
				g_bEraseStart = FALSE;
				m_btnADD.EnableWindow(TRUE);
				m_btnDELETE.EnableWindow(TRUE);
				m_combox_nCount.EnableWindow(TRUE);
				m_btnPAUSE.ShowWindow(SW_HIDE);
				m_btnERASE.SetWindowTextW(L"粉碎文件");
			}			
		}
	}
	else
	{
		if (g_nCurrentIndex != -1 && g_nCurrentIndex != -2)
		{
			WCHAR buf[16] = L"未處理";
			g_erase.Cancel(FALSE);
			g_bEraseStart = FALSE;
			m_btnADD.EnableWindow(TRUE);
			m_btnDELETE.EnableWindow(TRUE);
			m_combox_nCount.EnableWindow(TRUE);
			m_btnPAUSE.ShowWindow(SW_HIDE);
			m_btnERASE.SetWindowTextW(L"粉碎文件");
			m_wndList.SetItemText(g_nCurrentIndex, 2, buf);
		}
	}
}

LRESULT CShredDlg::OnUpdateProcess(WPARAM wParam, LPARAM lParam)
{
	PPROGRESS_INFO p = (PPROGRESS_INFO)wParam;
	WCHAR buf[16];
	WCHAR bufFilename[MAX_PATH + 20];
	swprintf_s(buf ,L"處理中 %d%%", p->nTotalProgress);
	if (p->bFinished)
	{
		swprintf_s(buf, L"處理完畢", p->nTotalProgress);
		m_statusbar.SetText(L"暫無處理任務", 0, 0);
		m_progress.SetPos(0);
	}
	swprintf_s(bufFilename, L"當前處理: %ls", p->wszFileName);
	m_statusbar.SetText(bufFilename, 0, 0);
	m_progress.SetPos(p->nFileProgress);
	m_wndList.SetItemText(g_nCurrentIndex, 2, buf);
	return TRUE;
}

LRESULT CShredDlg::OnUpdateResult(WPARAM wParam, LPARAM lParam)
{
	PSHRED_RESULT_INFO p = (PSHRED_RESULT_INFO)wParam;
	WCHAR buf[32];
	swprintf_s(buf, L"粉碎個數: %d 失敗: %d", p->nTotalCount, p->nFailedCount);
	//未發現失敗則取消文件前勾選
	if (p->nFailedCount)
	{
		m_wndList.SetCheck(g_nCurrentIndex, 0);
	}
	m_wndList.SetItemText(g_nCurrentIndex, 2, buf);
	int n = FindNextItermForErase(g_nCurrentIndex);
	if (n!= -1 &&n !=-2 )
	{
		g_nCurrentIndex = n;
		CString temp;
		temp = m_wndList.GetItemText(n, 1);
		WCHAR buf[MAX_PATH];
		CStringW strWide = CT2CW(temp);
		wcscpy_s(buf, strWide);
		if (g_erase.ShredPath(buf, m_hWnd) == SUCCESS)
		{
			return TRUE;
		}
		else
		{
			WCHAR file_error[16];
			swprintf_s(file_error, L"錯誤代碼: %d", GetLastError());
			m_wndList.SetItemText(g_nCurrentIndex, 2, file_error);
		}
	}
	g_bEraseStart = FALSE;
	m_btnADD.EnableWindow(TRUE);
	m_btnDELETE.EnableWindow(TRUE);
	m_combox_nCount.EnableWindow(TRUE);
	m_btnPAUSE.ShowWindow(SW_HIDE);
	m_btnERASE.SetWindowTextW(L"粉碎文件");
	m_statusbar.SetText(L"暫無處理任務", 0, 0);
	m_progress.SetPos(0);
	return TRUE;
}

void CShredDlg::OnBnClickedButtonPause()
{
	CString temp;
	m_btnPAUSE.GetWindowText(temp); //獲取現在的按鈕文字
	if (temp == L"暫停")
	{
		g_erase.Pause(TRUE);
		m_btnPAUSE.SetWindowText(L"繼續");
	}
	else
	{
		g_erase.Pause(FALSE);
		m_btnPAUSE.SetWindowText(L"暫停");
	}
}

void CShredDlg::OnBnClickedButtonWipe()
{
	// TODO: Add your control notification handler code here
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CDiskWipeDlg dlg(this);
	dlg.DoModal();
}
