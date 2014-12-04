
using System;
using System.Collections.Generic;
using System.Text;


namespace SimpleHtmlToPdfConverter
{


    class Draw
    {


        public static System.Drawing.Image GetHatchImage(int iHatchNumber, string strHtmlColor)
        {
            System.Drawing.Color targetcol = System.Drawing.ColorTranslator.FromHtml(strHtmlColor);
            return GetHatchImage(iHatchNumber, targetcol);
        } // End Function GetHatchImage


        public static System.Drawing.Image GetHatchImage(int iHatchNumber, System.Drawing.Color targetcol)
        {
            System.Drawing.Bitmap bmp = null;

            string strBasePath = null;
            if (System.Web.Hosting.HostingEnvironment.IsHosted)
            {
                // http://stackoverflow.com/questions/944219/what-is-the-difference-between-server-mappath-and-hostingenvironment-mappath
                strBasePath = System.Web.Hosting.HostingEnvironment.MapPath("~/images/VWS/hatches/");
            }
            else
            {
                strBasePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                strBasePath = System.IO.Path.Combine(strBasePath, "hatches");
            }


            string strInput = System.IO.Path.Combine(strBasePath, "hatch" + iHatchNumber.ToString() + ".png");

            System.Drawing.Color sourcecol = System.Drawing.Color.Black;

            switch (iHatchNumber)
            {
                case 9:
                    sourcecol = System.Drawing.Color.FromArgb(255, 255, 65, 65); // hatch9.png
                    break;
                case 35:
                    sourcecol = System.Drawing.Color.FromArgb(255, 32, 119, 32); // hatch 35.png
                    sourcecol = System.Drawing.Color.HotPink;
                    break;
                case 69:
                    sourcecol = System.Drawing.Color.FromArgb(255, 0, 0, 22); // hatch 39.png
                    break;
                default:
                    sourcecol = System.Drawing.Color.HotPink;
                    throw new NotImplementedException("No sourcecolor for hatch pattern no. " + iHatchNumber.ToString() + " implemented.");
            } // End switch (iHatchNumber)


            //targetcol = System.Drawing.Color.HotPink;

            System.Drawing.Color coltrans = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            System.Drawing.Color coltrans2 = System.Drawing.Color.FromArgb(0, 0, 0, 0);


            Tools.ColorSpace.HSLColor sourcehsl = new Tools.ColorSpace.HSLColor(sourcecol);

            using (System.Drawing.Image img = System.Drawing.Image.FromFile(strInput))
            {
                bmp = new System.Drawing.Bitmap(img);

                for (int x = 0; x < bmp.Width; ++x)
                {
                    for (int y = 0; y < bmp.Height; ++y)
                    {
                        System.Drawing.Color col = bmp.GetPixel(x, y);

                        if (col == coltrans || col == coltrans2) continue;

                        //col = System.Drawing.Color.FromArgb(255, 255 - col.R, 255 - col.G, 255 - col.B);
                        Tools.ColorSpace.HSLColor hsl = new Tools.ColorSpace.HSLColor(col);


                        double deltalight = sourcehsl.Lightness - hsl.Lightness;
                        double deltasat = sourcehsl.Saturation - hsl.Saturation;


                        Tools.ColorSpace.HSLColor targethsl = new Tools.ColorSpace.HSLColor(targetcol);
                        targethsl.Lightness = targethsl.Lightness - deltalight;
                        if (targethsl.Hue != 0 || targethsl.Saturation != 0)
                            targethsl.Saturation = targethsl.Saturation - deltasat;


                        targethsl.Lightness = Math.Min(targethsl.Lightness, 1.0);
                        targethsl.Lightness = Math.Max(targethsl.Lightness, 0.0);

                        targethsl.Saturation = Math.Min(targethsl.Saturation, 1.0);
                        targethsl.Saturation = Math.Max(targethsl.Saturation, 0.0);


                        // targethsl.Lightness = hsl.Lightness;
                        // targethsl.Saturation = hsl.Saturation;
                        //if (targethsl.Hue != 0 || targethsl.Saturation != 0) targethsl.Saturation = hsl.Saturation;
                        //col = targetcol;
                        //bmp.MakeTransparent(System.Drawing.Color.White);
                        col = targethsl.Color;
                        bmp.SetPixel(x, y, col);
                    } // Next y

                } // Next x

            } // End Using (System.Drawing.Image img = System.Drawing.Image.FromFile(strInput)) 

            return bmp;
        } // End Function GetHatchImage




        public static System.Drawing.Brush GetBrush(int iLegendPattern, System.Drawing.Color colFC, System.Drawing.Color colHB, System.Drawing.Color colHL)
        {
            if (iLegendPattern == 12) // Normal, SolidBrush
            {
                return new System.Drawing.SolidBrush(colFC);
            }
            else if (iLegendPattern == 43) // MB: Leerstand, HatchBrush - HatchStyle.DarkUpwardDiagonal
            {
                return new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DarkUpwardDiagonal, System.Drawing.Color.Black, System.Drawing.Color.White);
            }
            else if (iLegendPattern == 0) // 0 -- >40 Zeilen / >40 lignes, keine
            {
                return null; // Console.WriteLine("40 Zeilen / >40 lignes");
            }

            //else // [9: kein Mietobjekt,35: MB Mieter ], TextureBrush
            //Image image = Image.FromFile(@"D:\stefan.steiger\documents\visual studio 2010\Projects\DrawLegends\DrawLegends\hatches\hatch" + iLegendPattern + ".png");

            System.Drawing.TextureBrush tb = null;

            using (System.Drawing.Image image = GetHatchImage(iLegendPattern, colFC))
            {
                tb = new System.Drawing.TextureBrush(image, System.Drawing.Drawing2D.WrapMode.Tile);
            } // End Using Image image

            // tb.TranslateTransform(x, y);
            return tb;
        } // End Function GetBrush


        public static string DrawHatchRectangleToBase64String(int iLegendPattern, System.Drawing.Color colFC, System.Drawing.Color colHB, System.Drawing.Color colHL, System.Drawing.Imaging.ImageFormat format)
        {
            string str = null;
            byte[] ba = DrawHatchRectangleToByteArray(iLegendPattern, colFC, colHB, colHL, format);
            str = System.Convert.ToBase64String(ba);
            ba = null;

            return str;
        }


        public static byte[] ImageAsByteArray(System.Drawing.Image bmp, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] ba = null;

            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, format);
                ba = ms.ToArray();
            }

            return ba;
        } // End Function ImageAsByteArray


        public static byte[] ImageAsByteArray(string strFileName, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] ba = null;

            using (System.Drawing.Image bmp = System.Drawing.Image.FromFile(strFileName))
            { 
            
                using (var ms = new System.IO.MemoryStream())
                {
                    bmp.Save(ms, format);
                    ba = ms.ToArray();
                }
            }

            return ba;
        } // End Function ImageAsByteArray


        public static string ImageAsBase64String(string strFileName, System.Drawing.Imaging.ImageFormat format)
        {
            string str = null;

            byte[] ba = ImageAsByteArray(strFileName, format);
            str = System.Convert.ToBase64String(ba);
            ba = null;

            return str;
        } // End Function ImageAsByteArray
        

        public static string ImageAsDataString(string strFileName, System.Drawing.Imaging.ImageFormat format)
        {
            string str = ImageAsBase64String(strFileName, format);

            return "data:" + GetMimeType(format) + ";base64," + str;
        } // End Function ImageAsByteArray


        public static byte[] DrawHatchRectangleToByteArray(int iLegendPattern, System.Drawing.Color colFC, System.Drawing.Color colHB, System.Drawing.Color colHL, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] ba = null;

            using (System.Drawing.Image bmp = DrawHatchRectangleToImg(iLegendPattern, colFC, colHB, colHL))
            {

                using (var ms = new System.IO.MemoryStream())
                {
                    bmp.Save(ms, format);
                    ba = ms.ToArray();
                }

            }

            return ba;
        }


        public static System.Drawing.Image DrawHatchRectangleToImg(int iLegendPattern, System.Drawing.Color colFC, System.Drawing.Color colHB, System.Drawing.Color colHL)
        {
            int iBulletSize = 20;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(iBulletSize, iBulletSize);


            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.Clear(System.Drawing.Color.White);

                // Add legend rectangle with color 
                using (System.Drawing.Brush CurrentBrush = GetBrush(iLegendPattern, colFC, colHB, colHL))
                {
                    System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(0, 0, iBulletSize, iBulletSize);

                    if (CurrentBrush != null)
                    {
                        //if (object.ReferenceEquals(CurrentBrush, typeof(TextureBrush)))
                        if (CurrentBrush is System.Drawing.TextureBrush)
                            ((System.Drawing.TextureBrush)CurrentBrush).TranslateTransform(rect2.X, rect2.Y);

                        // Punkt - Quadrat
                        g.FillRectangle(CurrentBrush, rect2);
                        // cDrawingTools.FillTriangle(g, CurrentBrush, iOriginX, iOriginY, i, iBulletSize);
                        // cDrawingTools.FillCircle(g, CurrentBrush, iOriginX + iBulletSize / 2.0f, iOriginY + i * (iBulletSize * 2) + iBulletSize / 2.0f, iBulletSize / 2.0f);

                    } // End if (CurrentBrush != null)

                } // End Using System.Drawing.Brush CurrentBrush 
            }

            return bmp;
        } // End Sub 






        public static string GetMimeType(System.Drawing.Image i)
        {
            System.Guid imgguid = i.RawFormat.Guid;
            return GetMimeType(imgguid);
        }

        public static string GetMimeType(System.Drawing.Imaging.ImageFormat i)
        {
            System.Guid imgguid = i.Guid;
            return GetMimeType(imgguid);
        }


        public static string GetMimeType(System.Guid imgguid)
        {

            foreach (System.Drawing.Imaging.ImageCodecInfo codec in System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                    return codec.MimeType;
            }

            return "image/unknown";
        }


        public static string GetHatchRectangleAsDataString(
             int iLegendPattern
            , System.Drawing.Color colFC
            , System.Drawing.Color colHB
            , System.Drawing.Color colHL
            , System.Drawing.Imaging.ImageFormat format
        )
        {
            //pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(9, colFC, colHB, colHL);
            //pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(35, colFC, colHB, colHL);
            //pbRectangle.Image = SimpleHtmlToPdfConverter.Draw.DrawHatchRectangle_Img(69, colFC, colHB, colHL);

            string str = DrawHatchRectangleToBase64String(iLegendPattern, colFC, colHB, colHL, format);

            return "data:" + GetMimeType(format) + ";base64," + str;
        }



    }
}
