using System.Security.Principal;

namespace SharpUtils.MiscUtils
{
    public static class AdminCheckHelper
    {
        /// <summary>
        /// Checks if the current application is being run as an administrator.
        /// </summary>
        /// <returns>True if the current application is being run as an administrator, false otherwise.</returns>
        public static bool IsRunningAsAdmin()
        {
            bool isAdmin = false;
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            if (windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                isAdmin = true;
            }
            return isAdmin;
        }
    }
}
