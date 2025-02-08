using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TTAIHandlerByDeepSeek : System.Web.UI.Page
{
    protected async void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
          txtPrompt.Focus();
        }
    }

    protected async void btnGenerateText_Click(object sender, EventArgs e)
    {
        string localApiUrl, result;
        string strAIType, strAIURL;

        string strHQL;

        lblGeneratedText.Text = "";

        if (txtPrompt.Text.Trim() == "")
        {
            lblGeneratedText.Text = "Prompt can't be empty!";
            return;
        }

        try
        {
            strHQL = "Select AIType,URL,Model From T_AIInterface";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AIInterface");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strAIType = ds.Tables[0].Rows[0]["AIType"].ToString().Trim();
                strAIURL = ds.Tables[0].Rows[0]["URL"].ToString().Trim();

                if (strAIType == "Local")
                {
                    // DeepSeek 或 Ollama 的本地 API 地址
                    localApiUrl = strAIURL + "/api/generate"; // Ollama 的默认 API 地址

                    result = await CallLocalApi(localApiUrl);

                    // 显示结果
                    lblGeneratedText.Text = result; // 假设页面上有一个 Label 控件
                }
                else
                {
                    localApiUrl = strAIURL;
                    ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "window.open('" + strAIURL + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "ajustHeight", "window.open('https://www.deepseek.com');", true);
            }
        }
        catch (Exception ex)
        {
            lblGeneratedText.Text = ex.Message;
        }
    }

    private async Task<string> CallLocalApi(string apiUrl)
    {
        string strAIModel;

        string strHQL;

        try
        {
            strHQL = "Select AIType,URL,Model From T_AIInterface";
            DataSet ds = ShareClass.GetDataSetFromSql(strHQL, "T_AIInterface");
            if (ds.Tables[0].Rows.Count > 0)
            {
                strAIModel = ds.Tables[0].Rows[0]["Model"].ToString().Trim();

                using (HttpClient client = new HttpClient())
                {
                    // 示例请求体
                    var requestBody = new
                    {
                        //model = "deepseek-r1:1.5b", // 替换为实际模型名称

                        model = strAIModel, // 替换为实际模型名称
                        prompt = txtPrompt.Text
                    };


                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                    HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, httpContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();

                        jsonString = jsonString.Replace("\\u003cthink\\u003e", "").Replace("\\u003c/think\\u003e", "");
                        jsonString = jsonString.Replace("***", "");
                        jsonString = jsonString.Replace("**", "");
                        jsonString = jsonString.Replace("###", "");
                        jsonString = jsonString.Replace("##", "");
                        jsonString = jsonString.Replace("\\n", "<br>");

                        // 提取所有 "response": 后面的字符串
                        List<string> responses = new List<string>();
                        int index = 0;

                        while (true)
                        {
                            // 查找 "response":" 的位置
                            int startIndex = jsonString.IndexOf(@"""response"":""", index);
                            if (startIndex == -1) break; // 如果没有找到，退出循环

                            // 跳过 "response":" 的长度
                            startIndex += @"""response"":""".Length;

                            // 查找下一个双引号的位置
                            int endIndex = jsonString.IndexOf(@"""", startIndex);
                            if (endIndex == -1) break; // 如果没有找到，退出循环

                            // 提取 response 的值
                            string responseItem = jsonString.Substring(startIndex, endIndex - startIndex);
                            responses.Add(responseItem);

                            // 更新搜索起始位置
                            index = endIndex + 1;
                        }

                        // 用空格连接所有 response 值
                        string combinedResponse = string.Join("", responses);

                        return combinedResponse;
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            else
            {
                return "Error";
            }
        }
        catch (Exception ex)
        {
            return lblGeneratedText.Text = ex.Message;
        }
    }
}