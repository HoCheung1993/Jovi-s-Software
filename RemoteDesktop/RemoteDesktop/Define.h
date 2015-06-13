#pragma once
typedef int E_RESULT;
#define SUCCESS 1
#define FAIL 0

#define  WM_ADD_SERVER WM_USER + 101
#define  WM_MODIFY_SERVER WM_USER + 102
#define  WM_INITIAL_SERVERS WM_USER + 103

typedef struct _SERVER_
{
	CString name;
	CString ip;
	CString port;
	CString user;
	//map中key为结构体时需要重写<
	bool operator <(const _SERVER_& other) const
	{
		if (ip < other.ip)
		{
			return true;
		}
		else if (ip == other.ip)
		{
			if (port < other.port)
			{
				return true;
			}
			else if (port == other.port)
			{
				if (user < other.user)
				{
					return true;
				}
				else if (user == other.user)
				{
					return false;
				}
			}
		}
		return false;
	}
}SERVER, *PSERVER;

typedef struct _INFO_
{
	CString password;
	CString note;
}INFO, *PINFO;

typedef struct _DIALOGPASSDATA_
{
	BOOL bModify;
	SERVER server;
	INFO info;
}DIALOGPASSDATA,*PDIALOGPASSDATA;

typedef struct _SERVERINFO_
{
	SERVER server;
	INFO info;
}SERVERSINFO, *PSERVERINFO;

