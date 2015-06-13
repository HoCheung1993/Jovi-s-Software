
// FileShredDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "FileShred.h"
#include "FileShredDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

HINSTANCE g_hDll = NULL;
// CFileShredDlg 对话框



CFileShredDlg::CFileShredDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CFileShredDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CFileShredDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CFileShredDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON1, &CFileShredDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDC_BUTTON_GETICON, &CFileShredDlg::OnBnClickedButtonGeticon)
END_MESSAGE_MAP()


// CFileShredDlg 消息处理程序

BOOL CFileShredDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// 设置此对话框的图标。  当应用程序主窗口不是对话框时，框架将自动
	//  执行此操作
	SetIcon(m_hIcon, TRUE);			// 设置大图标
	SetIcon(m_hIcon, FALSE);		// 设置小图标

	// TODO:  在此添加额外的初始化代码

	return TRUE;  // 除非将焦点设置到控件，否则返回 TRUE
}

// 如果向对话框添加最小化按钮，则需要下面的代码
//  来绘制该图标。  对于使用文档/视图模型的 MFC 应用程序，
//  这将由框架自动完成。

void CFileShredDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // 用于绘制的设备上下文

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// 使图标在工作区矩形中居中
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// 绘制图标
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CFileShredDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CFileShredDlg::OnBnClickedButton1()
{
	// TODO:  在此添加控件通知处理程序代码
	typedef void(*lpFunc)();
	g_hDll = LoadLibrary(L".\\FileCleanerDll.dll");
	if (g_hDll == NULL)
	{
		MessageBox(L"加载FileCleanerDll.dll失败", L"错误", MB_OK);
		return;
	}
	lpFunc ShowCleanerDlg = (lpFunc)GetProcAddress(g_hDll, "CreateCleanerDlg");
	if (ShowCleanerDlg == NULL)
	{
		MessageBox(L"加载CreateCleanerDlg失败", L"错误", MB_OK);
		return;
	}
	ShowCleanerDlg();	
}


void CFileShredDlg::OnBnClickedButtonGeticon()
{
	// TODO: Add your control notification handler code here
	
}


void CFileShredDlg::OnCancel()
{
	// TODO: Add your specialized code here and/or call the base class
	CDialogEx::OnCancel();
	if (g_hDll != NULL)
	{
		FreeLibrary(g_hDll);
		g_hDll = NULL;
	}
}
