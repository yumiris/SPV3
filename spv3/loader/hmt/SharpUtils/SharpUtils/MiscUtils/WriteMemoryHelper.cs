using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

// TODO: This could use some work/refactoring.
// TODO: Should write a Win32ErrorCode helper too (maybe not).
// TODO: Maybe an IsProcessRunning(string ProcessName) helper?
namespace SharpUtils.MiscUtils
{
    /// <summary>
    /// If the PID for the target process was not found, this will be raised. Typically this means the process
    /// was not running.
    /// </summary>
    public class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException(string exceptionText) : base(exceptionText) { }
    }

    /// <summary>
    /// A helper class for writing to a foreign process' memory.
    /// </summary>
    public static class WriteMemoryHelper
    {
        const int PROCESS_WM_READ = 0x0010;
        const int PROCESS_VM_WRITE = 0x0020;
        const int PROCESS_VM_OPERATION = 0x0008;

        // WriteProcessMemory PINVOKE
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
        // OpenProcess PINVOKE
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        // CloseHandle PINVOKE
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);

        /// <summary>
        /// Returns the PID of the requested process. Throws ProcessNotFoundException if the process was not found.
        /// </summary>
        /// <param name="ProcessName">The name of the process to get the PID of.</param>
        /// <returns>The PID of the process.</returns>
        private static int GetPid(string ProcessName)
        {
            Process[] matchingProcesses = Process.GetProcessesByName(ProcessName);
            if (matchingProcesses.Length <= 0)
            {
                throw new ProcessNotFoundException("The specified process was not found. Is it running?");
            }
            return matchingProcesses[0].Id;
        }

        /// <summary>
        /// Gets the base address of the specified process. Throws ProcessNotFoundException if the process was not found.
        /// </summary>
        /// <param name="ProcessName">The name of the process to get the base address of.</param>
        /// <returns></returns>
        private static IntPtr GetBaseAddr(string ProcessName)
        {
            IntPtr baseAddress;
            Process[] matchingProcesses = Process.GetProcessesByName(ProcessName);
            if (matchingProcesses.Length <= 0)
            {
                throw new ProcessNotFoundException("The specified process was not found. Is it running?");
            }
            baseAddress = matchingProcesses[0].MainModule.BaseAddress;

            return baseAddress;
        }

        /// <summary>
        /// Writes the passed value into the target process' memory at the specified location.
        /// Use GetLastWin32Error() if the operation returns false.
        /// </summary>
        /// <param name="ProcessName">The name of the process to write to.</param>
        /// <param name="ValueToWrite">The value to write to the target address in the process' memory.</param>
        /// <param name="TargetAddress">The address to write the value to.</param>
        /// <returns>True if the operation was successful, false otherwise.</returns>
        public static bool WriteToProcessMemory(string ProcessName, byte[] ValueToWrite, int TargetAddress)
        {
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ | PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, GetPid(ProcessName));
            IntPtr baseAddr = GetBaseAddr(ProcessName);

            IntPtr writeLocationAddress = IntPtr.Add(baseAddr, TargetAddress);

            bool operationSuccessful = WriteProcessMemory(processHandle, writeLocationAddress, ValueToWrite, ValueToWrite.Length, out int bytesWritten);
            CloseHandle(processHandle);

            return operationSuccessful;
        }
    }
}
