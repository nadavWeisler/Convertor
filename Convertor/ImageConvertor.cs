﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Convertor
{
    class ImageConvertor
    {
        private readonly string _filePath;
        private readonly string _savePath;
        private readonly ImageFormat _imageFormat;

        /// <summary>
        /// class for multi thread use of SaveImageAs
        /// </summary>
        /// <param name="filePath">image file path to convert</param>
        /// <param name="imageFormat">converts to this image format</param>
        public ImageConvertor(string filePath, ImageFormat imageFormat)
        {
            this._filePath = filePath;
            this._imageFormat = imageFormat;
        }

        /// <summary>
        /// class for multi thread use of SaveImageAs
        /// </summary>
        /// <param name="filePath">image file path to convert</param>
        /// <param name="savePath">file path to save new image</param>
        /// <param name="imageFormat">converts to this image format</param>
        public ImageConvertor(string filePath, string savePath, ImageFormat imageFormat) : this(filePath, imageFormat)
        {
            this._savePath = savePath;
        }

        /// <summary>
        /// converts the image in path 'filePath' to 'imageFormat' and saves it in 'savePath'
        /// </summary>
        public void SaveImageAs()
        {
            ImageConvertor.SaveImageAs(this._filePath, this._savePath, this._imageFormat);
        }

        /// <summary>
        /// converts the image in path 'filePath' to 'imageFormat' and saves it in 'filePath' but with a the extension
        /// </summary>
        /// <param name="filePath">image file path to convert</param>
        /// <param name="imageFormat">converts to this image format</param>
        public static void SaveImageAs(string filePath, ImageFormat imageFormat)
        {
            using (Image image = new Bitmap(filePath))
            {
                string folderPath = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string convertedExtension = imageFormat.ToString().ToLower();
                string savePath = string.Format("{0}\\{1}.{2}", folderPath, fileName, convertedExtension);

                image.Save(savePath, imageFormat);
            }
        }

        /// <summary>
        /// converts the image in path 'filePath' to 'imageFormat' and saves it in 'savePath'
        /// </summary>
        /// <param name="filePath">image file path to convert</param>
        /// <param name="savePath">file path to save new image</param>
        /// <param name="imageFormat">converts to this image format</param>
        public static void SaveImageAs(string filePath, string savePath, ImageFormat imageFormat)
        {
            if (savePath == null)
            {
                SaveImageAs(filePath, imageFormat);
                return;
            }

            using (Image image = new Bitmap(filePath))
            {
                image.Save(savePath, imageFormat);
            }
        }

        /// <summary>
        /// converts the image by class attributes
        /// </summary>
        /// <param name="imageConvertor">attributes in class are taken to the function</param>
        public static void SaveImageAs(ImageConvertor imageConvertor)
        {
            SaveImageAs(imageConvertor._filePath, imageConvertor._savePath, imageConvertor._imageFormat);
        }

        /// <summary>
        /// converts the image in path 'filePath' to 'imageFormat' and saves it in 'savePath'
        /// </summary>
        /// <param name="filePath">image file path to convert</param>
        /// <param name="savePath">file path to save new image</param>>
        /// <param name="quality">positive number between 0 and 1 , 1 is the same quality as source(removes compression), 0 is no quality, this will determine how big the file size is</param>
        public static void SaveImageAsJpg(string filePath, string savePath, float quality)
        {
            using (Image image = new Bitmap(filePath))
            {
                EncoderParameters es = new EncoderParameters();
                EncoderParameter e1 = new EncoderParameter(Encoder.Quality, (long)(quality * 100));

                es.Param = new EncoderParameter[] { e1 };
                image.Save(savePath, GetJpgCodec(), es);
            }
        }

        private static ImageCodecInfo JpgCodec = null;
        private static ImageCodecInfo GetJpgCodec()
        {
            if (JpgCodec != null)
                return JpgCodec;

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == ImageFormat.Jpeg.Guid)
                {
                    JpgCodec = codec;
                    return codec;
                }
            }
            return null;
        }


        public static ImageFormat GetImageFormatByString(string formatName)
        {
            PropertyInfo[] properties = typeof(ImageFormat).GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.Name == formatName)
                {
                    return (ImageFormat)prop.GetMethod.Invoke(null,null);
                }
            }
            return null;
        }
    }
}
