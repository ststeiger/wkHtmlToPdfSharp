
using System;


namespace WriteTextToGif
{


    // Credits: Abdallah Gomah, 19 Jan 2009 CPOL
    // http://www.codeproject.com/Articles/32660/Write-text-to-transparent-GIF
    public partial class cGifText
    {
        private System.Drawing.Brush TextBrush = null;
        private System.Drawing.Font TextFont = null;
        private System.Drawing.Color TextColor = default(System.Drawing.Color);


        private System.Drawing.Bitmap _sourceImage = null;
        private System.Drawing.Bitmap _destintationImage = null;


        /// <summary>
        /// Gets or sets the source image to be modified.
        /// </summary>
        public System.Drawing.Bitmap SourceImage
        {
            get { return _sourceImage; }
            set { _sourceImage = value; }
        } // End Property SourceImage

        

        /// <summary>
        /// Gets or sets the modified image.
        /// </summary>
        public System.Drawing.Bitmap DestinationImage
        {
            get { return _destintationImage; }
            set 
            { 
                _destintationImage = value;
            }
        } // End Property DestinationImage


        public void OpenImage(string fileName)
        {
            this.SourceImage = new System.Drawing.Bitmap(fileName);
            this.DestinationImage = this.SourceImage;
        } // End Sub OpenImage


        public void SaveImage(string fileName)
        {
            System.Drawing.Bitmap gif = CreateIndexedImage(this.DestinationImage, this.SourceImage.Palette);
            gif.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);
        } // End Sub SaveImage


        public void SetFont(string strFontName, System.Drawing.FontStyle fsFontStyle, int iFontSize, System.Drawing.Color cTextColor)
        {
            TextFont = new System.Drawing.Font(strFontName, iFontSize, fsFontStyle, System.Drawing.GraphicsUnit.Point);
            TextColor = cTextColor;
            //Color.Black
            TextBrush = new System.Drawing.SolidBrush(TextColor);
        } // End Sub SetFont


        /// <summary>
        /// Writes a text to the bitmap.
        /// </summary>
        /// <param name="left">The left position of the text to be written.</param>
        /// <param name="top">The top position of the text to be written.</param>
        /// <param name="text">The text to be written.</param>
        private void WriteText(int left, int top, string text)
        {
            //Create a blank image.
            this.DestinationImage = new System.Drawing.Bitmap(this.SourceImage.Width, this.SourceImage.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            //Font titleFont = new Font("Arial Black", 36, FontStyle.Italic, GraphicsUnit.Point);

            // If forgot to specify the font name etc.
            if (TextFont == null)
            {
                TextFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            }

            // If forgot to specify color
            if (TextColor.IsEmpty)
            {
                TextColor = System.Drawing.Color.Black;
            }


            // If forgot to specify color
            if (TextBrush == null)
            {
                TextBrush = new System.Drawing.SolidBrush(TextColor);
            }


            //Get the Graphics object of the image to use in drawing.
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(this.DestinationImage))
            {
                //Draw the original image.
                g.DrawImage(this.SourceImage, 0, 0);
                //Used to write smooth text.
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                
                // float y = top + g.MeasureString(title, TextFont).Height;
                // g.DrawString(body, TextFont, TextBrush, left, y, StringFormat.GenericTypographic);
                g.DrawString(text, TextFont, TextBrush, left, top, System.Drawing.StringFormat.GenericTypographic);
            } // End Using g

        } // End Sub WriteText 



        /// <summary>
        /// Converts a color to an integer.
        /// </summary>
        /// <param name="color">The color to be converted.</param>
        /// <returns></returns>
        private static int Color2Int(System.Drawing.Color color)
        {
            return (color.R * 1000000 + color.G * 1000 + color.B);
        } // End Function Color2Int


        /// <summary>
        /// Gets the nearst color index from the palette to the givin color.
        /// </summary>
        /// <param name="pal">The color palette.</param>
        /// <param name="c">The color to be searched for.</param>
        /// <returns>The nearest color from the palette.</returns>
        private static int GetNearestColor(System.Drawing.Imaging.ColorPalette pal, System.Drawing.Color cColor)
        {
            int iNearest = int.MaxValue;
            int iIndex = 0;

            for (int i = 0; i < pal.Entries.Length; i++)
            {
                System.Drawing.Color cIndexColor = pal.Entries[i];

                //int iDistance = Convert.ToInt32(Math.Pow(cIndexColor.R - cColor.R, 2) + Math.Pow(cIndexColor.G - cColor.G, 2) + Math.Pow(cIndexColor.B - cColor.B, 2));
                int iDistance = Convert.ToInt32(Math.Pow(Convert.ToDouble(cIndexColor.R) - Convert.ToDouble(cColor.R), 2) + Math.Pow(Convert.ToDouble(cIndexColor.G) - Convert.ToDouble(cColor.G), 2) + Math.Pow(Convert.ToDouble(cIndexColor.B) - Convert.ToDouble(cColor.B), 2));

                if (iDistance < iNearest)
                {
                    iIndex = i;
                    iNearest = iDistance;
                } // End if (iDistance < iNearest)

            } // Next i

            return iIndex;
        } // End Function GetNearestColor 


        /// <summary>
        /// Creates an indexed image from a bitmap with a given palette.
        /// </summary>
        /// <param name="src">The source image.</param>
        /// <param name="palette">The palette to be used.</param>
        /// <returns>An indexed image.</returns>
        private static System.Drawing.Bitmap CreateIndexedImage(System.Drawing.Bitmap src, System.Drawing.Imaging.ColorPalette palette)
        {
            //Create an indexed image.
            System.Drawing.Bitmap dest = new System.Drawing.Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            //Create a dictionary of colors to speed up the search.
            System.Collections.Generic.Dictionary<int, int> colors = new System.Collections.Generic.Dictionary<int, int>();
            //The transparent color index.
            int transparent = 255;
            //Load the dictionary with the given palette.
            for (int i = 0; i < palette.Entries.Length; i++)
            {
                colors[Color2Int(palette.Entries[i])] = i;
                if (palette.Entries[i].A == 0)
                    transparent = i;
            }
            //Set the palette of the image.
            dest.Palette = palette;

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, src.Width, src.Height);
            //Lock the image data so you can modify it.
            System.Drawing.Imaging.BitmapData destData = dest.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, dest.PixelFormat);

            //The number of bytes in each line of the image.
            int dStride = System.Convert.ToInt32(Math.Abs(destData.Stride));
            //Create a buffer to hold the image data.
            byte[] destBytes = new byte[dest.Height * dStride];
            //Copy the image data into the buffer.
            IntPtr destPtr = destData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(destPtr, destBytes, 0, dest.Height * dStride);
            //Select the best fit color index for each pixel in the image.
            for (int row = 0; row < src.Height; row++)
            {
                for (int col = 0; col < src.Width; col++)
                {
                    //Get the color of the pixel.
                    System.Drawing.Color c = src.GetPixel(col, row);
                    int index = 255;

                    if (c.A == 0) //Transparent
                    {
                        index = transparent;
                    }
                    else
                    {
                        //Get the nearst color from the palette.
                        int ic = Color2Int(c);
                        if (colors.ContainsKey(ic))
                        {
                            index = colors[ic];
                        }
                        else
                        {
                            index = GetNearestColor(palette, c);
                            colors[ic] = index;
                        }
                    }

                    //Update the color index in the buffer.
                    destBytes[row * dStride + col] = (byte)index;
                } // Next col

            } // Next row

            //Copy the image data back to the image.
            System.Runtime.InteropServices.Marshal.Copy(destBytes, 0, destPtr, dest.Height * dStride);

            //Unlock the data.
            dest.UnlockBits(destData);

            return dest;
        } // End Function CreateIndexedImage


    } // End Class cGifText


} // End Namespace WriteTextToGif
