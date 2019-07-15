﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FaceAdmin
{
    public class HttpUitls
    {
        public static string Get(string Url)
        {
            //System.GC.Collect();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }

            return retString;
        }

        /// <summary> WebService：Post调用
        /// </summary>
        /// <param name="url">Webservice地址</param>
        /// <param name="paramsOfUrl">传入数据</param>
        /// <returns>返回结果</returns>
        public static string DoPost(string url, Hashtable paramsOfUrl)
        {
            if (url == null)
            {
                throw new Exception("WebService地址为空");
            }
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

            // 编辑并Encoding提交的数据 
            byte[] data = GetJointBOfParams(paramsOfUrl);

            // 发送请求 
            request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            // 获得回复 
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        /// <summary> 拼接参数串----Get
        /// </summary>
        /// <param name="paramsOfUrl">参数项</param>
        /// <returns>返回字符串</returns>
        private static String GetJointSOfParams(Hashtable paramsOfUrl)
        {
            if (paramsOfUrl == null || paramsOfUrl.Count == 0) return String.Empty;
            // 编辑并Encoding提交的数据 
            StringBuilder sbuilder = new StringBuilder();
            int i = 0;
            foreach (DictionaryEntry de in paramsOfUrl)
            {
                string value = ToHttpChar(de.Value.ToString());
                if (i == 0)
                {
                    sbuilder.Append(de.Key + "=" + value);
                }
                else
                {
                    sbuilder.Append("&" + de.Key + "=" + value);
                }
                i++;
            }
            return sbuilder.ToString();
        }

        /// <summary> 拼接参数串----Post
        /// </summary>
        /// <param name="paramsOfUrl">参数项</param>
        /// <returns>返回字节数组</returns>
        private static byte[] GetJointBOfParams(Hashtable paramsOfUrl)
        {
            // 编辑并Encoding提交的数据 
            String stringJointOfParams = GetJointSOfParams(paramsOfUrl);
            byte[] data = new ASCIIEncoding().GetBytes(stringJointOfParams);
            return data;
        }

        /// <summary> 转义特殊字符
        /// </summary>
        private static string ToHttpChar(string value)
        {
            value = value.ToString().Replace("+", "%2B");
            //value = value.ToString().Replace(" ", "%20");
            //value = value.ToString().Replace("/", "%2F");
            //value = value.ToString().Replace("?", "%3F");
            //value = value.ToString().Replace("%", "%25");
            //value = value.ToString().Replace("#", "%23");
            //value = value.ToString().Replace("&", "%26");
            //value = value.ToString().Replace("=", "%3D");
            //value = value.ToString().Replace(@"\", "%5C");
            //value = value.ToString().Replace(".", "%2E");
            //value = value.ToString().Replace(":", "%3A");
            return value;
        }
    }

}

