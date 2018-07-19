
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AccountBook.Common
{
    /// <summary>
    /// 网络操作
    /// </summary>
    public class Net
    {


        public static string  Ip
        {
            get
            {
                return "125.122.25.2";

            }
        }

        /// <summary>
        /// 淘宝api根据ip得到当前城市
        /// </summary>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static string GetIPCitys(string strIP)
    {
        try
        {
            string Url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + strIP + "";
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
            wReq.Timeout = 2000;
            System.Net.WebResponse wResp = wReq.GetResponse();
            System.IO.Stream respStream = wResp.GetResponseStream();
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream))
            {
                string jsonText = reader.ReadToEnd();
                    Newtonsoft.Json.Linq.JObject ja = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText);                
                if (ja["code"].ToString() == "0")
                {                                             
                string[] addresArry = { ja["data"]["country"].ToString(), ja["data"]["region"].ToString(),
                            ja["data"]["city"].ToString() };
                return string.Join("-", addresArry);
                    }
                else
                {
                    return "未知";
                }
            }
        }
        catch (Exception)
        {
            return ("未知");
        }
    }
    }
    
}
