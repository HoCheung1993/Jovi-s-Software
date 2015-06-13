// FileShredDll.h : FileShredDll DLL 的主头文件
//

#pragma once

#ifndef __AFXWIN_H__
	#error "在包含此文件之前包含“stdafx.h”以生成 PCH 文件"
#endif

#include "resource.h"		// 主符号


// CFileShredDllApp
// 有关此类实现的信息，请参阅 FileShredDll.cpp
//

class CFileShredDllApp : public CWinApp
{
public:
	CFileShredDllApp();

// 重写
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
