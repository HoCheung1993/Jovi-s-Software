#pragma once


// CSkinStatic

class CSkinStatic : public CStatic
{
	DECLARE_DYNAMIC(CSkinStatic)

public:
	CSkinStatic();
	virtual ~CSkinStatic();

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg HBRUSH CtlColor(CDC* /*pDC*/, UINT /*nCtlColor*/);
	void SetTextColor(COLORREF crText);
	void SetFontSize(int nSize);
	void SetBackColor(COLORREF crBackColor);
	void SetTransparent(BOOL bTran);
private:
	COLORREF m_crText;          // 字体颜色  
	COLORREF m_crBackColor;     // 背景颜色  
	HBRUSH   m_hBrush;          // 画刷  
	LOGFONT  m_lf;              // 字体大小 
	CFont*   m_pOldFont;        //保存系统字体
	CFont    m_font;            // 字体  
	BOOL     m_bTran;            // 是否透明  
};


