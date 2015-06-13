#pragma once
typedef int E_RESULT;
#define SUCCESS 1;
#define FAIL 0;

#define WM_DATE_CHANGED WM_USER + 1001
typedef struct _DATE_INFO_
{
	CString strDateStart;
	CString strDateEnd;
}DATEINFO, *PDATEINFO;

