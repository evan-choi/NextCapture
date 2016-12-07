using System;
using System.Windows.Forms;

namespace NextCapture.Utils
{
    static class Dispatcher
    {
        private static Control mControl;

        public static void UIInvoke(Action action)
        {
            if (mControl.InvokeRequired)
            {
                mControl.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public static void Init()
        {
            mControl = new Control();
        }
    }
}
