using System;
using System.Runtime.InteropServices;

namespace wkHtmlToPdfSharp.Util
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}