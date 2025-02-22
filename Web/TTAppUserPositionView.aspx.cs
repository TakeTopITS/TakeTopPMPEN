using System;
using System.Resources;
using System.Drawing;
using System.Data;

using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Xml;
using System.IO;
using System.Text;

using ProjectMgt.Model;
using ProjectMgt.DAL;
using ProjectMgt.BLL;

public partial class TTAppUserPositionView : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strLat, strLng;

        strLng = Request.QueryString["lng"];
        strLat = Request.QueryString["lat"];

        string strUserCode = Session["UserCode"].ToString();
        if (Page.IsPostBack != true)
        {
            LNG_value.Value = strLng;
            LAT_value.Value = strLat;
        }
    }



    private string GetAddressByBAIDU(string lng, string lat)
    {
        try
        {
            //string url = @"http://api.map.baidu.com/geocoder/v2/?ak=wqBXfIN3HkpM1AHKWujjCdsi&location=" + lat + "," + lng + @"&output=xml&pois=1";
            string url = @"http://api.map.baidu.com/geocoder/v2/?ak=r3oHIq6zgkF3BU9cXlgIQuMu&location=" + lat + "," + lng + @"&output=xml&pois=1";

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            XmlDocument xmlDoc = new XmlDocument();
            string sendData = xmlDoc.InnerXml;
            byte[] byteArray = Encoding.Default.GetBytes(sendData);

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("utf-8"));
            string responseXml = reader.ReadToEnd();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(responseXml);
            string status = xml.DocumentElement.SelectSingleNode("status").InnerText;
            if (status == "0")
            {

                XmlNodeList nodes = xml.DocumentElement.GetElementsByTagName("formatted_address");
                if (nodes.Count > 0)
                {
                    return nodes[0].InnerText;
                }
                else
                    return "Can not get position information,error code 3";
            }
            else
            {
                return "Can not get position information,error code 1";
            }
        }
        catch (System.Exception ex)
        {
            return "Can not get position information,error code 2";
        }
    }

    private string GetAddressByGOOGLE(string lng, string lat)
    {
        try
        {
            //webclient�ͻ��˶��� 
            WebClient client = new WebClient();
            string url = "http://maps.google.com/maps/api/geocode/xml?latlng=" + lng + "," + lat + "&language=zh-CN&sensor=false";//�����ַ 
            client.Encoding = Encoding.UTF8;//�����ʽ 
            string responseTest = client.DownloadString(url);
            //����xml��Ӧ���� 
            string address = "";//���صĵ�ַ 
            XmlDocument doc = new XmlDocument();
            //����XML�ĵ����� 
            if (!string.IsNullOrEmpty(responseTest))
            {
                doc.LoadXml(responseTest);//����xml�ַ��� 
                //��ѯ״̬��Ϣ 
                string xpath = @"GeocodeResponse/status";
                XmlNode node = doc.SelectSingleNode(xpath);
                string status = node.InnerText.ToString();
                if (status == "OK")
                {
                    //��ѯ��ϸ��ַ��Ϣ 
                    xpath = @"GeocodeResponse/result/formatted_address";
                    node = doc.SelectSingleNode(xpath);
                    address = node.InnerText.ToString();
                    //��ѯ������Ϣ 
                    XmlNodeList nodeListAll = doc.SelectNodes("GeocodeResponse/result");

                    XmlNode idt = nodeListAll[0];
                    XmlNodeList idts = idt.SelectNodes("address_component[type='sublocality']");
                    //address_component[type='sublocality']��ʾɸѡtype='sublocality'����������ӽڵ㣻 
                    XmlNode idtst = idts[0];

                    string area = idtst.SelectSingleNode("short_name").InnerText;
                    address = address + "," + area;

                    return address;
                }
                else
                {
                    return "Can not get position information,error code 2";
                }
            }
            else
            {
                return "Can not get position information,error code 2";
            }
        }
        catch (System.Exception ex)
        {
            return "Can not get position information,error code 2";
        }
    }

}