#include "stdafx.h"
#include "DllShredDlg.h"
#include "DllFileTreeDlg.h"
#include "UACHelper.h"
#include "VersionHelper.h"

extern CShredDlg *g_pDllDlg;

extern "C" __declspec(dllexport) void CreateCleanerDlg()
{
	if (!CVersionHelper::IsWindowsXPOrGreater())
	{
		MessageBox(NULL, L"文件粉碎機不支持WINXP以下系统！", L"錯誤", MB_OK | MB_ICONWARNING);
		return;
	}
	if (CVersionHelper::IsWindowsVistaOrGreater()) //UAC权限
	{
		if (!CUACHelper::IsProcessRunAsAdmin())  //非管理员运行进程
		{
			MessageBox(NULL,L"非管理員權限無法加載文件粉碎機！", L"錯誤", MB_OK | MB_ICONWARNING);
			return;
		}
	}
	if (g_pDllDlg == NULL)
	{
		g_pDllDlg = new CShredDlg();
		AFX_MANAGE_STATE(AfxGetStaticModuleState());
		//父窗体为桌面
		g_pDllDlg->Create(IDD_DLL_SHRED_DIALOG, CWnd::FromHandle(GetDesktopWindow()));
		g_pDllDlg->ShowWindow(SW_SHOW);
		g_pDllDlg->CenterWindow();
	}
	else
	{
		if (::IsWindow(g_pDllDlg->GetSafeHwnd()))
		{
			//激活当前窗体
			g_pDllDlg->ShowWindow(SW_SHOWNORMAL);
			g_pDllDlg->SetForegroundWindow();
		}
		else
		{
			AFX_MANAGE_STATE(AfxGetStaticModuleState());
			g_pDllDlg->Create(IDD_DLL_SHRED_DIALOG, CWnd::FromHandle(GetDesktopWindow()));
			g_pDllDlg->ShowWindow(SW_SHOW);
			g_pDllDlg->CenterWindow();
		}
	}
}

extern "C" __declspec(dllexport) void CreateModalCleanerDlg()
{
	if (!CVersionHelper::IsWindowsXPOrGreater())
	{
		MessageBox(NULL, L"文件粉碎機不支持WINXP以下系统！", L"錯誤", MB_OK | MB_ICONWARNING);
		return;
	}
	if (CVersionHelper::IsWindowsVistaOrGreater()) //UAC权限
	{
		if (!CUACHelper::IsProcessRunAsAdmin())  //非管理员运行进程
		{
			MessageBox(NULL, L"非管理員權限無法加載文件粉碎機！", L"錯誤", MB_OK | MB_ICONWARNING);
			return;
		}
	}
	AFX_MANAGE_STATE(AfxGetStaticModuleState());
	CShredDlg dlg;
	dlg.DoModal();
}