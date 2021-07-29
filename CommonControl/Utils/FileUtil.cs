using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYXUI.Utils
{
    /// <summary>
    /// 本地文件路径
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 保存数据的主目录名
        /// </summary>
        public static string HomeDir = "leyixue";



        private static string BlackboardFileName = "Blackboard.Json";

        public static string BlackboardFilePath = GetUserDataDir() + "\\" + BlackboardFileName;

        /// <summary>
        /// 获取用户数据目录(主目录)
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationDataDir()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        /// <summary>
        /// 获取程序数据目录
        /// </summary>
        /// <returns></returns>
        public static string GetUserDataDir()
        {
            var dataDir = GetApplicationDataDir();
            var resultFloder = Path.Combine(dataDir, HomeDir);
            if (!Directory.Exists(resultFloder))
            {
                Directory.CreateDirectory(resultFloder);
            }
            return resultFloder;
        }
    }
}
