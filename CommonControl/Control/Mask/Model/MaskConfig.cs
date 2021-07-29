using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LYXUI.Control.Mask.Model
{
    public class MaskConfig
    {
        //如果父元素为非window时需要计算父级相对窗口的定位
        public double top;
        public double left;

        /// <summary>
        /// 任务栏显示的名称
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// 是否为模态
        /// </summary>
        public bool ISModel { get; set; } = false;

        /// <summary>
        /// 是否可以点击蒙版阴影位置关闭蒙版
        /// </summary>
        public bool ClickBlankCanClose { get; set; } = false;

        /// <summary>
        /// 相对父级
        /// </summary>
        public FrameworkElement Ownner { get; set; } = null;

        /// <summary>
        /// 关闭蒙版回调
        /// </summary>
        public Action CloseCallback { get; set; } = null;

        /// <summary>
        /// 窗口显示状态发生变化回调
        /// </summary>
        public Action<Visibility> VisibilityChangeCallback { get; set; } = null;

    }
}
