using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace System.Drawing
{
    /// <summary>
    /// Thumbnail Mode Option 
    /// </summary>
    public enum ThumbnailMode
    {
        /// <summary>
        /// Fixed Height And Width
        /// </summary>
        FixedHeightAndWidth,
        /// <summary>
        /// Fixed Height And Width By Percent
        /// </summary>
        FixedHeightAndWidthByPercent,
        /// <summary>
        /// Fixed Width And Height By Percent
        /// </summary>
        FixedWidthAndHeightByPercent,
        /// <summary>
        /// Cut
        /// </summary>
        Cut,
        /// <summary>
        /// Percent By the Lower one of Height And Width
        /// </summary>
        PercentByLowLength,
        /// <summary>
        /// 
        /// </summary>
        FixedBox
    }

    /// <summary>
    /// 
    /// </summary>
    public class ThumbConfig
    {
        /// <summary>
        /// Thumb Save Path (absolute path)
        /// </summary>
        public string Path { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public ThumbnailMode Mode { get; set; }

        public bool IsPng { get; set; }
    }

    public static class ThumbnailUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="configs"></param>
        public static void MakeThumbnail(System.Drawing.Image originalImage, ThumbConfig config)
        {
            MakeThumbnail(originalImage, new List<ThumbConfig>() { config });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="configs"></param>
        public static void MakeThumbnail(System.Drawing.Image originalImage, IList<ThumbConfig> configs)
        {
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            foreach (ThumbConfig config in configs)
            {

                int towidth = config.Width;
                int toheight = config.Height;

                switch (config.Mode)
                {
                    case ThumbnailMode.FixedHeightAndWidth:

                        break;
                    case ThumbnailMode.FixedBox:
                        float sx = (float)config.Width / (float)ow;
                        float sy = (float)config.Height / (float)oh;
                        float scale = Math.Min(1, Math.Min(sx, sy));
                        towidth = (int)(ow * scale);
                        toheight = (int)(oh * scale);
                        break;
                    case ThumbnailMode.FixedHeightAndWidthByPercent:
                        toheight = originalImage.Height * config.Width / originalImage.Width;
                        break;
                    case ThumbnailMode.FixedWidthAndHeightByPercent:
                        towidth = originalImage.Width * config.Height / originalImage.Height;
                        break;
                    case ThumbnailMode.Cut:
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * config.Height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    case ThumbnailMode.PercentByLowLength:
                        int newwidth = config.Width;
                        int newheight = 0;

                        if (originalImage.Width > 0 && originalImage.Height > 0)
                        {
                            if (originalImage.Width / originalImage.Height >= 1)
                            {
                                if (originalImage.Width > newwidth)
                                {
                                    newheight = originalImage.Height * newwidth / originalImage.Width;
                                }
                                else
                                {
                                    newwidth = originalImage.Width;
                                    newheight = originalImage.Height;
                                }
                            }
                            else
                            {
                                if (originalImage.Height > newwidth)
                                {
                                    newheight = newwidth;
                                    newwidth = (originalImage.Width * newwidth) / originalImage.Height;
                                }
                                else
                                {
                                    newwidth = originalImage.Width;
                                    newheight = originalImage.Height;
                                }
                            }
                        }

                        towidth = newwidth;
                        toheight = newheight;
                        //config.Mode = ThumbnailMode.HeightAndWidth;
                        break;
                    default:
                        break;
                }

                config.Width = towidth;
                config.Height = toheight;

                //new Bitmap instance
                Image bitmap = new Bitmap(towidth, toheight);

                //new Graphics instance 
                Graphics g = Graphics.FromImage(bitmap);

                //set InterpolationMode
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //set CompositingQuality
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                //set HighQuality ,low speed SmoothingMode  
                g.SmoothingMode = SmoothingMode.AntiAlias;

                //clear the Graphics and fill with Transparent background color 

                if (config.IsPng)
                    g.Clear(Color.Transparent);
                else
                    g.Clear(Color.White);

                // render
                g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;

                foreach (ImageCodecInfo codec in codecs)
                {

                    if (config.IsPng)
                    {
                        if (codec.MimeType.IndexOf("png") > -1)
                        {
                            ici = codec;
                        }
                    }
                    else
                    {
                        if (codec.MimeType.IndexOf("jpeg") > -1)
                        {
                            ici = codec;
                        }
                    }
                }

                var myEncoder = Encoder.Quality;
                var myEncoderParameters = new EncoderParameters(1);

                //ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                //ImageCodecInfo ici = null;


                // 在这里设置图片的质量等级为95L. 
                var myEncoderParameter = new EncoderParameter(myEncoder, 95L);

                myEncoderParameters.Param[0] = myEncoderParameter;

                try
                {
                    // System.Drawing.Imaging.ImageFormat.Jpeg
                    bitmap.Save(config.Path, ici, myEncoderParameters);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                finally
                {
                    myEncoderParameter.Dispose();
                    myEncoderParameters.Dispose();

                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="configs"></param>
        public static void MakeThumbnail(string originalImagePath, IList<ThumbConfig> configs)
        {

            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            MakeThumbnail(originalImage, configs);

            originalImage.Dispose();
        }

        /// <summary>
        /// Make a Thumbnail
        /// </summary>
        public static void MakeThumbnail(string originalImagePath, ThumbConfig config)
        {
            MakeThumbnail(originalImagePath, new List<ThumbConfig>() { config });
        }

        static Font defaultWaterMarkFont = new Font("Verdana", 16);

        /// <summary>
        /// Add a water-mark Text 
        /// </summary>
        public static void AddWaterMarkWord(string srcPath, string savePath, string waterMarkText)
        {
            AddWaterMarkWord(srcPath, savePath, waterMarkText, defaultWaterMarkFont);
        }

        /// <summary>
        /// Add a water-mark Text 
        /// </summary>
        public static void AddWaterMarkWord(string srcPath, string savePath, string waterMarkText, Font waterMarkFont)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(srcPath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            Brush brush = new SolidBrush(Color.Blue);

            g.DrawString(waterMarkText, waterMarkFont, brush, 15, 15);
            g.Dispose();

            image.Save(savePath);
            image.Dispose();
        }

        /// <summary>
        /// Add a water-mark Picture
        /// </summary>
        public static void AddWaterMarkPic(string srcPath, string savePath, string waterMarkPicPath)
        {
            Image image = Image.FromFile(srcPath);
            Image copyImage = Image.FromFile(waterMarkPicPath);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(savePath);
            image.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strContentType"></param>
        /// <returns></returns>
        public static ImageFormat GetImageType(string strContentType)
        {
            ImageFormat GetImageType;
            switch (strContentType.ToLower())
            {
                default:
                    GetImageType = System.Drawing.Imaging.ImageFormat.MemoryBmp;
                    break;
                case "image/gif":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Gif;
                    break;
                case "image/pjpeg":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Jpeg;
                    break;
                case "image/x-png":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Png;
                    break;
                case "image/tiff":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Tiff;
                    break;
                case "image/bmp":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Bmp;
                    break;
                case "image/x-emf":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Emf;
                    break;
                case "image/x-wmf":
                    GetImageType = System.Drawing.Imaging.ImageFormat.Wmf;
                    break;
            }
            return GetImageType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imgfile"></param>
        public static void DeleteImg(string imgfile)
        {
            System.IO.FileInfo fi = new FileInfo(imgfile);
            if (fi.Exists)
                fi.Delete();
        }
    }
}
