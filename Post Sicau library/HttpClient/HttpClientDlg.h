#pragma once


// HttpClientDlg 对话框

class HttpClientDlg : public CDialog
{
	DECLARE_DYNAMIC(HttpClientDlg)

public:
	HttpClientDlg(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~HttpClientDlg();

// 对话框数据
	enum { IDD = IDD_HttpClient };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

	DECLARE_MESSAGE_MAP()
public:
	CString m_Url;
	CString m_response;
	afx_msg void OnBnClickedButton1();
};
