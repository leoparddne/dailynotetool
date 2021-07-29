using LYXUI.Control.Toast.Enum;
using LYXUI.Control.Toast.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace LYXUI.Control.Toast
{
    /// <summary>
    /// Toast.xaml 的交互逻辑
    /// </summary>
    public partial class Toast : Window
    {
        private static bool isShowing = false;
        private static readonly object _lockObj = new object();
        private static Queue<Toast> toasts = new Queue<Toast>();
        FrameworkElement OwnerFrameworkElement;
        //触发隐藏窗口
        DispatcherTimer displayTimer = new DispatcherTimer();
        ToastLocationEnum location;

        ToastModel model = new ToastModel();

        private Toast(string info, ToastDuringEnum duringEnum = ToastDuringEnum.NORMAL, FrameworkElement ownner = null,
            ToastLocationEnum location = ToastLocationEnum.Default, ToastTypeEnum level = ToastTypeEnum.Information)
        {
            InitializeComponent();
            this.DataContext = model;
            model.SetIcos(location);

            this.Topmost = true;
            this.OwnerFrameworkElement = ownner;

            if (ownner is Window ownnerWindow)
            {
                this.Owner = ownnerWindow;
            }
            this.location = location;


            displayTimer.Tick += new EventHandler(onTickedEvent);
            displayTimer.Interval = new TimeSpan(0, 0, (int)duringEnum);

            txtInfo.Text = info;


        }

        //重新计算定位
        private Tuple<double, double> Locate(ToastLocationEnum location)
        {
            Tuple<double, double> margin = new Tuple<double, double>(0, 0);
            switch (location)
            {
                case ToastLocationEnum.OwnerCenter:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.OwnerLeft:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.OwnerRight:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.OwnerTopLeft:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.OwnerTopCenter:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.OwnerTopRight:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.OwnerBottomLeft:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.OwnerBottomCenter:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.OwnerBottomRight:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.ScreenCenter:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.ScreenLeft:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.ScreenRight:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.ScreenTopLeft:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.ScreenTopCenter:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.ScreenTopRight:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.TOP);
                    break;
                case ToastLocationEnum.ScreenBottomLeft:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.LEFT, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.ScreenBottomCenter:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.ScreenBottomRight:
                    margin = CalcPosition(null, ToastHorizontalLocationEnum.RIGHT, ToastVerticalLocationEnum.BOTTON);
                    break;
                case ToastLocationEnum.Default:
                    margin = CalcPosition(Owner, ToastHorizontalLocationEnum.CENTER, ToastVerticalLocationEnum.CENTER);
                    break;
                case ToastLocationEnum.TOP:
                    margin = CalcOuterPosition(OwnerFrameworkElement, location);
                    break;
                case ToastLocationEnum.BOTTOM:
                    margin = CalcOuterPosition(OwnerFrameworkElement, location);
                    break;
                case ToastLocationEnum.LEFT:
                    margin = CalcOuterPosition(OwnerFrameworkElement, location);
                    break;
                case ToastLocationEnum.RIGHT:
                    margin = CalcOuterPosition(OwnerFrameworkElement, location);
                    break;
                default:
                    break;
            }

            if (margin != null)
            {
                this.Left = margin.Item1;

                this.Top = margin.Item2;
            }

            return margin;
        }

        /// <summary>
        /// 根据定位类型和相对容器计算位置
        /// </summary>
        /// <param name="onnerWindow">如果为空则为获取屏幕</param>
        /// <param name="horLocation"></param>
        /// <param name="verLocation"></param>
        /// <returns><left,top></returns>
        private Tuple<double, double> CalcPosition(FrameworkElement onnerWindow, ToastHorizontalLocationEnum horLocation, ToastVerticalLocationEnum verLocation)
        {
            double left = 0;
            double top = 0;

            var halfWidth = mainContainer.ActualWidth / 2;
            var halfHeight = mainContainer.ActualHeight / 2;

            double ownerWidth = (onnerWindow != null) ? Owner.ActualWidth : SystemParameters.WorkArea.Size.Width;
            double ownerHeight = (onnerWindow != null) ? Owner.ActualHeight : SystemParameters.WorkArea.Size.Height;

            double margin = 25;


            switch (horLocation)
            {
                case ToastHorizontalLocationEnum.LEFT:
                    left = margin;
                    break;
                case ToastHorizontalLocationEnum.CENTER:
                    left = ownerWidth / 2 - halfWidth;
                    break;
                case ToastHorizontalLocationEnum.RIGHT:
                    left = ownerWidth - mainContainer.ActualWidth - margin;
                    break;
                default:
                    break;
            }

            switch (verLocation)
            {
                case ToastVerticalLocationEnum.TOP:
                    top = margin;
                    break;
                case ToastVerticalLocationEnum.CENTER:
                    top = ownerHeight / 2 - halfHeight;
                    break;
                case ToastVerticalLocationEnum.BOTTON:
                    top = ownerHeight - mainContainer.ActualHeight - margin;
                    break;
                default:
                    break;
            }

            if (onnerWindow != null)
            {
                try
                {
                    var screenPoint = Owner.PointToScreen(new Point() { X = left, Y = top });

                    return new Tuple<double, double>(screenPoint.X, screenPoint.Y);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return new Tuple<double, double>(left, top);
            }
        }

        /// <summary>
        /// 根据容器计算获得toast的定位
        /// 只计算容器的上下左右四个位置
        /// </summary>
        /// <param name="onnerElement"></param>
        /// <param name="horLocation"></param>
        /// <param name="verLocation"></param>
        /// <returns></returns>
        private Tuple<double, double> CalcOuterPosition(FrameworkElement onnerElement, ToastLocationEnum horLocation)
        {
            if (onnerElement == null)
            {
                return new Tuple<double, double>(0, 0);
            }
            double left = 0;
            double top = 0;

            double margin = 3;
            var halfOwnWidth = onnerElement.ActualWidth / 2;
            var halfOwnHeight = onnerElement.ActualHeight / 2;
            var halfContainerWidth = mainContainer.ActualWidth / 2;
            var halfContainerHeight = mainContainer.ActualHeight / 2;


            switch (horLocation)
            {
                case ToastLocationEnum.TOP:
                    //相对容器的x轴原点+一半的相对容器宽度(居中)-toast一半的宽度(显示居中)
                    left = halfOwnWidth - halfContainerWidth;
                    top = -mainContainer.ActualHeight - margin;
                    break;
                case ToastLocationEnum.BOTTOM:
                    //相对容器的x轴原点+一半的相对容器宽度(居中)-toast一半的宽度(显示居中)
                    left = halfOwnWidth - halfContainerWidth;
                    //相对容器的y轴原点+toast高度+边距
                    top = onnerElement.ActualHeight + margin;
                    break;
                case ToastLocationEnum.LEFT:
                    //相对容器的x轴原点-toast宽度-边距
                    left = -mainContainer.ActualWidth - margin;
                    //相对容器的y轴原点+一半的相对容器高度(居中)-toast一半的高度(显示居中)
                    top = +halfOwnHeight - halfContainerHeight;
                    break;
                case ToastLocationEnum.RIGHT:
                    //相对容器的x轴原点+相对容器宽度+边距
                    left = onnerElement.ActualWidth + margin;
                    //相对容器的y轴原点+一半的相对容器高度(居中)-toast一半的高度(显示居中)
                    top = halfOwnHeight - halfContainerHeight;
                    break;
                default:
                    break;
            }

            try
            {
                var screentPoint = onnerElement.PointToScreen(new Point(left, top));
                return new Tuple<double, double>(screentPoint.X, screentPoint.Y);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Start()
        {
            displayTimer.Start();

            this.Show();
        }

        private void onTickedEvent(object sender, EventArgs e)
        {
            displayTimer.Stop();

            Task.Run(() =>
            {
                for (int i = 99; i > 0; i--)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Opacity = (double)i / 100;

                    });
                    Thread.Sleep(3);
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.Close();
                });
            });
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            lock (_lockObj)
            {
                isShowing = false;
            }
            //关闭当前toast,打开下一个toast
            ShowNext();
        }


        private static void Push(Toast newToast)
        {
            toasts.Enqueue(newToast);
            ShowNext();
        }

        /// <summary>
        /// 显示下一个toast
        /// </summary>
        private static void ShowNext()
        {
            if (!toasts.Any())
            {
                return;
            }
            lock (_lockObj)
            {
                if (!isShowing)
                {
                    isShowing = true;

                    var toast = toasts.Peek();

                    toasts.Dequeue();
                    if(toast.Locate(toast.location)!=null)
                    {
                        toast.Start();
                    }
                    else
                    {
                        isShowing = false;
                        ShowNext();
                    }
                }
            }
        }


        /// <summary>
        /// 创建toast
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ownnerOrRelevent">拥有toast的容器或是相对计算的容器</param>
        /// <param name="duringEnum"></param>
        /// <param name="location">如需锚定到窗口或者特定页面元素的相应位置则需要设置ownnerOrRelevent为window(窗口类型)或者FrameworkElement(页面元素)</param>
        /// <param name="level"></param>
        public static void MakeToast(string info,
                                    FrameworkElement ownnerOrRelevent = null,
                                    ToastDuringEnum duringEnum = ToastDuringEnum.SHORT,
                                    ToastLocationEnum location = ToastLocationEnum.Default,
                                    ToastTypeEnum level = ToastTypeEnum.Information)
        {
            var newToast = new Toast(info, duringEnum, ownnerOrRelevent, location, level);
            Push(newToast);
        }


        private void txtInfo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = txtInfo.ActualWidth + txtInfo.Margin.Left + txtInfo.Margin.Right + icoLeft.ActualWidth + icoRight.ActualWidth;
            this.Height = txtInfo.ActualHeight + txtInfo.Margin.Top + txtInfo.Margin.Bottom + icoTop.ActualHeight + icoBottom.ActualHeight;

            Locate(location);
        }
    }
}
