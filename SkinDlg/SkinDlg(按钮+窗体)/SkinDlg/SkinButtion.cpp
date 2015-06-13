// SkinButtion.cpp : 实现文件
//

#include "stdafx.h"
#include "SkinDlg.h"
#include "SkinButtion.h"


// CSkinButtion

IMPLEMENT_DYNAMIC(CSkinButton, CButton)

CSkinButton::CSkinButton()
{
	m_btninfoNor.pImg = m_btninfoPress.pImg = m_btninfoFocus.pImg = m_btninfoDis.pImg =NULL; //底图
	m_bFocus = m_bPress = FALSE;
	m_bAutoSize = TRUE;
}

CSkinButton::~CSkinButton()
{
	if (m_btninfoNor.pImg!=NULL)
	{
		delete m_btninfoNor.pImg;
		m_btninfoNor.pImg = NULL;
	}
	if (m_btninfoFocus.pImg != NULL)
	{
		delete m_btninfoFocus.pImg;
		m_btninfoFocus.pImg = NULL;
	}
	if (m_btninfoPress.pImg != NULL)
	{
		delete m_btninfoPress.pImg;
		m_btninfoPress.pImg = NULL;
	}
	if (m_btninfoDis.pImg != NULL)
	{
		delete m_btninfoDis.pImg;
		m_btninfoDis.pImg = NULL;
	}
}


BEGIN_MESSAGE_MAP(CSkinButton, CButton)
	ON_WM_LBUTTONUP()
	ON_WM_LBUTTONDOWN()
	ON_WM_MOUSEMOVE()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()



// CSkinButtion 消息处理程序

void CSkinButton::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct)
{
	ASSERT(lpDrawItemStruct->CtlType == ODT_BUTTON);
	CDC *pDC = CDC::FromHandle(lpDrawItemStruct->hDC);
	CRect rect = lpDrawItemStruct->rcItem;
	CString strText = _T("");
	GetWindowText(strText);

	if (lpDrawItemStruct->itemState&ODS_DISABLED)
	{
		DrawBK(*pDC, m_btninfoDis.pImg, SKIN_BTN_DIS);
	}
	else if (lpDrawItemStruct->itemState&ODS_SELECTED || (m_bFocus&&m_bPress))
	{
		DrawBK(*pDC, m_btninfoPress.pImg, SKIN_BTN_PRESS);
	}
	else if (m_bFocus)
	{
		DrawBK(*pDC, m_btninfoFocus.pImg, SKIN_BTN_FOCUS);
	}
	else
	{
		DrawBK(*pDC, m_btninfoNor.pImg, SKIN_BTN_NOR);
	}

	CString strTemp(strText);
	strTemp.Remove(' ');
	if (!strTemp.IsEmpty())
	{
		if (lpDrawItemStruct->itemState & ODS_DISABLED)
		{
			DrawBtnText(*pDC, strText, 0, SKIN_BTN_DIS);
		}
		else if (lpDrawItemStruct->itemState & ODS_SELECTED
			|| (m_bFocus && m_bPress))
		{
			DrawBtnText(*pDC, strText, 1, SKIN_BTN_PRESS);
		}
		else if (m_bFocus)
		{
			DrawBtnText(*pDC, strText, 0, SKIN_BTN_FOCUS);
		}
		else
		{
			DrawBtnText(*pDC, strText, 0, SKIN_BTN_NOR);
		}

	}
	// TODO:  添加您的代码以绘制指定项
}


void CSkinButton::OnLButtonUp(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	if (m_bPress)
	{
		CRect rect;
		GetClientRect(rect);
		Invalidate();
		CRect rcTemp(rect);
		ClientToScreen(&rcTemp);
		CPoint pointTemp(rcTemp.left, rcTemp.top);
		::ScreenToClient(GetParent()->GetSafeHwnd(), &pointTemp);
		rcTemp.SetRect(pointTemp.x, pointTemp.y, pointTemp.x + rect.Width(), pointTemp.y + rect.Height());
		GetParent()->InvalidateRect(rcTemp, TRUE);
		m_bPress = FALSE;
	}
	m_bPress = FALSE;
	CButton::OnLButtonUp(nFlags, point);
}


void CSkinButton::OnLButtonDown(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	CRect rect;
	GetClientRect(rect);
	CRect rcTemp(rect);
	ClientToScreen(&rcTemp);
	CPoint pointTemp(rcTemp.left, rcTemp.top);
	::ScreenToClient(GetParent()->GetSafeHwnd(), &pointTemp);
	rcTemp.SetRect(pointTemp.x, pointTemp.y, pointTemp.x + rect.Width(), pointTemp.y + rect.Height());
	GetParent()->InvalidateRect(rcTemp, TRUE);
	m_bPress = TRUE;
	CButton::OnLButtonDown(nFlags, point);
}


void CSkinButton::OnMouseMove(UINT nFlags, CPoint point)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	CRect rect;
	GetClientRect(rect);
	if (rect.PtInRect(point))
	{
		if (!(nFlags & MK_LBUTTON))
			m_bPress = FALSE;
		if (GetCapture() != this)
		{
			SetCapture();
		}
		if (m_bFocus == TRUE)
		{
			//
		}
		else
		{
			m_bFocus = TRUE;
			SetFocus();
			Invalidate();
			CRect rcTemp(rect);
			ClientToScreen(&rcTemp);
			CPoint pointTemp(rcTemp.left, rcTemp.top);
			::ScreenToClient(GetParent()->GetSafeHwnd(), &pointTemp);
			rcTemp.SetRect(pointTemp.x, pointTemp.y, pointTemp.x + rect.Width(), pointTemp.y + rect.Height());
			GetParent()->InvalidateRect(rcTemp, TRUE);
		}
	}
	else
	{
		ReleaseCapture();
		m_bFocus = FALSE;
		Invalidate();
		CRect rcTemp(rect);
		ClientToScreen(&rcTemp);
		CPoint pointTemp(rcTemp.left, rcTemp.top);
		::ScreenToClient(GetParent()->GetSafeHwnd(), &pointTemp);
		rcTemp.SetRect(pointTemp.x, pointTemp.y, pointTemp.x + rect.Width(), pointTemp.y + rect.Height());
		GetParent()->InvalidateRect(rcTemp, TRUE);
	}
	CButton::OnMouseMove(nFlags, point);
}


BOOL CSkinButton::OnEraseBkgnd(CDC* pDC)
{
	// TODO:  在此添加消息处理程序代码和/或调用默认值
	if (GetButtonStyle() & BS_OWNERDRAW)
		return TRUE;
	return CButton::OnEraseBkgnd(pDC);
}


void CSkinButton::PreSubclassWindow()
{
	// TODO:  在此添加专用代码和/或调用基类
	ModifyStyle(0, BS_OWNERDRAW);//设置自绘
	CButton::PreSubclassWindow();
}


//函数实现
void CSkinButton::Load(UINT IDBkGroup, int width , int height , const CString & resourceType )
{
	CImage orgImg;

	HINSTANCE hInst = AfxGetResourceHandle();
	HRSRC hRsrc = ::FindResource(hInst, MAKEINTRESOURCE(IDBkGroup), resourceType);
	if (hRsrc == NULL)
	{
		return;
	}

	//资源加载到内存
	DWORD len = SizeofResource(hInst, hRsrc);
	BYTE *lpRsrc = (BYTE *)LoadResource(hInst, hRsrc);
	if (lpRsrc == NULL)
	{
		return;
	}

	//为流申请资源
	HGLOBAL m_hMem = GlobalAlloc(GMEM_FIXED, len);
	BYTE* pMem = (BYTE*)GlobalLock(m_hMem);
	memcpy(pMem, lpRsrc, len);
	IStream* pstm;
	CreateStreamOnHGlobal(m_hMem, FALSE, &pstm);

	//加载流
	orgImg.Load(pstm);

	//释放
	GlobalUnlock(m_hMem);
	GlobalFree(m_hMem);
	pstm->Release();
	FreeResource(lpRsrc);

	if (resourceType == _T("PNG"))
	{
		if (orgImg.GetBPP() == 32) //PNG透明
		{
			for (int i = 0; i < orgImg.GetWidth(); i++)
			{
				for (int j = 0; j < orgImg.GetHeight(); j++)
				{
					BYTE * pucColor = (BYTE *)orgImg.GetPixelAddress(i, j);
					pucColor[0] = pucColor[0] * pucColor[3] / 255;
					pucColor[1] = pucColor[1] * pucColor[3] / 255;
					pucColor[2] = pucColor[2] * pucColor[3] / 255;
				}
			}
		}
	}
	if (width == 0)
	{
		width = orgImg.GetWidth();
	}
	if (height == 0)
	{
		height = orgImg.GetHeight();
	}

	m_btninfoNor.nHeight = height;
	m_btninfoNor.nWidth = width;
	m_btninfoPress.nHeight = height;
	m_btninfoPress.nWidth = width;
	m_btninfoFocus.nHeight = height;
	m_btninfoFocus.nWidth = width;
	m_btninfoDis.nHeight = height;
	m_btninfoDis.nWidth = width;

	CImage** imgs[] = { &m_btninfoNor.pImg, &m_btninfoFocus.pImg, &m_btninfoPress.pImg, &m_btninfoDis.pImg };
	int posX = 0;
	for (int i = 0; i < 4 && posX <= (orgImg.GetWidth() - width); i++, posX += width)
	{
		CImage* pMap = new CImage();
		if (*imgs[i] != NULL)
		{
			delete *imgs[i];
			*imgs[i] = NULL;
		}
		*imgs[i] = pMap;

		if (resourceType == _T("PNG"))
		{
			BOOL bStat = FALSE;
			if (orgImg.GetBPP() == 32)
			{
				bStat = pMap->CreateEx(width, height, orgImg.GetBPP(), BI_RGB, NULL, CImage::createAlphaChannel);
			}
			else
			{
				bStat = pMap->CreateEx(width, height, orgImg.GetBPP(), BI_RGB, NULL);
			}
			ASSERT(bStat);
		}
		else
		{
			BOOL bStat = pMap->CreateEx(width, height, orgImg.GetBPP(), BI_RGB, NULL);
			ASSERT(bStat);
		}

		CImageDC imageDC(*pMap);
		orgImg.Draw(imageDC, 0, 0, width, height, posX, 0, width, height);
	}
}


void CSkinButton::DrawBK(HDC dc, CImage *image, SKIN_BTNSTATE btnstate)
{
	if (!image)
		return;

	CRect rc;
	GetClientRect(&rc);
	CRect tmpRect;
	int nX = 0;
	int nY = 0;
	int nW = 0;
	int nH = 0;

	if (m_bAutoSize == TRUE)
	{
		tmpRect.SetRect(0, 0, rc.Width(), rc.Height());
		if (image)
			image->Draw(dc, tmpRect);
	}
	else
	{

		if (btnstate == SKIN_BTN_NOR)
		{
			nW = m_btninfoNor.nWidth;
			nH = m_btninfoNor.nHeight;

		}
		else if (btnstate == SKIN_BTN_FOCUS)
		{
			nW = m_btninfoFocus.nWidth;
			nH = m_btninfoFocus.nHeight;

		}
		else if (btnstate == SKIN_BTN_PRESS)
		{
			nW = m_btninfoPress.nWidth;
			nH = m_btninfoPress.nHeight;

		}
		else
		{
			nW = m_btninfoDis.nWidth;
			nH = m_btninfoDis.nHeight;
		}

		nX = (rc.Width() - nW) / 2;
		nY = (rc.Height() - nH) / 2;
		tmpRect.SetRect(nX, nY, nW + nX, nH + nY);
		if (image)
			image->Draw(dc, tmpRect);
	}
}


void CSkinButton::SetAutoSize(bool bAutoSize)
{
	m_bAutoSize = bAutoSize;
}


void CSkinButton::DrawBtnText(HDC dc, const CString & strText, int nMove, SKIN_BTNSTATE btnstate)
{
	CSize sizeText = CDC::FromHandle(dc)->GetTextExtent(strText);
	CRect rect;
	GetClientRect(&rect);

	rect.DeflateRect(nMove, nMove, 0, 0);
	CDC::FromHandle(dc)->SetBkMode(TRANSPARENT);

	if (btnstate == SKIN_BTN_NOR)
	{
		CDC::FromHandle(dc)->SetTextColor(RGB(30, 30, 30));
	}
	else if (btnstate == SKIN_BTN_FOCUS)
	{
		CDC::FromHandle(dc)->SetTextColor(RGB(30, 30, 30));
	}
	else if (btnstate == SKIN_BTN_PRESS)
	{
		CDC::FromHandle(dc)->SetTextColor(RGB(30, 30, 30));
	}
	else
	{
		CDC::FromHandle(dc)->SetTextColor(RGB(100, 100, 100));
	}
	CDC::FromHandle(dc)->DrawText(strText, rect, DT_SINGLELINE | DT_VCENTER | DT_CENTER);
}