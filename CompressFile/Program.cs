
using System;
using System.Collections.Generic;
using System.Text;


namespace CompressFile
{


    class Program
    {



        private static string InitBasePath()
        {
            // if (System.Web.HttpContext.Current != null) return System.Web.HttpContext.Current.Server.MapPath("~/bin/");
            // http://stackoverflow.com/questions/111927/how-to-access-the-httpserverutility-mappath-method-in-a-thread-or-timer

            // http://stackoverflow.com/questions/5154272/how-can-c-sharp-library-code-know-its-hosting-application-type-without-system-w
            // http://msdn.microsoft.com/en-us/library/system.environment.userinteractive(v=vs.110).aspx
            //if (!Environment.UserInteractive)
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                // return System.IO.Path.Combine(System.Web.HttpRuntime.AppDomainAppPath,"bin");
                return System.Web.Hosting.HostingEnvironment.MapPath("~/bin");
            }

            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }


        public static void LzmaTest()
        {
            // http://www.splinter.com.au/compressing-using-the-7zip-lzma-algorithm-in/

            // Some random text to compress, thanks to http://en.wikipedia.org/wiki/V8
            string OriginalText =
            @"From A V8 engine is a V engine with eight cylinders mounted on the crankcase
in two banks of four cylinders, in most cases set at a right angle to each other
but sometimes at a narrower angle, with all eight pistons driving a common crankshaft.
In its simplest form, it is basically two straight-4 engines sharing a common
crankshaft. However, this simple configuration, with a single-plane crankshaft,
has the same secondary dynamic imbalance problems as two straight-4s, resulting
in annoying vibrations in large engine displacements. As a result, since the 1920s
most V8s have used the somewhat more complex crossplane crankshaft with heavy
counterweights to eliminate the vibrations. This results in an engine which is
smoother than a V6, while being considerably less expensive than a V12 engine.
Racing V8s continue to use the single plane crankshaft because it allows faster
acceleration and more efficient exhaust system designs.";

            System.Text.Encoding enc = System.Text.Encoding.UTF8;

            // Convert the text into bytes
            byte[] DataBytes = enc.GetBytes(OriginalText);
            Console.WriteLine("Original data is {0} bytes", DataBytes.Length);

            // Compress it
            byte[] Compressed = SevenZip.Compression.LZMA.SevenZipHelper.Compress(DataBytes);
            Console.WriteLine("Compressed data is {0} bytes", Compressed.Length);

            // Decompress it
            byte[] Decompressed = SevenZip.Compression.LZMA.SevenZipHelper.Decompress(Compressed);
            Console.WriteLine("Decompressed data is {0} bytes", Decompressed.Length);

            // Convert it back to text
            string DecompressedText = enc.GetString(Decompressed);
            Console.WriteLine("Is the decompressed text the same as the original? {0}",
              DecompressedText == OriginalText);

            // Print it out
            Console.WriteLine("And here is the decompressed text:");
            Console.WriteLine(DecompressedText);
        }


        public static void LoadEmbeddedResource(string ResourceName, string tgt)
        {
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();

            if (ResourceName != null)
            {
                using (System.IO.FileStream fsOutput = new System.IO.FileStream(tgt, System.IO.FileMode.Create))
                {
                    using (System.IO.Stream strm = ass.GetManifestResourceStream(ResourceName))
                    {
                        byte[] buffer = new byte[8 * 1024];
                        int len;
                        while ((len = strm.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fsOutput.Write(buffer, 0, len);
                        }
                        buffer = null;
                    } // End Using strm

                } // End Using fsOutput 

            } // End if (ResourceName != null)
            
        }


        static void Main(string[] args)
        {
            string FileToCompress = @"D:\wkhtmltopdf\msvc\x64\bin\wkhtmltox.dll";
            //FileToCompress = @"D:\wkhtmltopdf\msvc\x32\bin\wkhtmltox.dll";

            string CompressedFile = System.IO.Path.Combine(
                 System.IO.Path.GetDirectoryName(FileToCompress)
                ,System.IO.Path.GetFileName(FileToCompress) + ".gz"
            );



            SevenZip.Compression.LZMA.SevenZipHelper.Compress(FileToCompress, FileToCompress + ".lzma");
            SevenZip.Compression.LZMA.SevenZipHelper.Decompress(@"D:\wkhtmltopdf\msvc\x32\bin\wkhtmltox.dll.lzma", @"D:\wkhtmltopdf\msvc\x32\bin\decomp.dll");
            
            Lzf.LZF lz = new Lzf.LZF();


            //byte[] ba = System.IO.File.ReadAllBytes(@"path");
            //byte[] outp = new byte[ba.Length*2];
            //int size = lz.Compress(ba, ba.Length, outp, outp.Length);


            CompressFile(FileToCompress, CompressedFile);
            // CompressFile_AllInOne(FileToCompress, CompressedFile);

            DeflateCompressFile(FileToCompress, CompressedFile + ".def");

            DeflateDeCompressFile(CompressedFile + ".def", CompressedFile + ".def.uncompressed");


            //UncompressFile2(CompressedFile, CompressedFile + ".uncompressed");
            DeCompressFile(CompressedFile, CompressedFile + ".uncompressed");
            
            byte[] DecompressedBuffer = System.IO.File.ReadAllBytes(CompressedFile);
            System.IO.File.WriteAllBytes(CompressedFile + ".lolunc", Decompress(DecompressedBuffer));


            byte[] UncompressedBuffer = System.IO.File.ReadAllBytes(FileToCompress);
            System.IO.File.WriteAllBytes(CompressedFile + ".lolcomp", Compress(UncompressedBuffer));
            


            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        } // End Sub Main


        // http://www.webpronews.com/gzip-vs-deflate-compression-and-performance-2006-12
        // First I tested the GZipStream and then the DeflateStream. 
        // I expected a minor difference because the two compression methods are different, but the result astonished me. 
        // I measured the DeflateStream to 41% faster than GZip. 
        // That's a very big difference. With this knowledge, 
        // I'll have to change the HTTP compression module to choose Deflate over GZip.



        // Deflate is just the compression algorithm. GZip is actually a format.
        // If you use the GZipStream to compress a file (and save it with the extension .gz), 
        // the result can actually be opened by archivers such as WinZip or the gzip tool. 
        // If you compress with a DeflateStream, those tools won't recognize the file.
        public static void CompressFile(string FileToCompress, string CompressedFile)
        {
            //byte[] buffer = new byte[1024 * 1024 * 64];
            byte[] buffer = new byte[1024 * 1024]; // 1MB

            using (System.IO.FileStream sourceFile = System.IO.File.OpenRead(FileToCompress))
            {

                using (System.IO.FileStream destinationFile = System.IO.File.Create(CompressedFile))
                {

                    using (System.IO.Compression.GZipStream output = new System.IO.Compression.GZipStream(destinationFile,
                        System.IO.Compression.CompressionMode.Compress))
                    {

                        int bytesRead = 0;
                        while (bytesRead < sourceFile.Length)
                        {
                            int ReadLength = sourceFile.Read(buffer, 0, buffer.Length);
                            output.Write(buffer, 0, ReadLength);
                            output.Flush();
                            bytesRead += ReadLength;
                        } // Whend

                        destinationFile.Flush();
                    } // End Using System.IO.Compression.GZipStream output

                    destinationFile.Close();
                } // End Using System.IO.FileStream destinationFile 

                // Close the files.
                sourceFile.Close();
            } // End Using System.IO.FileStream sourceFile

        } // End Sub CompressFile


        public static void DeflateCompressFile(string FileToCompress, string CompressedFile)
        {
            //byte[] buffer = new byte[1024 * 1024 * 64];
            byte[] buffer = new byte[1024 * 1024]; // 1MB

            using (System.IO.FileStream sourceFile = System.IO.File.OpenRead(FileToCompress))
            {

                using (System.IO.FileStream destinationFile = System.IO.File.Create(CompressedFile))
                {

                    using (System.IO.Compression.DeflateStream output = new System.IO.Compression.DeflateStream(destinationFile,
                        System.IO.Compression.CompressionMode.Compress))
                    {

                        int bytesRead = 0;
                        while (bytesRead < sourceFile.Length)
                        {
                            int ReadLength = sourceFile.Read(buffer, 0, buffer.Length);
                            output.Write(buffer, 0, ReadLength);
                            output.Flush();
                            bytesRead += ReadLength;
                        } // Whend

                        destinationFile.Flush();
                    } // End Using System.IO.Compression.GZipStream output

                    destinationFile.Close();
                } // End Using System.IO.FileStream destinationFile 

                // Close the files.
                sourceFile.Close();
            } // End Using System.IO.FileStream sourceFile

        } // End Sub CompressFile



        // http://www.dotnetperls.com/compress
        /// <summary>
        /// Compresses byte array to new byte array.
        /// </summary>
        public static byte[] Compress(byte[] raw)
        {
            byte[] baRetVal;

            using (System.IO.MemoryStream memstrm = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(memstrm, System.IO.Compression.CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                    gzip.Flush();
                } // End Using System.IO.Compression.GZipStream gzip

                memstrm.Flush();
                baRetVal = memstrm.ToArray();
                memstrm.Close();
            } // End Using System.IO.MemoryStream memory

            return baRetVal;
        } // End Function Compress


        // http://www.dotnetperls.com/compress
        /// <summary>
        /// Compresses byte array to new byte array.
        /// </summary>
        public static byte[] DeflateCompress(byte[] raw)
        {
            byte[] baRetVal;

            using (System.IO.MemoryStream memstrm = new System.IO.MemoryStream())
            {
                using (System.IO.Compression.DeflateStream gzip = new System.IO.Compression.DeflateStream(memstrm, System.IO.Compression.CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                    gzip.Flush();
                } // End Using System.IO.Compression.GZipStream gzip

                memstrm.Flush();
                baRetVal = memstrm.ToArray();
                memstrm.Close();
            } // End Using System.IO.MemoryStream memory

            return baRetVal;
        } // End Function Compress




        public static void DeCompressFile(string CompressedFile, string DeCompressedFile)
        {
            byte[] buffer = new byte[1024 * 1024];

            using (System.IO.FileStream fstrmCompressedFile = System.IO.File.OpenRead(CompressedFile)) // fi.OpenRead())
            {
                using (System.IO.FileStream fstrmDecompressedFile = System.IO.File.Create(DeCompressedFile))
                {
                    using (System.IO.Compression.GZipStream strmUncompress = new System.IO.Compression.GZipStream(fstrmCompressedFile,
                            System.IO.Compression.CompressionMode.Decompress))
                    {
                        int numRead = strmUncompress.Read(buffer, 0, buffer.Length);

                        while (numRead != 0)
                        {
                            fstrmDecompressedFile.Write(buffer, 0, numRead);
                            fstrmDecompressedFile.Flush();
                            numRead = strmUncompress.Read(buffer, 0, buffer.Length);
                        } // Whend

                        //int numRead = 0;

                        //while ((numRead = strmUncompress.Read(buffer, 0, buffer.Length)) != 0)
                        //{
                        //    fstrmDecompressedFile.Write(buffer, 0, numRead);
                        //    fstrmDecompressedFile.Flush();
                        //} // Whend

                        strmUncompress.Close();
                    } // End Using System.IO.Compression.GZipStream strmUncompress 

                    fstrmDecompressedFile.Flush();
                    fstrmDecompressedFile.Close();
                } // End Using System.IO.FileStream fstrmCompressedFile 

                fstrmCompressedFile.Close();
            } // End Using System.IO.FileStream fstrmCompressedFile 

        } // End Sub DeCompressFile


        public static void DeflateDeCompressFile(string CompressedFile, string DeCompressedFile)
        {
            byte[] buffer = new byte[1024 * 1024];

            using (System.IO.FileStream fstrmCompressedFile = System.IO.File.OpenRead(CompressedFile)) // fi.OpenRead())
            {
                using (System.IO.FileStream fstrmDecompressedFile = System.IO.File.Create(DeCompressedFile))
                {
                    using (System.IO.Compression.DeflateStream strmUncompress = new System.IO.Compression.DeflateStream(fstrmCompressedFile,
                            System.IO.Compression.CompressionMode.Decompress))
                    {
                        int numRead = strmUncompress.Read(buffer, 0, buffer.Length);

                        while (numRead != 0)
                        {
                            fstrmDecompressedFile.Write(buffer, 0, numRead);
                            fstrmDecompressedFile.Flush();
                            numRead = strmUncompress.Read(buffer, 0, buffer.Length);
                        } // Whend

                        //int numRead = 0;

                        //while ((numRead = strmUncompress.Read(buffer, 0, buffer.Length)) != 0)
                        //{
                        //    fstrmDecompressedFile.Write(buffer, 0, numRead);
                        //    fstrmDecompressedFile.Flush();
                        //} // Whend

                        strmUncompress.Close();
                    } // End Using System.IO.Compression.GZipStream strmUncompress 

                    fstrmDecompressedFile.Flush();
                    fstrmDecompressedFile.Close();
                } // End Using System.IO.FileStream fstrmCompressedFile 

                fstrmCompressedFile.Close();
            } // End Using System.IO.FileStream fstrmCompressedFile 

        } // End Sub DeCompressFile


        // http://www.dotnetperls.com/decompress
        public static byte[] Decompress(byte[] gzip)
        {
            byte[] baRetVal = null;
            using (System.IO.MemoryStream ByteStream = new System.IO.MemoryStream(gzip))
            {

                // Create a GZIP stream with decompression mode.
                // ... Then create a buffer and write into while reading from the GZIP stream.
                using (System.IO.Compression.GZipStream stream = new System.IO.Compression.GZipStream(ByteStream
                    , System.IO.Compression.CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (System.IO.MemoryStream memstrm = new System.IO.MemoryStream())
                    {
                        int count = 0;
                        count = stream.Read(buffer, 0, size);
                        while (count > 0)
                        {
                            memstrm.Write(buffer, 0, count);
                            memstrm.Flush();
                            count = stream.Read(buffer, 0, size);
                        } // Whend

                        baRetVal = memstrm.ToArray();
                        memstrm.Close();
                    } // End Using memstrm

                    stream.Close();
                } // End Using System.IO.Compression.GZipStream stream 

                ByteStream.Close();
            } // End Using System.IO.MemoryStream ByteStream

            return baRetVal;
        } // End Sub Decompress


        public static byte[] DeflateDecompress(byte[] gzip)
        {
            byte[] baRetVal = null;
            using (System.IO.MemoryStream ByteStream = new System.IO.MemoryStream(gzip))
            {

                // Create a GZIP stream with decompression mode.
                // ... Then create a buffer and write into while reading from the GZIP stream.
                using (System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ByteStream
                    , System.IO.Compression.CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (System.IO.MemoryStream memstrm = new System.IO.MemoryStream())
                    {
                        int count = 0;
                        count = stream.Read(buffer, 0, size);
                        while (count > 0)
                        {
                            memstrm.Write(buffer, 0, count);
                            memstrm.Flush();
                            count = stream.Read(buffer, 0, size);
                        } // Whend

                        baRetVal = memstrm.ToArray();
                        memstrm.Close();
                    } // End Using memstrm

                    stream.Close();
                } // End Using System.IO.Compression.GZipStream stream 

                ByteStream.Close();
            } // End Using System.IO.MemoryStream ByteStream

            return baRetVal;
        } // End Sub Decompress


        // http://msdn.microsoft.com/en-us/library/ms404280(v=vs.80).aspx
        // And just allocate a buffer that might be several GB in size...
        public static void CompressFile_ShittyMsdnVersion(string FileToCompress, string CompressedFile)
        {
            using (System.IO.FileStream sourceFile = System.IO.File.OpenRead(FileToCompress))
            {
                using (System.IO.FileStream destinationFile = System.IO.File.Create(CompressedFile))
                {
                    byte[] buffer = new byte[sourceFile.Length];
                    sourceFile.Read(buffer, 0, buffer.Length);

                    using (System.IO.Compression.GZipStream output = new System.IO.Compression.GZipStream(destinationFile,
                        System.IO.Compression.CompressionMode.Compress))
                    {
                        output.Write(buffer, 0, buffer.Length);
                        output.Flush();
                        destinationFile.Flush();
                    } // End Using System.IO.Compression.GZipStream output

                    // Close the files.        
                    destinationFile.Close();
                } // End Using System.IO.FileStream destinationFile 

                sourceFile.Close();
            } // End Using System.IO.FileStream sourceFile

        } // End Sub CompressFile


        // http://msdn.microsoft.com/en-us/library/ms404280(v=vs.80).aspx
        // And swiftly fail on a large file because buffer < origFile.Length
        private static void DeCompressFile_ShittyMsdnVersion(string CompressedFile, string DeCompressedFile)
        {
            System.IO.FileStream sourceFile = System.IO.File.OpenRead(CompressedFile);
            System.IO.FileStream destinationFile = System.IO.File.Create(DeCompressedFile);

            // Because the uncompressed size of the file is unknown, 
            // we are using an arbitrary buffer size.
            byte[] buffer = new byte[1024 * 1024 * 100]; // Note: Fails here when buffer < FileSizeOfUncompressedFile
            int n;

            using (System.IO.Compression.GZipStream input = new System.IO.Compression.GZipStream(sourceFile,
                System.IO.Compression.CompressionMode.Decompress, false))
            {
                Console.WriteLine("Decompressing {0} to {1}.", sourceFile.Name, destinationFile.Name);

                n = input.Read(buffer, 0, buffer.Length);
                destinationFile.Write(buffer, 0, n);
            } // End Using System.IO.Compression.GZipStream input

            // Close the files.
            sourceFile.Close();
            destinationFile.Close();
        } // End Sub DeCompressFile_ShittyMsdnVersion


        // My first try at fixing the shitty msdn sample
        // Fails because sourceFile.Length is length of compressed instead of uncompressed file... 
        // And input.Length throws NotSupportedException...
        private static void DeCompressFile_MyShittyFix(string CompressedFile, string DeCompressedFile)
        {
            using (System.IO.FileStream sourceFile = System.IO.File.OpenRead(CompressedFile))
            {

                using (System.IO.FileStream destinationFile = System.IO.File.Create(DeCompressedFile))
                {
                    byte[] buffer = new byte[1024 * 1024]; // 1MB

                    using (System.IO.Compression.GZipStream input = new System.IO.Compression.GZipStream(sourceFile,
                        System.IO.Compression.CompressionMode.Decompress, false))
                    {
                        int bytesRead = 0;
                        while (bytesRead < sourceFile.Length) // Fails here 
                        {
                            int ReadLength = input.Read(buffer, 0, buffer.Length);
                            destinationFile.Write(buffer, 0, ReadLength);
                            bytesRead += ReadLength;
                        } // Whend

                        destinationFile.Flush();
                    } // End Using System.IO.Compression.GZipStream input 

                    // Close the files.
                    destinationFile.Close();
                } // End Using System.IO.FileStream destinationFile 

                sourceFile.Close();
            } // End Using System.IO.FileStream sourceFile 

        } // End Sub DeCompressFile_MyShittyFix


    } // End Class Program


} // End Namespace CompressFile
