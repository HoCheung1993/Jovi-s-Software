using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			object dcode = "19930807";
			object dcode2 = "1450250506";
			int length = dcode.ToString().Length;
			byte[] b;
			int result;
			string tmpstr;
			string dcode1 = "";
			dcode2 = Convert.ToInt64(dcode2) * 137;
			for (int i = 1; i <= length; i++)
			{
				tmpstr = dcode.ToString().Substring(i - 1, 1);
				b = System.Text.Encoding.Unicode.GetBytes(tmpstr.Substring(0, 1));
				result = Convert.ToInt32(string.Format("{0}", b[0])) - i - Convert.ToInt32(dcode2.ToString().Substring(i - 1, 1));
				dcode1 += (char)result;
				Console.WriteLine(result.ToString());
				Console.WriteLine(dcode1);
				Console.ReadKey();
			}
			    dcode1 = dcode1.Replace("%", "%25");
			    dcode1 = dcode1.Replace("!", "%21");
				dcode1 = dcode1.Replace("#", "%23");
				dcode1 = dcode1.Replace("$", "%24");
				dcode1 = dcode1.Replace("&", "%26");
				dcode1 = dcode1.Replace("'", "%27");
				dcode1 = dcode1.Replace("(", "%28");
				dcode1 = dcode1.Replace(")", "%29");
				//dcode1 = dcode1.Replace("*", "%2A");
				dcode1 = dcode1.Replace("+", "%2B");
				dcode1 = dcode1.Replace(",", "%2C");
				dcode1 = dcode1.Replace("/", "%2F");
				dcode1 = dcode1.Replace(":", "%3A");
				dcode1 = dcode1.Replace(";", "%3B");
				dcode1 = dcode1.Replace("=", "%3D");
				dcode1 = dcode1.Replace("?", "%3F");
				dcode1 = dcode1.Replace("@", "%40");
				dcode1 = dcode1.Replace("[", "%5B");
				dcode1 = dcode1.Replace("]", "%5D");
				Console.WriteLine(dcode1);
				Console.ReadKey();
		}
	}
}
