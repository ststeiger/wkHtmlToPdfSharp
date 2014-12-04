
using System;
using System.Collections.Generic;


namespace wkHtmlToPdfSharp
{

    
    public class SharedLibrary
    {

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        protected static extern IntPtr LoadLibrary(string lpFileName);

        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        protected static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

        [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        protected static extern bool FreeLibrary(IntPtr hModule);




        // See http://mpi4py.googlecode.com/svn/trunk/src/dynload.h
        protected const int RTLD_LAZY = 1; // for dlopen's flags
        protected const int RTLD_NOW = 2; // for dlopen's flags

        [System.Runtime.InteropServices.DllImport("libdl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        protected static extern IntPtr dlopen(string filename, int flags);

        [System.Runtime.InteropServices.DllImport("libdl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        protected static extern IntPtr dlsym(IntPtr handle, string symbol);

        [System.Runtime.InteropServices.DllImport("libdl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        protected static extern int dlclose(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("libdl", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        protected static extern string dlerror();

        

        // Note: Not thread safe, but OK on app-start / end
        protected static Dictionary<string, IntPtr> m_dict_LoadedDlls = new Dictionary<string, IntPtr>();

        //System.IntPtr library = wkHtmlToPdfSharp.NotwkHtmlToPdfSharp.OS.SharedLibrary.Load(strFileName);
        public static System.IntPtr Load(string strFileName)
        {
            IntPtr hSO = IntPtr.Zero;

            try
            {

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    hSO = dlopen(strFileName, RTLD_NOW);
                }
                else
                {
                    hSO = LoadLibrary(strFileName);
                } // End if (Environment.OSVersion.Platform == PlatformID.Unix)

            } // End Try
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            } // End Catch

            if (hSO == IntPtr.Zero)
            {
                throw new ApplicationException("Cannot open " + strFileName);
            } // End if (hSO == IntPtr.Zero)


            m_dict_LoadedDlls.Add(strFileName, hSO);

            return hSO;
        } // End Function LoadSharedObject

        
        // wkHtmlToPdfSharp.NotwkHtmlToPdfSharp.OS.SharedLibrary.Unload(library);
        public static bool Unload(System.IntPtr hSO)
        {
            bool bError = true;

            if (hSO == IntPtr.Zero)
            {
                throw new ArgumentNullException("hSO");
            } // End if (hSO == IntPtr.Zero)

            try
            {

                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    // If the referenced object was successfully closed, dlclose() shall return 0. 
                    // If the object could not be closed, or if handle does not refer to an open object, 
                    // dlclose() shall return a non-zero value. 
                    // More detailed diagnostic information shall be available through dlerror().

                    // http://stackoverflow.com/questions/956640/linux-c-error-undefined-reference-to-dlopen
                    if(dlclose(hSO) == 0)
                        bError = false;

                    if (bError)
                        throw new ApplicationException("Error unloading handle " + hSO.ToInt64().ToString() + Environment.NewLine + "System error message: " +  dlerror());
                }
                else
                {
                    // FreeLibrary: If the function succeeds, the return value is nonzero.
                    // If the function fails, the return value is zero. 
                    // To get extended error information, call the GetLastError function.
                    bError = !FreeLibrary(hSO);

                    if(bError)
                        throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error()); 

                } // End if (Environment.OSVersion.Platform == PlatformID.Unix)

            } // End Try
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            } // End Catch

            if (bError)
            {
                throw new ApplicationException("Cannot unload handle " + hSO.ToInt64().ToString());
            } // End if (hExe == IntPtr.Zero)

            return bError;
        } // End Function Unload


        public static void UnloadAllLoadedDlls()
        {
            foreach (string strKey in m_dict_LoadedDlls.Keys)
            {
                try
                {
                    Unload(m_dict_LoadedDlls[strKey]);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error unloading \"" + strKey + "\".");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }

            } // Next strKey

        } // End Sub UnloadAllLoadedDlls


    } // End Class SharedLibrary


} // End Namespace Platform
