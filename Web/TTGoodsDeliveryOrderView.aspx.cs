using NickLee.Views.Web.UI;
using NickLee.Views.Windows.Forms.Printing;

using ProjectMgt.BLL;
using ProjectMgt.DAL;
using ProjectMgt.Model;

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class TTGoodsDeliveryOrderView : System.Web.UI.Page
{
    string strDOID;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strUserCode = Session["UserCode"].ToString();

        strDOID = Request.QueryString["DOID"];

        if (Page.IsPostBack != true)
        {
            LoadGoodsDeliveryOrder(strDOID);
            LoadGoodsDeliveryOrderDetail(strDOID);
        }
    }

    protected void LoadGoodsDeliveryOrder(string strDOID)
    {
        string strHQL;
        IList lst;

        strHQL = "from GoodsDeliveryOrder as goodsDeliveryOrder where goodsDeliveryOrder.DOID = " + strDOID;
        GoodsDeliveryOrderBLL goodsDeliveryOrderBLL = new GoodsDeliveryOrderBLL();
        lst = goodsDeliveryOrderBLL.GetAllGoodsDeliveryOrders(strHQL);

        GoodsDeliveryOrder goodsDeliveryOrder = (GoodsDeliveryOrder)lst[0];

        DataList20.DataSource = lst;
        DataList20.DataBind();

        Img_BarCode.ImageUrl = ShareClass. GenerateQrCodeImage(ShareClass.GetBarType(),goodsDeliveryOrder.DOName.Trim(), 200, 200);

    }

    public static string GenerateQrCodeImage(string strBarType, string strQrCodeString, int intWidth, int intHight)
    {
        string strImageUrl;

        try
        {
            try
            {
                System.Drawing.Bitmap imgTemp;

                if (strBarType == "NoLogoQrCode")
                {
                    //不带图二维码
                    imgTemp = BarcodeHelper.GenerateNoLogoQrCode(strQrCodeString, intWidth, intHight);
                }
                else if (strBarType == "HaveLogoQrCode")
                {
                    //带图二维码
                    imgTemp = BarcodeHelper.GenerateHaveLogoQrCode(strQrCodeString, intWidth, intHight);
                }
                else if (strBarType == "BarCode")
                {
                    //条形码
                    imgTemp = BarcodeHelper.GenerateBarCode(strQrCodeString, 260, 50);
                }
                else
                {
                    return "";
                }

                ////带图二维码
                //System.Drawing.Bitmap imgTemp = BarcodeHelper.GenerateHaveLogoQrCode(strQrCodeString, 240, 240);

                string strFileName = strQrCodeString + "BarCode" + DateTime.Now.ToString("yyyyMMddHHmmsssssfffffff") + ".gif";
                string strDocSavePath = HttpContext.Current.Server.MapPath("Doc") + "\\Bar\\";
                string strUrl = strDocSavePath + strFileName;

                if (Directory.Exists(strDocSavePath) == false)
                {
                    //如果不存在就创建file文件夹{
                    Directory.CreateDirectory(strDocSavePath);
                }

                imgTemp.Save(strUrl, System.Drawing.Imaging.ImageFormat.Gif);

                strImageUrl = "Doc/Bar/" + strFileName;

                return strImageUrl;
            }
            catch
            {
                return "";
            }
        }
        catch
        {
            return "";
        }
    }

    protected void LoadGoodsDeliveryOrderDetail(string strDOID)
    {
        string strHQL = "Select * from T_GoodsDeliveryOrderDetail where DOID = " + strDOID + " Order by ID DESC";

        DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_GoodsDeliveryOrder");
        DataGrid1.DataSource = ds;
        DataGrid1.DataBind();
    }



}