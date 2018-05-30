using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MP.Tools
{
    #region 获取json文件
    /// <summary>
    /// 获取json配置文件
    /// </summary>
    public class ConfigurationAppSetting
    {
        private static IConfigurationRoot config = null;
        /// <summary>
        ///  Microsoft.Extensions.Configuration扩展包提供的    
        /// </summary>
        static ConfigurationAppSetting()
        {
            config = new ConfigurationBuilder().Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();
        }

        public static IConfigurationRoot AppSettings
        {
            get
            {
                return config;
            }
        }

        public static string Get(string key)
        {
            return config[key];
        }

    }
    #endregion
    /// <summary>
    /// 获取config配置文件
    /// </summary>
    public class AppSettingHelper
    {
        /// <summary>
        /// 默认读取app.config文件
        /// </summary>
        static public string ApiUrl = ConfigurationAppSetting.AppSettings["AndroidAppkey"];


        static public string EncryptionKey = ConfigurationAppSetting.AppSettings["EncryptionKey"];
    }


}
