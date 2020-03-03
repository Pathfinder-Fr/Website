// -----------------------------------------------------------------------
// <copyright file="ImageHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    public static class ImageHelper
    {
        /// <summary>
        /// Generate a new image from the bitmap with the specified format, width, and height, and at the specified location.
        /// </summary>
        /// <param name="sourceBmp">
        /// The bitmap containing an image from which to generate a new image with the
        /// specified settings. This bitmap is not modified.
        /// </param>
        /// <param name="newFilePath">The location on disk to store the image that is generated.</param>
        /// <param name="newImageFormat">The new image format.</param>
        /// <param name="newWidth">The width to make the new image.</param>
        /// <param name="newHeight">The height to make the new image.</param>
        /// <param name="newJpegQuality">
        /// The JPEG quality setting (0 - 100) for the new image. Only used if the
        /// image format paramater is JPEG; ignored for all other formats.
        /// </param>
        /// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">
        /// Thrown when <paramref name="sourceBmp" />
        /// cannot be resized to the requested dimensions.
        /// </exception>
        public static void SaveImageFile(Image sourceBmp, string newFilePath, ImageFormat newImageFormat, int newWidth, int newHeight, int newJpegQuality)
        {
            //Create new bitmap with the new dimensions and in the specified format.
            var destinationBmp = CreateResizedBitmap(sourceBmp, sourceBmp.Size.Width, sourceBmp.Size.Height, newWidth, newHeight);
            try
            {
                SaveImageToDisk(destinationBmp, newFilePath, newImageFormat, newJpegQuality);
            }
            finally
            {
                destinationBmp.Dispose();
            }
        }

        /// <summary>
        /// Calculate the required width and height of the optimized image based on the specified Bitmap. The aspect ratio
        /// of the bitmap image is preserved in the calculated values. This method does not create the optimized image -
        /// it only calculates the dimensions it should be created with.
        /// </summary>
        /// <param name="bmp">The Bitmap containing an image from which the optimized width and height values should be calculated.</param>
        /// <param name="width">The calculated width of the optimized image.</param>
        /// <param name="height">The calculated height of the optimized image.</param>
        public static void CalculateOptimizedWidthAndHeight(Image bmp, out int width, out int height)
        {
            if (bmp == null)
                throw new ArgumentNullException("bmp");

            // Calculate the width and height based on the user settings and aspect ratio of the specified bitmap.
            // 

            ///	maxLength: The length (in pixels) of the longest edge of an optimized image.  This value is used when an optimized
            ///	image is created. The length of the shorter side is calculated automatically based on the aspect ratio of the image.

            var maxLength = SueetieConfiguration.Get().AvatarSettings.Height;
            int originalWidth, originalHeight, newWidth, newHeight;

            originalWidth = bmp.Width;
            originalHeight = bmp.Height;

            if ((maxLength > originalWidth) && (maxLength > originalHeight))
            {
                // Bitmap is smaller than desired optimized dimensions. Don't enlarge optimized; just use bitmap size.
                newWidth = originalWidth;
                newHeight = originalHeight;
            }
            else if (originalWidth > originalHeight)
            {
                // Bitmap is in landscape format (width > height). The width will be the longest dimension.
                newWidth = maxLength;
                newHeight = originalHeight * newWidth / originalWidth;
            }
            else
            {
                // Bitmap is in portrait format (height > width). The height will be the longest dimension.
                newHeight = maxLength;
                newWidth = originalWidth * newHeight / originalHeight;
            }

            width = newWidth;
            height = newHeight;
        }


        /// <summary>
        /// Create a new Bitmap with the specified dimensions.
        /// </summary>
        /// <param name="inputBmp">The source bitmap to use.</param>
        /// <param name="sourceBmpWidth">
        /// The width of the input bitmap. This should be equal to inputBmp.Size.Width, but it is added as
        /// a parameter so that calling code can send a cached value rather than requiring this method to query the bitmap for the
        /// data.
        /// If a value less than zero is specified, then inputBmp.Size.Width is used.
        /// </param>
        /// <param name="sourceBmpHeight">
        /// The height of the input bitmap. This should be equal to inputBmp.Size.Height, but it is added as
        /// a parameter so that calling code can send a cached value rather than requiring this method to query the bitmap for the
        /// data.
        /// </param>
        /// If a value less than zero is specified, then inputBmp.Size.Height is used.
        /// <param name="newWidth">The width of the new bitmap.</param>
        /// <param name="newHeight">The height of the new bitmap.</param>
        /// <returns>Returns a new Bitmap with the specified dimensions.</returns>
        /// <exception cref="GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException">
        /// Thrown when <paramref name="inputBmp" />
        /// cannot be resized to the requested dimensions. Typically this will occur during <see cref="Graphics.DrawImage" />
        /// because there is
        /// not enough system memory.
        /// </exception>
        public static Bitmap CreateResizedBitmap(Image inputBmp, int sourceBmpWidth, int sourceBmpHeight, int newWidth, int newHeight)
        {
            //Adapted (but mostly copied) from http://www.codeproject.com/cs/media/bitmapmanip.asp
            //Create a new bitmap object based on the input
            if (inputBmp == null)
                throw new ArgumentNullException("inputBmp");

            if (sourceBmpWidth <= 0)
                sourceBmpWidth = inputBmp.Size.Width;

            if (sourceBmpHeight <= 0)
                sourceBmpHeight = inputBmp.Size.Height;

            double xScaleFactor = newWidth / (float)sourceBmpWidth;
            double yScaleFactor = newHeight / (float)sourceBmpHeight;

            var calculatedNewWidth = (int)(sourceBmpWidth * xScaleFactor);
            var calculatedNewHeight = (int)(sourceBmpHeight * yScaleFactor);

            if (calculatedNewWidth <= 0)
            {
                calculatedNewWidth = 1; // Make sure the value is at least 1.
                xScaleFactor = calculatedNewWidth / (float)sourceBmpWidth; // Update the scale factor to reflect the new width
            }

            if (calculatedNewHeight <= 0)
            {
                calculatedNewHeight = 1; // Make sure the value is at least 1.
                yScaleFactor = calculatedNewHeight / (float)sourceBmpHeight; // Update the scale factor to reflect the new height
            }

            var newBmp = new Bitmap(calculatedNewWidth, calculatedNewHeight, PixelFormat.Format24bppRgb); //Graphics.FromImage doesn't like Indexed pixel format

            //Create a graphics object attached to the new bitmap
            using (var newBmpGraphics = Graphics.FromImage(newBmp))
            {
                newBmpGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                newBmpGraphics.FillRectangle(Brushes.White, 0, 0, sourceBmpWidth, sourceBmpHeight);

                newBmpGraphics.ScaleTransform((float)xScaleFactor, (float)yScaleFactor);

                //Draw the bitmap in the graphics object, which will apply the scale transform.
                //Note that pixel units must be specified to ensure the framework doesn't attempt
                //to compensate for varying horizontal resolutions in images by resizing; in this case,
                //that's the opposite of what we want.
                var drawRect = new Rectangle(0, 0, sourceBmpWidth, sourceBmpHeight);

                lock (inputBmp)
                {
                    try
                    {
                        try
                        {
                            newBmpGraphics.DrawImage(inputBmp, drawRect, drawRect, GraphicsUnit.Pixel);
                        }
                        catch (OutOfMemoryException)
                        {
                            // The garbage collector will automatically run to try to clean up memory, so let's wait for it to finish and 
                            // try again. If it still doesn't work because the image is just too large and the system doesn't have enough
                            // memory, catch the OutOfMemoryException and throw one of our UnsupportedImageTypeException exceptions instead.
                            GC.WaitForPendingFinalizers();
                            newBmpGraphics.DrawImage(inputBmp, drawRect, drawRect, GraphicsUnit.Pixel);
                        }
                    }
                    catch (OutOfMemoryException)
                    {
                        //throw new GalleryServerPro.ErrorHandler.CustomExceptions.UnsupportedImageTypeException();
                    }
                }
            }

            return newBmp;
        }

        /// <summary>
        /// Persist the specified image to disk at the specified path. If the directory to contain the file does not exist, it
        /// is automatically created.
        /// </summary>
        /// <param name="image">The image to persist to disk.</param>
        /// <param name="newFilePath">
        /// The full physical path, including the file name to where the image is to be stored. Ex:
        /// C:\mypics\cache\2008\May\flower.jpg
        /// </param>
        /// <param name="imageFormat">The file format for the image.</param>
        /// <param name="jpegQuality">
        /// The quality value to save JPEG images at. This is a value between 1 and 100. This parameter
        /// is ignored if the image format is not JPEG.
        /// </param>
        public static void SaveImageToDisk(Image image, string newFilePath, ImageFormat imageFormat, int jpegQuality)
        {
            VerifyDirectoryExistsForNewFile(newFilePath);

            if (imageFormat.Equals(ImageFormat.Jpeg))
                SaveJpgImageToDisk(image, newFilePath, jpegQuality);
            else
                SaveNonJpgImageToDisk(image, newFilePath, imageFormat);
        }

        private static void SaveJpgImageToDisk(Image image, string newFilepath, long jpegQuality)
        {
            //Save the image in the JPG format using the specified compression value.
            using (var eps = new EncoderParameters(1))
            {
                eps.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                var ici = GetEncoderInfo("image/jpeg");
                image.Save(newFilepath, ici, eps);
            }
        }

        /// <summary>
        /// Make sure the directory exists for the file at the specified path. It is created if it does not exist.
        /// (For example, it might not exist when the user changes the thumbnail or optimized location and subsequently
        /// synchronizes. This process creates a new directory structure to match the directory structure where the
        /// originals are stored, and there may be cases where we need to save a file to a directory that doesn't yet exist.
        /// </summary>
        /// <param name="newFilepath">
        /// The full physical path for which to verify the directory exists. Ex:
        /// C:\mypics\cache\2008\May\flower.jpg
        /// </param>
        private static void VerifyDirectoryExistsForNewFile(string newFilepath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(newFilepath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(newFilepath));
            }
        }

        private static void SaveNonJpgImageToDisk(Image image, string newFilepath, ImageFormat imgFormat)
        {
            image.Save(newFilepath, imgFormat);
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            //Get the image codec information for the specified mime type.
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (var j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static string GetFilePath(int photoId, bool forUrl, SueetiePhotoSize size, SueetiePhotoType _photoType)
        {
            string result = null;

            string filenameToken;
            if (size == SueetiePhotoSize.Full)
                filenameToken = "Lg";
            else if (size == SueetiePhotoSize.Medium)
                filenameToken = "Md";
            else if (size == SueetiePhotoSize.Small)
                filenameToken = "Sm";
            else
                filenameToken = "Org";

            var uri = HttpContext.Current.Request.Url;
            var rootUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port + "/";
            var uploadPath = @"/images/slideshows/";
            if (_photoType == SueetiePhotoType.ProductPhoto)
                uploadPath = @"/images/products/";

            if (forUrl)
            {
                result = string.Format("{0}/{1}/{2}.{3}.jpg", rootUrl, uploadPath, photoId, filenameToken);
            }
            else
            {
                var context = HttpContext.Current;
                if (context != null)
                {
                    var serverDirectory = context.Server.MapPath(uploadPath);
                    var file = string.Format("{0}.{1}.jpg", photoId, filenameToken);
                    result = Path.Combine(serverDirectory, file);
                }
            }

            return result;
        }

        public static bool DeleteLocalPhotoFiles(int imageID, SueetiePhotoType _photoType)
        {
            var result = false;
            try
            {
                var fullPhotoPath = GetFilePath(imageID, false, SueetiePhotoSize.Full, _photoType);
                var mediumPhotoPath = GetFilePath(imageID, false, SueetiePhotoSize.Medium, _photoType);
                var smallPhotoPath = GetFilePath(imageID, false, SueetiePhotoSize.Small, _photoType);
                var originalPhotoPath = GetFilePath(imageID, false, SueetiePhotoSize.Original, _photoType);

                DeleteFile(fullPhotoPath);
                DeleteFile(mediumPhotoPath);
                DeleteFile(smallPhotoPath);
                DeleteFile(originalPhotoPath);

                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public static void WriteToFile(string filename, byte[] bytes)
        {
            if (filename != null)
            {
                using (var full = File.Open(filename, FileMode.Create))
                {
                    full.Write(bytes, 0, bytes.Length);
                    full.Flush();
                }
            }
        }

        public static void DeleteFile(string filename)
        {
            if (filename != null)
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }
    }
}