using System;
using System.Runtime.InteropServices;
using wkHtmlToPdfSharp.Util;

namespace wkHtmlToPdfSharp
{
    internal static class wkHtmlToPdfSharpBindings
    {
        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_init(int useGraphics);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_deinit();

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_extended_qt();

        
        //[DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPStr)]
        //public static extern String wkhtmltopdf_version();

        //[DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPWStr)]
        //public static extern String wkhtmltopdf_version();

        
        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(ConstUtf8Marshaler))]
        public static extern String wkhtmltopdf_version();


        //[DllImport("wkhtmltox0.dll", EntryPoint = "wkhtmltopdf_version", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //[return: MarshalAs(UnmanagedType.LPWStr)]
        //public static extern String wkhtmltopdf_version_internal();


        //public static string wkhtmltopdf_version()
        //{
        //    string strVersion = wkhtmltopdf_version_internal();
        //    strVersion = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Unicode.GetBytes(strVersion));

        //    return strVersion;
        //}
        


        
        //[DllImport("wkhtmltox0.dll", CharSet = CharSet.Ansi, EntryPoint = "wkhtmltopdf_version", CallingConvention = CallingConvention.StdCall)]
        //public static extern IntPtr wkhtmltopdf_version_internal();

        //// http://stackoverflow.com/questions/15793736/difference-between-marshalasunmanagedtype-lpwstr-and-marshal-ptrtostringuni
        //public static string wkhtmltopdf_version()
        //{
        //    IntPtr ptr = wkhtmltopdf_version_internal();
        //    string retVal = Marshal.PtrToStringAnsi(ptr);
            
        //    // System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
        //    // System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocAnsi(ptr);
        //    // System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            
        //    // System.Runtime.InteropServices.Marshal.FreeCoTaskMem(ptr);
        //    // System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemAnsi(ptr);
        //    // System.Runtime.InteropServices.Marshal.ZeroFreeCoTaskMemUnicode(ptr);
            
        //    // System.Runtime.InteropServices.Marshal.FreeBSTR(ptr);
        //    // System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);

        //    return retVal;
        //}
        





        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_global_settings();

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_object_settings();

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_set_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String value);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_get_global_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String name,
            [In][Out] ref byte[] value, int valueSize);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_set_object_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String value);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_get_object_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String name,
            [In][Out] ref byte[] value, int vs);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_warning_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_phase_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] VoidCallback callback);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_progress_changed_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_convert(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))]String data);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings, byte[] data);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_current_phase(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_phase_count(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_phase_description(IntPtr converter, int phase);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_progress_string(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_http_error_code(IntPtr converter);

        [DllImport("wkhtmltox0.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        //public static extern long wkhtmltopdf_get_output(IntPtr converter, out IntPtr result);
        public static extern int wkhtmltopdf_get_output(IntPtr converter, out IntPtr data);
    }
}
