
// RemoteDesktopDlg.h : header file
//

#pragma once
#include "mstscax.h"
#include "afxcmn.h"
#include "ReportCtrl.h"
#include "Define.h"
#include <map>
#include <vector>


// CRemoteDesktopDlg dialog
class CRemoteDesktopDlg : public CDialogEx
{
// Construction
public:
	CRemoteDesktopDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	enum { IDD = IDD_REMOTEDESKTOP_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;
	CMstscax m_rdp;
	std::map<SERVER, INFO> m_mapServerInfo;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	CReportCtrl m_list_servers;
	afx_msg void OnBnClickedBtnConnect();
	afx_msg void OnBnClickedButtonDisconnect();

private:
	void SetDlgSize(BOOL bFlag);
	void FastMode_RemoteDesktop();
	void STDMode_RemoteDesktop(BOOL bFullScreen);
	static UINT InitThread(LPVOID pParam);
	static UINT WriteThread(LPVOID pParam);
	E_RESULT GetServer_Info(SERVER &sever,INFO &info);
	E_RESULT GetRDPPassword(CString &strPassword1, CString &strPassword2);
public:
	DECLARE_EVENTSINK_MAP()
	void OnDisconnectedMstscax(long discReason);
protected:
	afx_msg LRESULT OnInitialServers(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnBnClickedBtnAddServer();
	afx_msg void OnBnClickedBtnMstsc();
protected:
	afx_msg LRESULT OnAddServer(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnNMClickListServers(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedBtnModifyServer();
	afx_msg void OnBnClickedBtnConnectDelServer();
protected:
	afx_msg LRESULT OnModifyServer(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnNMDblclkListServers(NMHDR *pNMHDR, LRESULT *pResult);
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	afx_msg void OnClose();
protected:
	afx_msg LRESULT OnDeleteServer(WPARAM wParam, LPARAM lParam);
	virtual void OnOK();

protected:
	void BackupConfig();
	void ImportConfig();
	void About();
};


class CIniCore
{
public:
	CIniCore();
	E_RESULT ReadIni(CString strFilepath, CString &strResult,CRemoteDesktopDlg *dlg);
	E_RESULT WriteIni(CString strFilepath, CString &strValue);
	E_RESULT SetOpenFlags(int flags);
private:
	int m_OpenFlags;
};

class CIniHelper
{
public:
	static E_RESULT Analyse(CString& strSource,CRemoteDesktopDlg *pDlg);
	static E_RESULT Summarize(CString &strSource, CString& strResult);
private:
	static E_RESULT Encrypt(CString &strSource, CString &strResult, DWORD Key);
	static E_RESULT Decrypt(CString &strSource, CString &strResult, DWORD Key);
};

typedef struct _NETSTATUSDATA_
{
	CRemoteDesktopDlg *dlg;
	CString ipAddress;
	CString port;
}NETSTATUSDATA, *PNETSTATUSDATA;

class CNetStatusHelper
{
public:
	static E_RESULT CheckIPandPort(PNETSTATUSDATA pNetstatusdata);
	static E_RESULT NetInitial();
private:
	static UINT Checkthread(LPVOID pParam);
};