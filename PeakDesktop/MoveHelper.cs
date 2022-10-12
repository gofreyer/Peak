using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakDesktop
{
    public class MoveHelper : DependencyObject
    {
        public static readonly DependencyProperty IsFrameProperty = DependencyProperty.RegisterAttached(
            "IsFrame", typeof(bool), typeof(MoveHelper), new PropertyMetadata(false));


        public static void SetIsFrame(DependencyObject target, Boolean value)
        {
            target.SetValue(IsFrameProperty, value);
        }

        public static bool GetIsFrame(DependencyObject target)
        {
            return (bool)target.GetValue(IsFrameProperty);
        }

        public static readonly DependencyProperty IsTurnProperty = DependencyProperty.RegisterAttached(
            "IsTurn", typeof(bool), typeof(MoveHelper), new PropertyMetadata(false));


        public static void SetIsTurn(DependencyObject target, Boolean value)
        {
            target.SetValue(IsTurnProperty, value);
        }

        public static bool GetIsTurn(DependencyObject target)
        {
            return (bool)target.GetValue(IsTurnProperty);
        }
    }
}
