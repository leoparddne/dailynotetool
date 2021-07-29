using LYXUI.Control.Mask.Model;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace LYXUI.Control.Mask
{
    /// <summary>
    /// Mask.xaml 的交互逻辑
    /// </summary>
    public partial class Mask : Window
    {
        MaskConfig model;

        //父元素
        FrameworkElement uiElement;

        //自动定位
        DependencyPropertyDescriptor descriptor;

        /// <summary>
        /// 私有构造函数避免外部主动参与控制窗口
        /// </summary>
        /// <param name="config"></param>
        private Mask(FrameworkElement uiElement, MaskConfig config = null)
        {
            InitializeComponent();
            model = config;

            //绑定需要监听的事件
            Bind(uiElement);

            container.Children.Add(uiElement);
            InitWindow();
        }

        void Bind(FrameworkElement uiElement)
        {
            if (uiElement == null)
            {
                return;
            }
            this.uiElement = uiElement;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        private void InitWindow()
        {
            //Binding binding = new Binding
            //{
            //    Source = uiElement,
            //    Path = new PropertyPath(LeftProperty),
            //    Mode= BindingMode.TwoWay
            //};
            ////Control x;
            //this.SetBinding(LeftProperty, binding);

            if (model == null)
            {
                return;
            }
            this.Title = model.Title;

            CalcPosition(model.Ownner);
        }

        /// <summary>
        /// 计算窗口定位
        /// </summary>
        /// <param name="parent"></param>
        void CalcPosition(FrameworkElement parent)
        {
            if (parent == null)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                //获取窗口<0,0>坐标屏幕位置
                var boundPoint = parent.PointToScreen(new Point(0, 0));
                this.Left = boundPoint.X;
                this.Top = boundPoint.Y;
                this.Width = parent.ActualWidth;
                this.Height = parent.ActualHeight;
                AutoMoving(this.model.Ownner);
            }
        }

        /// <summary>
        /// 自动更新定位
        /// </summary>
        /// <param name="parent"></param>
        private void AutoMoving(FrameworkElement parent)
        {
            Window parentWindow = null;
            if (this.Owner is Window win)
            {
                parentWindow = this.Owner;

                if (!(this.model.Ownner is Window))
                {
                    if (this.model.Ownner != null)
                    {
                        var point = this.model.Ownner.TranslatePoint(new(0, 0), (Window)this.Owner);
                        this.model.left = point.X;
                        this.model.top = point.Y;

                    }
                }

            }
            if (parent is Window par)
            {
                parentWindow = par;
            }

            //根据父窗口实时移动位置
            if (null != parentWindow && descriptor == null)
            {
                descriptor = DependencyPropertyDescriptor.FromProperty(LeftProperty, typeof(Window));
                //第一个对象为触发变化的对象
                descriptor.AddValueChanged(parentWindow, (o, handle) =>
                {
                    if (o is Window parentWindow)
                    {
                        this.Left = parentWindow.Left + this.model.left;
                        this.Top = parentWindow.Top + this.model.top;
                    }
                });
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                //this.WindowState = WindowState.Maximized;
            }
        }

        private void container_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (model == null)
            {
                return;
            }

            //点击空白处关闭窗口
            if (model.ClickBlankCanClose)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 激活窗口(链式调用)
        /// </summary>
        public Mask ActiveWindow()
        {
            //激活已显示的窗口
            if (this.Visibility == Visibility.Visible)
            {
                this.Activate();
            }
            else
            {
                if (model == null)
                {
                    this.Show();
                    return this;
                }
                //设置ownner
                if (model.Ownner != null)
                {
                    if (model.Ownner is Window parWindow)
                    {
                        this.Owner = parWindow;
                    }
                    else
                    {
                        var window = GetWindow(model.Ownner);
                        this.Owner = window;
                    }
                    CalcPosition(model.Ownner);
                }

                //根据配置控制是否打开模态框
                if (model.ISModel)
                {
                    this.ShowDialog();
                }
                else
                {
                    this.Show();
                }
            }

            return this;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public void CloseWindow()
        {
            this.Close();
        }

        /// <summary>
        /// 构造蒙版对象
        /// </summary>
        /// <param name="uiElement">需要展示的ui组件</param>
        /// <param name="config">配置</param>
        /// <returns></returns>
        public static Mask Create(FrameworkElement uiElement, MaskConfig config = null)
        {

            return new Mask(uiElement, config);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (model == null)
            {
                return;
            }
            model.CloseCallback?.Invoke();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (model == null)
            {
                return;
            }
            if (e.NewValue is bool value)
            {
                if (value)
                {
                    AutoMoving(model.Ownner);
                }
                else
                {
                    descriptor = null;
                }

            }


            model.VisibilityChangeCallback?.Invoke(this.Visibility);
        }
    }
}
