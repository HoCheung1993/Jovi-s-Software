// HttpClientDlg.cpp : 实现文件
//

#include "stdafx.h"
#include "HttpClient.h"
#include "HttpClientDlg.h"
#include "afxdialogex.h"
#include "Http.h"

using namespace std;

// HttpClientDlg 对话框

IMPLEMENT_DYNAMIC(HttpClientDlg, CDialog)

HttpClientDlg::HttpClientDlg(CWnd* pParent /*=NULL*/)
	: CDialog(HttpClientDlg::IDD, pParent)
	, m_Url(_T(""))
	, m_response(_T(""))
{

}

HttpClientDlg::~HttpClientDlg()
{
}

void HttpClientDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Text(pDX, IDC_EDIT1, m_Url);
	DDX_Text(pDX, IDC_EDIT2, m_response);
}


BEGIN_MESSAGE_MAP(HttpClientDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, &HttpClientDlg::OnBnClickedButton1)
END_MESSAGE_MAP()


// HttpClientDlg 消息处理程序

void HttpClientDlg::OnBnClickedButton1()
{
	// TODO:  在此添加控件通知处理程序代码
	CHttp http;
	m_response = http.get("http://202.115.182.3/gdweb/ReaderTable.aspx");
	m_response = http.post(("http://202.115.182.3/gdweb/ReaderLogin.aspx")," " ,("ScriptManager1=UpdatePanel1|ImageButton1&__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwUJNzQ4NzgzNjUwD2QWAgIDD2QWAgIFD2QWAmYPZBYGAgEPZBYCAgEPDxYCHgRUZXh0BesGPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J2RlZmF1bHQuYXNweCc%2BPHNwYW4%2B6aaW6aG1PC9zcGFuPjwvQT48L3RkPjx0ZCBzdHlsZT0iaGVpZ2h0OiAyMXB4Ij48QSBocmVmPSdkZWZhdWx0LmFzcHgnPjxzcGFuPuS5puebruafpeivojwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nTWFnYXppbmVDYW50b1NjYXJjaC5hc3B4Jz48c3Bhbj7mnJ%2FliIrnr4flkI08L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J1Jlc2VydmVkTGlzdC5hc3B4Jz48c3Bhbj7pooTnuqbliLDppoY8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J0V4cGlyZWRMaXN0LmFzcHgnPjxzcGFuPui2heacn%2BWFrOWRijwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nTmV3Qm9vS1NjYXJjaC5hc3B4Jz48c3Bhbj7mlrDkuabpgJrmiqU8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J0FkdmljZXNTY2FyY2guYXNweCc%2BPHNwYW4%2B5oOF5oql5qOA57SiPC9zcGFuPjwvQT48L3RkPjx0ZCBzdHlsZT0iaGVpZ2h0OiAyMXB4Ij48QSBocmVmPSdDb21tZW5kTmV3Qm9va1NjYXJjaC5hc3B4Jz48c3Bhbj7mlrDkuablvoHorqI8L3NwYW4%2BPC9BPjwvdGQ%2BPHRkIHN0eWxlPSJoZWlnaHQ6IDIxcHgiPjxBIGhyZWY9J1JlYWRlckxvZ2luLmFzcHgnPjxzcGFuPuivu%2BiAheeZu%2BW9lTwvc3Bhbj48L0E%2BPC90ZD48dGQgc3R5bGU9ImhlaWdodDogMjFweCI%2BPEEgaHJlZj0nT25saW5lU3R1ZHkuYXNweCc%2BPHNwYW4%2B5Zyo57q%2F5ZKo6K%2BiL%2BWfueiurTwvc3Bhbj48L0E%2BPC90ZD5kZAIDD2QWBAICDw8WAh8ABTI8c3Bhbj7mrKLov47mgqg6R3Vlc3Qg6K%2B36YCJ5oup5L2g55qE5pON5L2cPC9zcGFuPmRkAgMPDxYCHgdWaXNpYmxlaGRkAgUPZBYCAgEPZBYCAgMPZBYCZg8QZBAVAwzlgJ%2Fkuabor4Hlj7cM6K%2B76ICF5p2h56CBBuWnk%2BWQjRUDDOWAn%2BS5puivgeWPtwzor7vogIXmnaHnoIEG5aeT5ZCNFCsDA2dnZ2RkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQxJbWFnZUJ1dHRvbjGsgYTxIJS3IjlmswA%2BQmiST1GrfQ%3D%3D&DropDownList1=%E5%80%9F%E4%B9%A6%E8%AF%81%E5%8F%B7&TextBox1=20123025&TextBox2=&__EVENTVALIDATION=%2FwEWCAKmvqvMBwLgnZ70BALrr%2BCHBALntNySDgLwuLirBQLs0bLrBgLs0fbZDALSwpnTCIb1zlqPjvSF0veYooewNIwAiNJZ&ImageButton1.x=23&ImageButton1.y=17"));
	
	//姓名抓取
	int start = m_response.Find("\"LblreaderName\">");
	start = start + 16;
	int end = m_response.Find("</span>", start);
	CString name = m_response.Mid(start, end - start);
	//学院抓取
	start = m_response.Find("\"LblReaderUnit\">");
	start = start + 16;
	end = m_response.Find("</span>", start);
	CString unit = m_response.Mid(start, end - start);

	CWnd *pWnd = GetDlgItem(IDC_EDIT2);
	pWnd->SetWindowTextA(m_response);
}
