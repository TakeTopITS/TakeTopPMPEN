using System; using System.Resources;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;

public partial class TTCheckCode : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       this.CreateCheckCodeImage(GenerateCheckCode());
    }

    private string GenerateCheckCode()
    {
       int number;
       char code;
       string checkCode = String.Empty;

       System.Random random = new Random();

       for(int i=0; i<5; i++)
       {
        number = random.Next();

        if(number % 2 == 0)
         code = (char)('0' + (char)(number % 10));
        else
         code = (char)('A' + (char)(number % 26));

        checkCode += code.ToString();
       }

       Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));

       return checkCode;
    }

    private void CreateCheckCodeImage(string checkCode)
    {
       if(checkCode == null || checkCode.Trim() == String.Empty)
        return;

       System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
       Graphics g = Graphics.FromImage(image);

       try
       {
        //�������������
        Random random = new Random();

        //���ͼƬ����ɫ
        g.Clear(Color.White);

        //��ͼƬ�ı���������
        for(int i=0; i<25; i++)
        {
         int x1 = random.Next(image.Width);
         int x2 = random.Next(image.Width);
         int y1 = random.Next(image.Height);
         int y2 = random.Next(image.Height);

         g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        }

        Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
        System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
        g.DrawString(checkCode, font, brush, 2, 2);

        //��ͼƬ��ǰ��������
        for(int i=0; i<100; i++)
        {
         int x = random.Next(image.Width);
         int y = random.Next(image.Height);

         image.SetPixel(x, y, Color.FromArgb(random.Next()));
        }

        //��ͼƬ�ı߿���
        g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        Response.ClearContent();
        Response.ContentType = "image/Gif";
        Response.BinaryWrite(ms.ToArray());
       }
       finally
       {
        g.Dispose();
        image.Dispose();
       }
     }
 } 


