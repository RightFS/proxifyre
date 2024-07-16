using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpeedUp
{
    internal class Utils
    {
        public static string GetShortcutTarget(string shortcutPath)
        {
            if (System.IO.File.Exists(shortcutPath))
            {
                // 创建一个WshShell对象来访问快捷方式
                WshShell shell = new WshShell();
                // 使用WshShell的CreateShortcut方法打开快捷方式文件
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                // 返回快捷方式的目标路径
                return shortcut.TargetPath;
            }
            return null;
        }

        public static string ComputeMd5Hash(string rawData)
        {
            // 创建一个 MD5   
            using (MD5 md5Hash = MD5.Create())
            {
                // 计算字符串的哈希值   
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // 将字节数组转换为十六进制字符串   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
