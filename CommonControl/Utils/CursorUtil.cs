using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Drawing;

namespace LYXUI.Utils
{
    public static class CursorUtil
    {
        /// <summary>
        /// 根据cur文件创建光标
        /// </summary>
        /// <param name="curPath"></param>
        /// <returns></returns>
        public static Cursor CreateCursorFromCur(string curPath)
        {
            var fileURI = new Uri(curPath, UriKind.Relative);

            StreamResourceInfo penStream = Application.GetResourceStream(fileURI); 
            Cursor penCursor = new Cursor(penStream.Stream);
            return penCursor;
        }
    }

}
