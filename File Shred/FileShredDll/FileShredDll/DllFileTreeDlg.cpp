// FileTreeDlg.cpp : implementation file
//

#include "stdafx.h"
#include "FileCleanerDll.h"
#include "DllFileTreeDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define  DEBUG_
#endif
// CFileTreeDlg dialog


CFileTreeDlg::CFileTreeDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CFileTreeDlg::IDD, pParent)
{
}

void CFileTreeDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CFileTreeDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_NOTIFY(TVN_ITEMEXPANDING, IDC_TREE_FILE, OnItemExpandingTreeCtrl)
	ON_NOTIFY(NM_DBLCLK, IDC_TREE_FILE, OnNMDblclkTreectrl)
	ON_WM_SHOWWINDOW()
	ON_NOTIFY(NM_CLICK, IDC_TREE_FILE, &CFileTreeDlg::OnNMClickTree1)
	ON_BN_CLICKED(IDC_BUTTON_OK, &CFileTreeDlg::OnBnClickedButtonOk)
	ON_BN_CLICKED(IDC_BUTTON_CANCEL, &CFileTreeDlg::OnBnClickedButtonNo)
END_MESSAGE_MAP()


// CFileTreeDlg message handlers

BOOL CFileTreeDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CFileTreeDlg::OnPaint()
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
HCURSOR CFileTreeDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

void CFileTreeDlg::OnItemExpandingTreeCtrl(NMHDR *pNMHDR, LRESULT *pResult)
{
	m_treeCtrlBuilder.OnItemExpanding(pNMHDR, pResult);
}

void CFileTreeDlg::OnNMDblclkTreectrl(NMHDR *pNMHDR, LRESULT *pResult)
{
	HTREEITEM hTreeItem = m_treeCtrl.GetSelectedItem();
	if (m_treeCtrl.GetChildItem(hTreeItem) == NULL) {
		CString strText(m_treeCtrlBuilder.GetFullPath(hTreeItem));
	}
	*pResult = 1;
}

void CFileTreeDlg::InitializeExplorerTree()
{
	m_treeCtrl.SubclassDlgItem(IDC_TREE_FILE, this);
	m_treeCtrlBuilder.SetTreeCtrl(&m_treeCtrl);
	PathBuildPolicy &pathBuildPolicy = m_treeCtrlBuilder.GetBuildPolicy();
	CSystemImageList &systemImageList = pathBuildPolicy.GetSystemImageList();
	m_treeCtrl.SetImageList(&systemImageList.GetImageList(), TVSIL_NORMAL);
	TV_INSERTSTRUCT tvInsertStruct;

	DWORD dwDrives;
	char a;
	CString DriveName;
	dwDrives = GetLogicalDrives();

	SHFILEINFO sfi;
	memset(&sfi, 0, sizeof(sfi));

	//获得桌面，文档，磁盘树

	TCHAR MyDir[MAX_PATH];
	SHGetSpecialFolderPath(this->GetSafeHwnd(), MyDir, CSIDL_DESKTOP, 0);
	LPTSTR lpstrDestDir = MyDir;
	SHGetFileInfo(MyDir, FILE_ATTRIBUTE_DIRECTORY, &sfi, sizeof(sfi), SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_OPENICON);
	ZeroMemory(&tvInsertStruct, sizeof(TV_INSERTSTRUCT));
	tvInsertStruct.hParent = TVI_ROOT;
	tvInsertStruct.hInsertAfter = TVI_LAST;
	tvInsertStruct.item.mask = TVIF_CHILDREN | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_TEXT;
	tvInsertStruct.item.cChildren = m_treeCtrlBuilder.HasChildren(lpstrDestDir);
	tvInsertStruct.item.pszText = lpstrDestDir;
	tvInsertStruct.item.iImage = sfi.iIcon;
	tvInsertStruct.item.iSelectedImage = sfi.iIcon;
	m_treeCtrl.InsertItem(&tvInsertStruct);


	SHGetSpecialFolderPath(this->GetSafeHwnd(), MyDir, CSIDL_PERSONAL, 0);
	lpstrDestDir = MyDir;
	SHGetFileInfo(MyDir, FILE_ATTRIBUTE_DIRECTORY, &sfi, sizeof(sfi), SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_OPENICON);
	ZeroMemory(&tvInsertStruct, sizeof(TV_INSERTSTRUCT));
	tvInsertStruct.hParent = TVI_ROOT;
	tvInsertStruct.hInsertAfter = TVI_LAST;
	tvInsertStruct.item.mask = TVIF_CHILDREN | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_TEXT;
	tvInsertStruct.item.cChildren = m_treeCtrlBuilder.HasChildren(lpstrDestDir);
	tvInsertStruct.item.pszText = lpstrDestDir;
	tvInsertStruct.item.iImage = sfi.iIcon;
	tvInsertStruct.item.iSelectedImage = sfi.iIcon;
	m_treeCtrl.InsertItem(&tvInsertStruct);

	a = 'A';
	while (dwDrives > 0)
	{
		if (dwDrives % 2 == 1)
		{
			DriveName.Format(L"%c:\\", a);
			LPTSTR lpstrDestDir = (LPTSTR)(LPCTSTR)DriveName;
			SHGetFileInfo(DriveName, FILE_ATTRIBUTE_DIRECTORY, &sfi, sizeof(sfi), SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_OPENICON);
			ZeroMemory(&tvInsertStruct, sizeof(TV_INSERTSTRUCT));
			tvInsertStruct.hParent = TVI_ROOT;
			tvInsertStruct.hInsertAfter = TVI_LAST;
			tvInsertStruct.item.mask = TVIF_CHILDREN | TVIF_IMAGE | TVIF_SELECTEDIMAGE | TVIF_TEXT;
			tvInsertStruct.item.cChildren = m_treeCtrlBuilder.HasChildren(lpstrDestDir);
			tvInsertStruct.item.pszText = lpstrDestDir;
			tvInsertStruct.item.iImage = sfi.iIcon;
			tvInsertStruct.item.iSelectedImage = sfi.iIcon;
			m_treeCtrl.InsertItem(&tvInsertStruct);
		}
		a++;
		dwDrives /= 2;
	}
}

void CFileTreeDlg::OnShowWindow(BOOL bShow, UINT nStatus)
{
	CDialogEx::OnShowWindow(bShow, nStatus);
	InitializeExplorerTree();
	// TODO: Add your message handler code here
}

void CFileTreeDlg::OnNMClickTree1(NMHDR* pNMHDR, LRESULT* pResult)
{
	// TODO: Add your control notification handler code here  
	NM_TREEVIEW* pHdr = (NM_TREEVIEW*)pNMHDR;
	*pResult = 0;
	CPoint point;
	UINT uFlag = 0;
	GetCursorPos(&point);
	m_treeCtrl.ScreenToClient(&point);
	HTREEITEM item = m_treeCtrl.HitTest(point, &uFlag);
	if ((item) && (TVHT_ONITEMSTATEICON & uFlag))
	{
		BOOL bCheck = m_treeCtrl.GetCheck(item);
		SetItemCheckState(item, bCheck);
		if (!bCheck)  //选中
		{
			CString path = GetCurItemRootPath(item);
			m_lstFilePath.push_front(path);
			//			MessageBox(L"被选中"+path);
		}
		else
		{
			CString path = GetCurItemRootPath(item);
			m_lstFilePath.remove(path);
			//			MessageBox(L"取消选中1"+path);
		}
	}
}

void CFileTreeDlg::SetItemCheckState(HTREEITEM item, BOOL bCheck)
{
	SetChildCheck(item, bCheck);
	SetParentCheck(item, bCheck);
}

void CFileTreeDlg::SetChildCheck(HTREEITEM item, BOOL bCheck)
{
	HTREEITEM child = m_treeCtrl.GetChildItem(item);
	BOOL bChildCheck;
	while (child)
	{
		bChildCheck = m_treeCtrl.GetCheck(child);
		if (bChildCheck)
		{
			CString path = GetCurItemRootPath(child);
			m_lstFilePath.remove(path);
			//MessageBox(L"取消选中2" + path);
		}
		m_treeCtrl.SetCheck(child, FALSE);
		SetChildCheck(child, FALSE);
		child = m_treeCtrl.GetNextItem(child, TVGN_NEXT);
	}
}

void CFileTreeDlg::SetParentCheck(HTREEITEM item, BOOL bCheck)
{
	HTREEITEM parent = m_treeCtrl.GetParentItem(item);
	if (parent == NULL)
		return;
	BOOL bParentCheck = m_treeCtrl.GetCheck(parent);
	if (bParentCheck)
	{
		CString path = GetCurItemRootPath(parent);
		m_lstFilePath.remove(path);
		//MessageBox(L"取消选中3" + path);
	}
	if (bCheck)
	{
		m_treeCtrl.SetCheck(parent, !bCheck);
	}
	else
	{
		HTREEITEM bro = m_treeCtrl.GetNextItem(item, TVGN_NEXT);
		BOOL bFlag = false;
		while (bro)
		{
			if (m_treeCtrl.GetCheck(bro))
			{
				bFlag = true;
				break;
			}
			bro = m_treeCtrl.GetNextItem(bro, TVGN_NEXT);
		}
		if (!bFlag)
		{
			bro = m_treeCtrl.GetNextItem(item, TVGN_PREVIOUS);
			while (bro)
			{
				if (m_treeCtrl.GetCheck(bro))
				{
					bFlag = true;
					break;
				}
				bro = m_treeCtrl.GetNextItem(bro, TVGN_PREVIOUS);
			}
		}
		if (!bFlag)
		{
			m_treeCtrl.SetCheck(parent, false);
		}
	}
	SetParentCheck(parent, !bCheck);
}

CString CFileTreeDlg::GetCurItemRootPath(HTREEITEM htiItem)//当前选中的节点
{
	if (htiItem == m_treeCtrl.GetRootItem())
	{
		return m_treeCtrl.GetItemText(htiItem);
	}
	else
	{
		HTREEITEM parent = htiItem;
		CString strPath; //保存当前节点路径
		strPath = m_treeCtrl.GetItemText(htiItem);
		while ((parent = m_treeCtrl.GetParentItem(parent)) != NULL)
		{
			CString strtext = m_treeCtrl.GetItemText(parent);
			if (strtext[(strtext.GetLength() - 1)] != '\\')
			{
				strtext += "\\";
			}
			strPath = strtext + strPath;
		}
		return strPath;
	}
}

void CFileTreeDlg::OnBnClickedButtonOk()
{
	// TODO: Add your control notification handler code here
	::SendMessage(this->GetParent()->GetSafeHwnd(), WM_ADD_FILES, (WPARAM)&m_lstFilePath, 0);
	SendMessage(WM_CLOSE);
	//for each (CString var in m_lstFilePath)
	//{
	//	MessageBox(L"已选中" + var);
	//}
}

void CFileTreeDlg::OnBnClickedButtonNo()
{
	// TODO: Add your control notification handler code here
	SendMessage(WM_CLOSE);
}


BOOL CFileTreeDlg::PreTranslateMessage(MSG* pMsg)
{
	// TODO: Add your specialized code here and/or call the base class
	if (pMsg->message == WM_KEYDOWN   &&   pMsg->wParam == VK_ESCAPE)     return   TRUE;
	if (pMsg->message == WM_KEYDOWN   &&   pMsg->wParam == VK_RETURN)   return   TRUE;
	else
		return CDialogEx::PreTranslateMessage(pMsg);
}
