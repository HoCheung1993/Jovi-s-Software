using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;

namespace 空课表生成工具
{
	class RowsAndColumns  //存储行列
	{
		private int rows;
		private int columns;

		public int Rows
		{
			get
			{
				return rows;
			}
			set
			{
				rows = value;
			}
		}

		public int Columns
		{
			get
			{
				return columns;
			}
			set
			{
				columns = value;
			}
		}

		public RowsAndColumns()  //构造
		{
			rows = -1;
			columns = -1;
		}
	}

	class BLL
	{
		public static string GetStrCon(string Filename) //获取连接字符串
		{
			return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Filename + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=0'";
		}

		public static bool CheckRepeatItem(ListBox lb, object item)  //检查item是否重复,重复返回true
		{
			for (int i = 0; i < lb.Items.Count; i++)
			{
				if (lb.Items[i].ToString().ToLower() == item.ToString().ToLower())
				{
					lb.SetSelected(i, true);  //重复选定Item
					return true;
				}
			}
			return false;
		}

		public static string GetName(string FilePath)//处理路径名
		{
			int i;
			for (i = FilePath.Length - 2; i >= 0 && FilePath[i] != '\\'; i--)
				;
			int startPosation = i + 1;
			int endPosation = FilePath.IndexOf(".");
			int length = endPosation - startPosation;
			return FilePath.Substring(startPosation, length);
		}

		public static void ShowExcel(DataGridView dgv, object item)  //显示表格
		{
				string strCon = BLL.GetStrCon(item.ToString());
				OleDbConnection myConn = new OleDbConnection(strCon);
				string strCom = "SELECT * FROM [Sheet1$]";
				myConn.Open();
				OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(strCom, myConn);
				DataSet myDataSet = new DataSet();
				myDataAdapter.Fill(myDataSet, "[Sheet1$]");
				myConn.Close();
				DataTable dt = myDataSet.Tables[0]; //初始化DataTable实例
				dgv.DataSource = myDataSet.Tables[0].DefaultView; //显示到datagridview
		}

		public static RowsAndColumns GetRowsAndColumns(DataGridView dgv, string CellString)  //获取字段所在的行列
		{
			RowsAndColumns RC = new RowsAndColumns();
			for (int i = 0; i < dgv.RowCount; i++)
			{
				for (int j = 0; j < dgv.ColumnCount; j++)
					if (dgv.Rows[i].Cells[j].Value != null)
						if (dgv.Rows[i].Cells[j].Value.ToString() == CellString)
						{
							RC.Rows = i;
							RC.Columns = j;
							return RC;
						}
			}
			return RC;
		}

		public static DataGridView FillDayOrTime(DataGridView dgv, DataGridView dgv2, params string[] DayOrTime)  //填充星期或者时间
		{
			try
			{
				for (int num = 0; num < DayOrTime.Length; num++)
				{
					RowsAndColumns RC = GetRowsAndColumns(dgv, DayOrTime[num]);
					int Row = RC.Rows + 1;
					int Columns = RC.Columns;
					dgv2.Rows[Row].Cells[Columns].Style.BackColor = Color.Yellow;
					dgv2.Rows[Row].Cells[Columns].Value = DayOrTime[num];   //将时间填入新的dgv2
				}
				return dgv2;
			}
			catch
			{
				MessageBox.Show("制作不完整，请参照帮助，检查格式！", "警告 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return dgv2;
			}
		}

		public static DataGridView MakeEmptyTimeTable(DataGridView dgv, DataGridView dgv2, string Name, params string[] Time)  //获取时间段对应的空白单元格
		{
			for (int num = 0; num < Time.Length; num++)
			{
				int Row;
				int Columns;
				string OldNameString = "";
				RowsAndColumns RC = GetRowsAndColumns(dgv, Time[num]);
				RowsAndColumns RC2 = GetRowsAndColumns(dgv2, Time[num]);
				Row = RC2.Rows;
				Columns = RC2.Columns;
				for (int j = Columns + 1; j < dgv.ColumnCount; j++)
					if (dgv.Rows[Row].Cells[j].Value.ToString() == string.Empty)
					{
						if (dgv2.Rows[Row].Cells[j].Value == null)  //第一次时候进行排版
						{
							dgv2.Rows[Row].Cells[j].Value = Name;
						}
						else
						{
							OldNameString = dgv2.Rows[Row].Cells[j].Value.ToString();
							OldNameString = OldNameString + "," + Name;
							dgv2.Rows[Row].Cells[j].Value = OldNameString;
						}
					}
			}
			return dgv2;
		}

		public static DataGridView ClearNull(DataGridView dgv2)   //清除空课表当中的Null行
		{
			for (int i = 0; i < dgv2.RowCount - 1; i++)
				for (int j = 0; j < dgv2.ColumnCount; j++)
				{
					if (dgv2.Rows[i].Cells[j].Value != null)
						break;
					else
						if (j == dgv2.ColumnCount - 1)
							dgv2.Rows.Remove(dgv2.Rows[i]);
				}
			return dgv2;
		}

		public static DataGridView PaintColumn(DataGridView dgv2, string HeaderCellString, string BottomCellString, Color color) //对列进行上色
		{
			RowsAndColumns RC1 = new RowsAndColumns();
			RowsAndColumns RC2 = new RowsAndColumns();
			RC1 = GetRowsAndColumns(dgv2, HeaderCellString);
			RC2 = GetRowsAndColumns(dgv2, BottomCellString);
			for (int i = RC1.Rows; i <= RC2.Rows; i++)
				for (int j = RC1.Columns; j <= RC2.Columns; j++)
					dgv2.Rows[i].Cells[j].Style.BackColor = color;
			return dgv2;
		}

		public static DataGridView PaintCell(DataGridView dgv2, Color color, params string[] CellString) //给特定单元格上色
		{
			for (int num = 0; num < CellString.Length; num++)
			{
				RowsAndColumns RC = new RowsAndColumns();
				RC = GetRowsAndColumns(dgv2, CellString[num]);
				if (RC.Rows != -1 && RC.Columns != -1)
					dgv2.Rows[RC.Rows].Cells[RC.Columns].Style.BackColor = color;
			}
			return dgv2;
		}

	}

	class ExportExcel
	{
		public static void ExcelSave(DataGridView dgv)
		{
			Microsoft.Office.Interop.Excel.Application myExcelApp = null;
			string saveFileName = string.Empty;
			try
			{
				if (dgv.RowCount <= 0)
				{
					MessageBox.Show("缺少可以导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				saveFileName = string.Empty;
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.DefaultExt = "xls";
				sfd.Filter = "Excel文件|*.xls";
				sfd.FileName = "空课表";
				if (sfd.ShowDialog() == DialogResult.OK)
					saveFileName = sfd.FileName;
				else
					throw new Exception("用户取消操作");
				if (saveFileName.IndexOf(".") < 0)
				{
					return;
				}
				myExcelApp = new Microsoft.Office.Interop.Excel.Application();
				if (myExcelApp == null)
				{
					MessageBox.Show("无法创建Excel，可能您未安装Excel", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				Microsoft.Office.Interop.Excel.Workbooks workbooks = myExcelApp.Workbooks;
				Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
				Microsoft.Office.Interop.Excel.Sheets sheets = workbook.Worksheets;
				Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);
				Microsoft.Office.Interop.Excel.Range range;
				object oMis = System.Reflection.Missing.Value;
				//显示为文本格式
				range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dgv.RowCount + 1, dgv.ColumnCount]);
				range.NumberFormatLocal = "@";
				//读入数据
				for (int i = 0; i < dgv.ColumnCount; i++)
				{
					worksheet.Cells[1, i + 1] = dgv.Columns[i].HeaderText.ToString().Trim();
				}
				for (int r = 0; r < dgv.RowCount; r++)
				{
					for (int i = 0; i < dgv.ColumnCount; i++)
					{
						if (dgv.Rows[r].Cells[i].Value != null)
							worksheet.Cells[r + 2, i + 1] = dgv.Rows[r].Cells[i].Value.ToString().Trim();
					}
				}
				range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dgv.RowCount + 1, dgv.ColumnCount]);
				range.Columns.AutoFit();
				range.RowHeight = 18;
				range.ColumnWidth = 15;
				range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
				//保存
				if (saveFileName != string.Empty)
				{
					try
					{
						workbook.Saved = true;
						workbook.SaveCopyAs(saveFileName);
					}
					catch (Exception ex)
					{
						MessageBox.Show("导出文件时出错，文件可能正被打开！\n" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					MessageBox.Show("文件名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch
			{
				return;
			}
			finally
			{
				myExcelApp.Visible = false;
				myExcelApp.Quit();
				GC.Collect();//垃圾回收
			}
			MessageBox.Show(saveFileName + "\n\n导出成功！ ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}


	class DataGridViewPrinter
	{
		// The DataGridView Control which will be printed
		private DataGridView TheDataGridView;
		// The PrintDocument to be used for printing
		private PrintDocument ThePrintDocument;
		// Determine if the report will be
		// printed in the Top-Center of the page
		private bool IsCenterOnPage;
		// Determine if the page contain title text
		private bool IsWithTitle;
		// The title text to be printed
		// in each page (if IsWithTitle is set to true)
		private string TheTitleText;
		// The font to be used with the title
		// text (if IsWithTitle is set to true)
		private Font TheTitleFont;
		// The color to be used with the title
		// text (if IsWithTitle is set to true)
		private Color TheTitleColor;
		// Determine if paging is used
		private bool IsWithPaging;

		// A static parameter that keep track
		// on which Row (in the DataGridView control)
		// that should be printed
		static int CurrentRow;

		static int PageNumber;

		private int PageWidth;
		private int PageHeight;
		private int LeftMargin;
		private int TopMargin;
		private int RightMargin;
		private int BottomMargin;

		// A parameter that keep track
		// on the y coordinate of the page,
		// so the next object to be printed
		// will start from this y coordinate
		private float CurrentY;

		private float RowHeaderHeight;
		private List<float> RowsHeight;
		private List<float> ColumnsWidth;
		private float TheDataGridViewWidth;

		// Maintain a generic list to hold start/stop
		// points for the column printing
		// This will be used for wrapping
		// in situations where the DataGridView will not
		// fit on a single page
		private List<int[]> mColumnPoints;
		private List<float> mColumnPointsWidth;
		private int mColumnPoint;

		// The class constructor
		public DataGridViewPrinter(DataGridView aDataGridView,
		PrintDocument aPrintDocument,
		bool CenterOnPage, bool WithTitle,
		string aTitleText, Font aTitleFont,
		Color aTitleColor, bool WithPaging)
		{
			TheDataGridView = aDataGridView;
			ThePrintDocument = aPrintDocument;
			IsCenterOnPage = CenterOnPage;
			IsWithTitle = WithTitle;
			TheTitleText = aTitleText;
			TheTitleFont = aTitleFont;
			TheTitleColor = aTitleColor;
			IsWithPaging = WithPaging;

			PageNumber = 0;

			RowsHeight = new List<float>();
			ColumnsWidth = new List<float>();

			mColumnPoints = new List<int[]>();
			mColumnPointsWidth = new List<float>();

			// Claculating the PageWidth and the PageHeight
			if (!ThePrintDocument.DefaultPageSettings.Landscape)
			{
				PageWidth =
				  ThePrintDocument.DefaultPageSettings.PaperSize.Width;
				PageHeight =
				  ThePrintDocument.DefaultPageSettings.PaperSize.Height;
			}
			else
			{
				PageHeight =
				  ThePrintDocument.DefaultPageSettings.PaperSize.Width;
				PageWidth =
				  ThePrintDocument.DefaultPageSettings.PaperSize.Height;
			}

			// Claculating the page margins
			LeftMargin = ThePrintDocument.DefaultPageSettings.Margins.Left;
			TopMargin = ThePrintDocument.DefaultPageSettings.Margins.Top;
			RightMargin = ThePrintDocument.DefaultPageSettings.Margins.Right;
			BottomMargin = ThePrintDocument.DefaultPageSettings.Margins.Bottom;

			// First, the current row to be printed
			// is the first row in the DataGridView control
			CurrentRow = 0;
		}

		// The function that calculate
		// the height of each row (including the header row),
		// the width of each column (according
		// to the longest text in all its cells including
		// the header cell), and the whole DataGridView width
		private void Calculate(Graphics g)
		{
			if (PageNumber == 0)
			// Just calculate once
			{
				SizeF tmpSize = new SizeF();
				Font tmpFont;
				float tmpWidth;

				TheDataGridViewWidth = 0;
				for (int i = 0; i < TheDataGridView.Columns.Count; i++)
				{
					tmpFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
					if (tmpFont == null)
						// If there is no special HeaderFont style,
						// then use the default DataGridView font style
						tmpFont = TheDataGridView.DefaultCellStyle.Font;

					tmpSize = g.MeasureString(
							  TheDataGridView.Columns[i].HeaderText,
							  tmpFont);
					tmpWidth = tmpSize.Width;
					RowHeaderHeight = tmpSize.Height;

					for (int j = 0; j < TheDataGridView.Rows.Count; j++)
					{
						tmpFont = TheDataGridView.Rows[j].DefaultCellStyle.Font;
						if (tmpFont == null)
							// If the there is no special font style of the
							// CurrentRow, then use the default one associated
							// with the DataGridView control
							tmpFont = TheDataGridView.DefaultCellStyle.Font;

						tmpSize = g.MeasureString("Anything", tmpFont);
						RowsHeight.Add(tmpSize.Height);

						tmpSize =
							g.MeasureString(
							TheDataGridView.Rows[j].Cells[i].
									 EditedFormattedValue.ToString(),
							tmpFont);
						if (tmpSize.Width > tmpWidth)
							tmpWidth = tmpSize.Width;
					}
					if (TheDataGridView.Columns[i].Visible)
						TheDataGridViewWidth += tmpWidth;
					ColumnsWidth.Add(tmpWidth);
				}

				// Define the start/stop column points
				// based on the page width and
				// the DataGridView Width
				// We will use this to determine
				// the columns which are drawn on each page
				// and how wrapping will be handled
				// By default, the wrapping will occurr
				// such that the maximum number of
				// columns for a page will be determine
				int k;

				int mStartPoint = 0;
				for (k = 0; k < TheDataGridView.Columns.Count; k++)
					if (TheDataGridView.Columns[k].Visible)
					{
						mStartPoint = k;
						break;
					}

				int mEndPoint = TheDataGridView.Columns.Count;
				for (k = TheDataGridView.Columns.Count - 1; k >= 0; k--)
					if (TheDataGridView.Columns[k].Visible)
					{
						mEndPoint = k + 1;
						break;
					}

				float mTempWidth = TheDataGridViewWidth;
				float mTempPrintArea = (float)PageWidth - (float)LeftMargin -
					(float)RightMargin;

				// We only care about handling
				// where the total datagridview width is bigger
				// then the print area
				if (TheDataGridViewWidth > mTempPrintArea)
				{
					mTempWidth = 0.0F;
					for (k = 0; k < TheDataGridView.Columns.Count; k++)
					{
						if (TheDataGridView.Columns[k].Visible)
						{
							mTempWidth += ColumnsWidth[k];
							// If the width is bigger
							// than the page area, then define a new
							// column print range
							if (mTempWidth > mTempPrintArea)
							{
								mTempWidth -= ColumnsWidth[k];
								mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
								mColumnPointsWidth.Add(mTempWidth);
								mStartPoint = k;
								mTempWidth = ColumnsWidth[k];
							}
						}
						// Our end point is actually
						// one index above the current index
						mEndPoint = k + 1;
					}
				}
				// Add the last set of columns
				mColumnPoints.Add(new int[] { mStartPoint, mEndPoint });
				mColumnPointsWidth.Add(mTempWidth);
				mColumnPoint = 0;
			}
		}

		// The funtion that print the title, page number, and the header row
		private void DrawHeader(Graphics g)
		{
			CurrentY = (float)TopMargin;

			// Printing the page number (if isWithPaging is set to true)
			if (IsWithPaging)
			{
				PageNumber++;
				string PageString = "Page " + PageNumber.ToString();

				StringFormat PageStringFormat = new StringFormat();
				PageStringFormat.Trimming = StringTrimming.Word;
				PageStringFormat.FormatFlags = StringFormatFlags.NoWrap |
					StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
				PageStringFormat.Alignment = StringAlignment.Far;

				Font PageStringFont = new Font("Tahoma", 8, FontStyle.Regular,
					GraphicsUnit.Point);

				RectangleF PageStringRectangle =
				   new RectangleF((float)LeftMargin, CurrentY,
				   (float)PageWidth - (float)RightMargin - (float)LeftMargin,
				   g.MeasureString(PageString, PageStringFont).Height);

				g.DrawString(PageString, PageStringFont,
				   new SolidBrush(Color.Black),
				   PageStringRectangle, PageStringFormat);

				CurrentY += g.MeasureString(PageString,
									 PageStringFont).Height;
			}

			// Printing the title (if IsWithTitle is set to true)
			if (IsWithTitle)
			{
				StringFormat TitleFormat = new StringFormat();
				TitleFormat.Trimming = StringTrimming.Word;
				TitleFormat.FormatFlags = StringFormatFlags.NoWrap |
					StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
				if (IsCenterOnPage)
					TitleFormat.Alignment = StringAlignment.Center;
				else
					TitleFormat.Alignment = StringAlignment.Near;

				RectangleF TitleRectangle =
					new RectangleF((float)LeftMargin, CurrentY,
					(float)PageWidth - (float)RightMargin - (float)LeftMargin,
					g.MeasureString(TheTitleText, TheTitleFont).Height);

				g.DrawString(TheTitleText, TheTitleFont,
					new SolidBrush(TheTitleColor),
					TitleRectangle, TitleFormat);

				CurrentY += g.MeasureString(TheTitleText, TheTitleFont).Height;
			}

			// Calculating the starting x coordinate
			// that the printing process will start from
			float CurrentX = (float)LeftMargin;
			if (IsCenterOnPage)
				CurrentX += (((float)PageWidth - (float)RightMargin -
				  (float)LeftMargin) - mColumnPointsWidth[mColumnPoint]) / 2.0F;

			// Setting the HeaderFore style
			Color HeaderForeColor =
				  TheDataGridView.ColumnHeadersDefaultCellStyle.ForeColor;
			if (HeaderForeColor.IsEmpty)
				// If there is no special HeaderFore style,
				// then use the default DataGridView style
				HeaderForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
			SolidBrush HeaderForeBrush = new SolidBrush(HeaderForeColor);

			// Setting the HeaderBack style
			Color HeaderBackColor =
				  TheDataGridView.ColumnHeadersDefaultCellStyle.BackColor;
			if (HeaderBackColor.IsEmpty)
				// If there is no special HeaderBack style,
				// then use the default DataGridView style
				HeaderBackColor = TheDataGridView.DefaultCellStyle.BackColor;
			SolidBrush HeaderBackBrush = new SolidBrush(HeaderBackColor);

			// Setting the LinePen that will
			// be used to draw lines and rectangles
			// (derived from the GridColor property
			// of the DataGridView control)
			Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

			// Setting the HeaderFont style
			Font HeaderFont = TheDataGridView.ColumnHeadersDefaultCellStyle.Font;
			if (HeaderFont == null)
				// If there is no special HeaderFont style,
				// then use the default DataGridView font style
				HeaderFont = TheDataGridView.DefaultCellStyle.Font;

			// Calculating and drawing the HeaderBounds        
			RectangleF HeaderBounds = new RectangleF(CurrentX, CurrentY,
				mColumnPointsWidth[mColumnPoint], RowHeaderHeight);
			g.FillRectangle(HeaderBackBrush, HeaderBounds);

			// Setting the format that will be
			// used to print each cell of the header row
			StringFormat CellFormat = new StringFormat();
			CellFormat.Trimming = StringTrimming.Word;
			CellFormat.FormatFlags = StringFormatFlags.NoWrap |
			   StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

			// Printing each visible cell of the header row
			RectangleF CellBounds;
			float ColumnWidth;
			for (int i = (int)mColumnPoints[mColumnPoint].GetValue(0);
				i < (int)mColumnPoints[mColumnPoint].GetValue(1); i++)
			{
				// If the column is not visible then ignore this iteration
				if (!TheDataGridView.Columns[i].Visible)
					continue;

				ColumnWidth = ColumnsWidth[i];

				// Check the CurrentCell alignment
				// and apply it to the CellFormat
				if (TheDataGridView.ColumnHeadersDefaultCellStyle.
						   Alignment.ToString().Contains("Right"))
					CellFormat.Alignment = StringAlignment.Far;
				else if (TheDataGridView.ColumnHeadersDefaultCellStyle.
						 Alignment.ToString().Contains("Center"))
					CellFormat.Alignment = StringAlignment.Center;
				else
					CellFormat.Alignment = StringAlignment.Near;

				CellBounds = new RectangleF(CurrentX, CurrentY,
							 ColumnWidth, RowHeaderHeight);

				// Printing the cell text
				g.DrawString(TheDataGridView.Columns[i].HeaderText,
							 HeaderFont, HeaderForeBrush,
				   CellBounds, CellFormat);

				// Drawing the cell bounds
				// Draw the cell border only if the HeaderBorderStyle is not None
				if (TheDataGridView.RowHeadersBorderStyle !=
								DataGridViewHeaderBorderStyle.None)
					g.DrawRectangle(TheLinePen, CurrentX, CurrentY, ColumnWidth,
						RowHeaderHeight);

				CurrentX += ColumnWidth;
			}

			CurrentY += RowHeaderHeight;
		}

		// The function that print a bunch of rows that fit in one page
		// When it returns true, meaning that
		// there are more rows still not printed,
		// so another PagePrint action is required
		// When it returns false, meaning that all rows are printed
		// (the CureentRow parameter reaches
		// the last row of the DataGridView control)
		// and no further PagePrint action is required
		private bool DrawRows(Graphics g)
		{
			// Setting the LinePen that will be used to draw lines and rectangles
			// (derived from the GridColor property of the DataGridView control)
			Pen TheLinePen = new Pen(TheDataGridView.GridColor, 1);

			// The style paramters that will be used to print each cell
			Font RowFont;
			Color RowForeColor;
			Color RowBackColor;
			SolidBrush RowForeBrush;
			SolidBrush RowBackBrush;
			SolidBrush RowAlternatingBackBrush;

			// Setting the format that will be used to print each cell
			StringFormat CellFormat = new StringFormat();
			CellFormat.Trimming = StringTrimming.Word;
			CellFormat.FormatFlags = StringFormatFlags.NoWrap |
									 StringFormatFlags.LineLimit;

			// Printing each visible cell
			RectangleF RowBounds;
			float CurrentX;
			float ColumnWidth;
			while (CurrentRow < TheDataGridView.Rows.Count)
			{
				// Print the cells of the CurrentRow only if that row is visible
				if (TheDataGridView.Rows[CurrentRow].Visible)
				{
					// Setting the row font style
					RowFont = TheDataGridView.Rows[CurrentRow].DefaultCellStyle.Font;
					// If the there is no special font style of the CurrentRow,
					// then use the default one associated with the DataGridView control
					if (RowFont == null)
						RowFont = TheDataGridView.DefaultCellStyle.Font;

					// Setting the RowFore style
					RowForeColor =
					  TheDataGridView.Rows[CurrentRow].DefaultCellStyle.ForeColor;
					// If the there is no special RowFore style of the CurrentRow,
					// then use the default one associated with the DataGridView control
					if (RowForeColor.IsEmpty)
						RowForeColor = TheDataGridView.DefaultCellStyle.ForeColor;
					RowForeBrush = new SolidBrush(RowForeColor);

					// Setting the RowBack (for even rows) and the RowAlternatingBack
					// (for odd rows) styles
					RowBackColor =
					  TheDataGridView.Rows[CurrentRow].DefaultCellStyle.BackColor;
					// If the there is no special RowBack style of the CurrentRow,
					// then use the default one associated with the DataGridView control
					if (RowBackColor.IsEmpty)
					{
						RowBackBrush = new SolidBrush(
							  TheDataGridView.DefaultCellStyle.BackColor);
						RowAlternatingBackBrush = new
							SolidBrush(
							TheDataGridView.AlternatingRowsDefaultCellStyle.BackColor);
					}
					// If the there is a special RowBack style of the CurrentRow,
					// then use it for both the RowBack and the RowAlternatingBack styles
					else
					{
						RowBackBrush = new SolidBrush(RowBackColor);
						RowAlternatingBackBrush = new SolidBrush(RowBackColor);
					}

					// Calculating the starting x coordinate
					// that the printing process will
					// start from
					CurrentX = (float)LeftMargin;
					if (IsCenterOnPage)
						CurrentX += (((float)PageWidth - (float)RightMargin -
							(float)LeftMargin) -
							mColumnPointsWidth[mColumnPoint]) / 2.0F;

					// Calculating the entire CurrentRow bounds                
					RowBounds = new RectangleF(CurrentX, CurrentY,
						mColumnPointsWidth[mColumnPoint], RowsHeight[CurrentRow]);

					// Filling the back of the CurrentRow
					if (CurrentRow % 2 == 0)
						g.FillRectangle(RowBackBrush, RowBounds);
					else
						g.FillRectangle(RowAlternatingBackBrush, RowBounds);

					// Printing each visible cell of the CurrentRow                
					for (int CurrentCell = (int)mColumnPoints[mColumnPoint].GetValue(0);
						CurrentCell < (int)mColumnPoints[mColumnPoint].GetValue(1);
						CurrentCell++)
					{
						// If the cell is belong to invisible
						// column, then ignore this iteration
						if (!TheDataGridView.Columns[CurrentCell].Visible)
							continue;

						// Check the CurrentCell alignment
						// and apply it to the CellFormat
						if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.
								Alignment.ToString().Contains("Right"))
							CellFormat.Alignment = StringAlignment.Far;
						else if (TheDataGridView.Columns[CurrentCell].DefaultCellStyle.
								Alignment.ToString().Contains("Center"))
							CellFormat.Alignment = StringAlignment.Center;
						else
							CellFormat.Alignment = StringAlignment.Near;

						ColumnWidth = ColumnsWidth[CurrentCell];
						RectangleF CellBounds = new RectangleF(CurrentX, CurrentY,
							ColumnWidth, RowsHeight[CurrentRow]);

						// Printing the cell text
						g.DrawString(
						  TheDataGridView.Rows[CurrentRow].Cells[CurrentCell].
						  EditedFormattedValue.ToString(), RowFont, RowForeBrush,
						  CellBounds, CellFormat);

						// Drawing the cell bounds
						// Draw the cell border only
						// if the CellBorderStyle is not None
						if (TheDataGridView.CellBorderStyle !=
									DataGridViewCellBorderStyle.None)
							g.DrawRectangle(TheLinePen, CurrentX, CurrentY,
								  ColumnWidth, RowsHeight[CurrentRow]);

						CurrentX += ColumnWidth;
					}
					CurrentY += RowsHeight[CurrentRow];

					// Checking if the CurrentY is exceeds the page boundries
					// If so then exit the function and returning true meaning another
					// PagePrint action is required
					if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
					{
						CurrentRow++;
						return true;
					}
				}
				CurrentRow++;
			}

			CurrentRow = 0;
			// Continue to print the next group of columns
			mColumnPoint++;

			if (mColumnPoint == mColumnPoints.Count)
			// Which means all columns are printed
			{
				mColumnPoint = 0;
				return false;
			}
			else
				return true;
		}

		// The method that calls all other functions
		public bool DrawDataGridView(Graphics g)
		{
			try
			{
				Calculate(g);
				DrawHeader(g);
				bool bContinue = DrawRows(g);
				return bContinue;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Operation failed: " + ex.Message.ToString(),
					Application.ProductName + " - Error", MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return false;
			}
		}
	}
}