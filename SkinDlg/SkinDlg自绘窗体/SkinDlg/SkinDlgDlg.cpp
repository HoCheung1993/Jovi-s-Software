
// SkinDlgDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "SkinDlg.h"
#include "SkinDlgDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSkinDlg 对话框



CSkinDlg::CSkinDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CSkinDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CSkinDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CSkinDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_SIZE()
	ON_WM_LBUTTONDOWN()
END_MESSAGE_MAP()


// CSkinDlg 消息处理程序

BOOL CSkinDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	//产生阴影
	SetClassLong(this->m_hWnd, GCL_STYLE, GetClassLong(this->m_hWnd, GCL_STYLE) | CS_DROPSHADOW);

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

void CSkinDlg::OnPaint()
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
		//画边线
		CPaintDC dc2(this);
		CRect rect;
		GetClientRect(rect);

		//背景绘图
		dc2.SetBkMode(TRANSPARENT);
		CDC MemDC;
		MemDC.CreateCompatibleDC(&dc2);
		CBitmap Bitmap;
		BITMAP bmpinfo;
		Bitmap.LoadBitmap(IDB_BK1);
		Bitmap.GetObject(sizeof(bmpinfo),& bmpinfo);  //获取图片信息
		CBitmap *pOldBitmap = MemDC.SelectObject(&Bitmap);
		
		dc2.StretchBlt(-1, -1, rect.Width()+2, rect.Height()+2, &MemDC, 0, 0, bmpinfo.bmWidth, bmpinfo.bmHeight, SRCCOPY);  //拉伸位图
//		dc2.BitBlt(rect.left, rect.top, rect.Width(), rect.Height(), &MemDC, rect.left, rect.top, SRCCOPY);
		MemDC.SelectObject(pOldBitmap);
		MemDC.DeleteDC();

		//外边框
		CPen *oldpen = NULL;
		CPen newpen(PS_SOLID, 1, RGB(27, 147, 186));
		oldpen = dc2.SelectObject(&newpen);

		dc2.MoveTo(rect.left, CORNER_SIZE);
		dc2.LineTo(CORNER_SIZE, rect.top);
		dc2.LineTo(rect.right - CORNER_SIZE - 1, rect.top);
		dc2.LineTo(rect.right - 1, CORNER_SIZE);
		dc2.LineTo(rect.right - 1, rect.bottom - CORNER_SIZE - 1);
		dc2.LineTo(rect.right - CORNER_SIZE - 1, rect.bottom - 1);
		dc2.LineTo(CORNER_SIZE, rect.bottom - 1);
		dc2.LineTo(rect.left, rect.bottom - CORNER_SIZE - 1);
		dc2.LineTo(rect.left, CORNER_SIZE);

		//填充空缺处
		dc2.MoveTo(rect.left + 1, CORNER_SIZE);
		dc2.LineTo(CORNER_SIZE + 1, rect.top);

		dc2.MoveTo(rect.right - CORNER_SIZE - 1, rect.top + 1);
		dc2.LineTo(rect.right - 1, CORNER_SIZE + 1);

		dc2.MoveTo(rect.right - 2, rect.bottom - CORNER_SIZE - 1);
		dc2.LineTo(rect.right - CORNER_SIZE - 1, rect.bottom - 1);

		dc2.MoveTo(CORNER_SIZE, rect.bottom - 2);
		dc2.LineTo(rect.left, rect.bottom - CORNER_SIZE - 2);

		dc2.SelectObject(oldpen);

		//内边框
		CPen newpen2(PS_SOLID, 1, RGB(196, 234, 247));
		oldpen = dc2.SelectObject(&newpen2);

		dc2.MoveTo(rect.left + 1, CORNER_SIZE + 1);
		dc2.LineTo(CORNER_SIZE + 1, rect.top + 1);
		dc2.LineTo(rect.right - CORNER_SIZE - 2, rect.top + 1);
		dc2.LineTo(rect.right - 2, CORNER_SIZE + 1);
		dc2.LineTo(rect.right - 2, rect.bottom - CORNER_SIZE - 2);
		dc2.LineTo(rect.right - CORNER_SIZE - 2, rect.bottom - 2);
		dc2.LineTo(CORNER_SIZE + 1, rect.bottom - 2);
		dc2.LineTo(rect.left + 1, rect.bottom - CORNER_SIZE - 2);
		dc2.LineTo(rect.left + 1, CORNER_SIZE + 1);
		CDialog::OnPaint();
	}
}

//当用户拖动最小化窗口时系统调用此函数取得光标
//显示。
HCURSOR CSkinDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CSkinDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialogEx::OnSize(nType, cx, cy);
	if (nType != SIZE_MAXIMIZED)
	{
		CRect rc;
		GetClientRect(&rc);

		CRgn rgn;
		CPoint points[8] = { CPoint(rc.left, CORNER_SIZE), CPoint(CORNER_SIZE, rc.top),
			CPoint(rc.right - CORNER_SIZE, rc.top), CPoint(rc.right, CORNER_SIZE),
			CPoint(rc.right, rc.bottom - CORNER_SIZE - 1), CPoint(rc.right - CORNER_SIZE - 1, rc.bottom),
			CPoint(CORNER_SIZE + 1, rc.bottom), CPoint(rc.left, rc.bottom - CORNER_SIZE - 1) };
		int nPolyCounts[1] = { 8 };
		int dd = rgn.CreatePolyPolygonRgn(points, nPolyCounts, 1, WINDING);
		SetWindowRgn(rgn, TRUE);
	}
	else
	{
		SetWindowRgn(NULL, FALSE);
	}
	// TODO:  在此处添加消息处理程序代码
}



void CSkinDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	PostMessage(WM_NCLBUTTONDOWN, HTCAPTION, MAKELPARAM(point.x, point.y));
	CDialogEx::OnLButtonDown(nFlags, point);
}
