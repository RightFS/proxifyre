using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedUp
{
    internal class Utils
    {
        public string GetShortcutTarget(string shortcutPath)
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
    }
}
