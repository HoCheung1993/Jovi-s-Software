#include "stdafx.h"
#include "UpdateSever.h"


UpdateSever::UpdateSever()
{

}

UpdateSever::~UpdateSever()
{

}

//获取版本信息
void UpdateSever::GetVersionInfo(VerInfo *pVerinfo)
{
	CHttp http;
	CString response;
	response = http.get("http://blog.sina.com.cn/s/blog_a62456420101w0j5.html");
	if (response == "" || response.Find("版本号:[") == -1 || response.Find("下载地址:[") == -1 || response.Find("更新日期:[") == -1 || response.Find("强制版本:[") == -1 || response.Find("更新说明:[") == -1)
	{
		return;
	}
	int start = response.Find("版本号:[");
	int end = response.Find("]", start);
	start += 8;
	pVerinfo->Version = response.Mid(start , end - start);

	start = response.Find("下载地址:[", end);
	end = response.Find("]", start);
	start += 10;
	pVerinfo->DownloadUrl = response.Mid(start, end - start);

	start = response.Find("更新日期:[", end);
	end = response.Find("]", start);
	start += 10;
	pVerinfo->Date = response.Mid(start, end - start);

	start = response.Find("强制版本:[", end);
	end = response.Find("]", start);
	start += 10;
	pVerinfo->CanNotUseOld = response.Mid(start, end - start);

	start = response.Find("更新说明:[", end);
	end = response.Find("]", start);
	start += 10;
	pVerinfo->Introduce = response.Mid(start, end - start);
	return;
}

//进行更新检查
bool UpdateSever::CheckVeision(VerInfo *pOldVerinfo,VerInfo *pVerinfo)
{
	if ((_ttoi(pOldVerinfo->Date)-_ttoi(pVerinfo->Date))<0)
	{
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

//进行更新提示
void UpdateSever::DoUpdate(VerInfo *pVerinfo)
{
	CString tips;
	if (pVerinfo->CanNotUseOld=="否")
	{
		tips = "旧版本仍可继续使用。";
	}
	else
	{
		tips = "旧版本不能继续使用！";
	}

	if (IDOK == MessageBox(NULL, "版本号:     "+pVerinfo->Version+"\n更新日期:  "+pVerinfo->Date+"\n更新说明:  "+pVerinfo->Introduce+"\n\n"+tips+"\n现在更新吗？", "发现新版本", MB_OKCANCEL |MB_ICONINFORMATION))
	{
		ShellExecute(NULL, "open", pVerinfo->DownloadUrl, NULL, NULL, SW_SHOWNORMAL);
		exit(0);  //结束自身
	}
	else if (pVerinfo->CanNotUseOld == "是")
	{
		MessageBox(NULL, "请更新后使用！", "错误", MB_OK|MB_ICONERROR);
		exit(0); //强制版本时不更新旧版不能用
	}
}