
// SicauPhotoWatcher.cpp : 定义应用程序的类行为。
//

#include "stdafx.h"
#include "afxwinappex.h"
#include "afxdialogex.h"
#include "SicauPhotoWatcher.h"
#include "PhotoWatcherDlg.h"
#include "UpdateSever.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSicauPhotoWatcherApp

BEGIN_MESSAGE_MAP(CSicauPhotoWatcherApp, CWinApp)
END_MESSAGE_MAP()


// CSicauPhotoWatcherApp 构造

CSicauPhotoWatcherApp::CSicauPhotoWatcherApp()
{
	// 支持重新启动管理器
	m_dwRestartManagerSupportFlags = AFX_RESTART_MANAGER_SUPPORT_RESTART;
#ifdef _MANAGED
	// 如果应用程序是利用公共语言运行时支持(/clr)构建的，则: 
	//     1) 必须有此附加设置，“重新启动管理器”支持才能正常工作。
	//     2) 在您的项目中，您必须按照生成顺序向 System.Windows.Forms 添加引用。
	System::Windows::Forms::Application::SetUnhandledExceptionMode(System::Windows::Forms::UnhandledExceptionMode::ThrowException);
#endif

	// TODO:  将以下应用程序 ID 字符串替换为唯一的 ID 字符串；建议的字符串格式
	//为 CompanyName.ProductName.SubProduct.VersionInformation
	SetAppID(_T("SicauPhotoWatcher.AppID.NoVersion"));

	// TODO:  在此处添加构造代码，
	// 将所有重要的初始化放置在 InitInstance 中
}

// 唯一的一个 CSicauPhotoWatcherApp 对象

CSicauPhotoWatcherApp theApp;


// CSicauPhotoWatcherApp 初始化

BOOL CSicauPhotoWatcherApp::InitInstance()
{
	// 如果一个运行在 Windows XP 上的应用程序清单指定要
	// 使用 ComCtl32.dll 版本 6 或更高版本来启用可视化方式，
	//则需要 InitCommonControlsEx()。  否则，将无法创建窗口。
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	// 将它设置为包括所有要在应用程序中使用的
	// 公共控件类。
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();


	// 初始化 OLE 库
	if (!AfxOleInit())
	{
		AfxMessageBox(IDP_OLE_INIT_FAILED);
		return FALSE;
	}

	AfxEnableControlContainer();

	EnableTaskbarInteraction(FALSE);

	// 使用 RichEdit 控件需要  AfxInitRichEdit2()	
	// AfxInitRichEdit2();

	// 标准初始化
	// 如果未使用这些功能并希望减小
	// 最终可执行文件的大小，则应移除下列
	// 不需要的特定初始化例程
	// 更改用于存储设置的注册表项
	// TODO:  应适当修改该字符串，
	// 例如修改为公司或组织名
	SetRegistryKey(_T("应用程序向导生成的本地应用程序"));


	//版本号
	VerInfo oldverinfo;
	oldverinfo.Version = "2.2.0.0";
	oldverinfo.Date = "20140921";

	// 若要创建主窗口，此代码将创建新的框架窗口
	// 对象，然后将其设置为应用程序的主窗口对象

	//MessageBox(NULL, "使用工具，请保持内外网畅通！", "提示", MB_OK | MB_ICONINFORMATION);

	//版本更新检查
	VerInfo verinfo;
	UpdateSever updatesever;
	updatesever.GetVersionInfo(&verinfo);
	if (updatesever.CheckVeision(&oldverinfo, &verinfo)) //发现更新
	{
		updatesever.DoUpdate(&verinfo);
	}

	//无网络可用
	PhotoWatcherDlg Dlg;
	m_pMainWnd = &Dlg;
	SkinH_Init(m_hInstance);//初始化皮肤，并不是加载皮肤注意哦，这个是加载皮肤的前提而已。
	Dlg.DoModal();
	return TRUE;
}

int CSicauPhotoWatcherApp::ExitInstance()
{
	//TODO:  处理可能已添加的附加资源
	AfxOleTerm(FALSE);

	return CWinApp::ExitInstance();
}

// CSicauPhotoWatcherApp 消息处理程序

