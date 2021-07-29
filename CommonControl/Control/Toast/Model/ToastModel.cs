using LYXUI.Control.Toast.Enum;
using LYXUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LYXUI.Control.Toast.Model
{
    public class ToastModel : ViewModelBase
    {
        Visibility topIcoVisibility = Visibility.Collapsed;
        Visibility rightIcoVisibility = Visibility.Collapsed;
        Visibility bottomIcoVisibility = Visibility.Collapsed;
        Visibility leftIcoVisibility = Visibility.Collapsed;

        /// <summary>
        /// 左侧的小三角是否显示
        /// </summary>
        public Visibility LeftIcoVisibility
        {
            get => leftIcoVisibility;
            set
            {
                leftIcoVisibility = value;
                PCEH();
            }
        }

        /// <summary>
        /// 右侧的小三角是否显示
        /// </summary>
        public Visibility RightIcoVisibility
        {
            get => rightIcoVisibility;
            set
            {
                rightIcoVisibility = value;
                PCEH();
            }
        }

        /// <summary>
        /// 底部的小三角是否显示
        /// </summary>
        public Visibility BottomIcoVisibility
        {
            get => bottomIcoVisibility;
            set
            {
                bottomIcoVisibility = value;
                PCEH();
            }
        }

        /// <summary>
        /// 上面的小三角是否显示
        /// </summary>
        public Visibility TopIcoVisibility
        {
            get => topIcoVisibility;
            set
            {
                topIcoVisibility = value;
                PCEH();
            }
        }

        /// <summary>
        /// 设置箭头显示状态
        /// </summary>
        /// <param name="toastLocationEnum"></param>
        public void SetIcos(ToastLocationEnum toastLocationEnum)
        {
            bool left = false, top = false, right = false, bottom = false;
            switch (toastLocationEnum)
            {
                case ToastLocationEnum.TOP:
                    SetVisible(ref bottomIcoVisibility, bottom = true);
                    break;
                case ToastLocationEnum.BOTTOM:
                    SetVisible(ref topIcoVisibility, top = true);
                    break;
                case ToastLocationEnum.LEFT:
                    SetVisible(ref rightIcoVisibility, right = true);
                    break;
                case ToastLocationEnum.RIGHT:
                    SetVisible(ref leftIcoVisibility, left = true);
                    break;
                default:
                    break;
            }
            ReSetVisible(ref topIcoVisibility, top);
            ReSetVisible(ref bottomIcoVisibility, bottom);
            ReSetVisible(ref leftIcoVisibility, left);
            ReSetVisible(ref rightIcoVisibility, right);


            PC("TopIcoVisibility");
            PC("BottomIcoVisibility");
            PC("LeftIcoVisibility");
            PC("RightIcoVisibility");
        }

        /// <summary>
        /// 如果isVisible为false则重置visibility为Collapsed
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="isVisible"></param>
        private void ReSetVisible(ref Visibility visibility, bool isVisible)
        {
            if (false == isVisible)
            {
                visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 根据isVisible设置visibility
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="isVisible"></param>
        private void SetVisible(ref Visibility visibility, bool isVisible)
        {
            visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
