using System;
using System.Windows;

namespace SharpUtils.WPFUtils
{
    /// <summary>
    /// Helper class concerning WPF windows.
    /// </summary>
    public static class WindowHelpers
    {
        public static bool IsWindowOpen(Type WindowType)
        {
            bool windowOpen = false;
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == WindowType)
                {
                    windowOpen = true;
                }
            }
            return windowOpen;
        }
    }
}
