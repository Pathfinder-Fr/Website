namespace Sueetie.Commerce
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using Sueetie.Core;

    public static class ProductPhotos
    {
        public static int AddPhoto(ProductPhoto productPhoto)
        {
            int num = -1;
            num = CommerceDataProvider.LoadProvider().AddPhoto(productPhoto);
            ClearProductPhotoListCache();
            return num;
        }

        public static void ClearProductPhotoListCache()
        {
            SueetieCache.Current.Remove(ProductPhotoListCacheKey());
        }

        public static void DeletePhoto(int photoID)
        {
            CommerceDataProvider.LoadProvider().DeletePhoto(photoID);
            ImageHelper.DeleteLocalPhotoFiles(photoID, SueetiePhotoType.ProductPhoto);
            ClearProductPhotoListCache();
        }

        public static List<ProductPhoto> GetPhotosByProduct(int productID)
        {
            return (from p in GetProductPhotoList()
                    where p.ProductID == productID
                    select p).ToList<ProductPhoto>();
        }

        public static List<ProductPhoto> GetProductPhotoList()
        {
            string key = ProductPhotoListCacheKey();
            List<ProductPhoto> productPhotoList = SueetieCache.Current[key] as List<ProductPhoto>;
            if (productPhotoList == null)
            {
                productPhotoList = CommerceDataProvider.LoadProvider().GetProductPhotoList();
                SueetieCache.Current.Insert(key, productPhotoList);
            }
            return productPhotoList;
        }

        /// <summary>
        /// Insert a photo as product pictures.
        /// </summary>
        /// <remarks>
        /// This method is called directly by UI using ASP.NET ObjectDataSource.
        /// </remarks>
        public static int InsertPhoto(int productID, byte[] bytesFull, byte[] bytesMedium, byte[] bytesSmall, bool useAsPreview)
        {
            int photoId = -1;

            if (bytesFull != null && bytesMedium != null && bytesSmall != null)
            {
                var productPhoto = new ProductPhoto
                {
                    ProductID = productID,
                    IsMainPreview = useAsPreview
                };

                photoId = AddPhoto(productPhoto);

                string fullSizePath = ImageHelper.GetFilePath(photoId, false, SueetiePhotoSize.Full, SueetiePhotoType.ProductPhoto);
                string mediumSizePath = ImageHelper.GetFilePath(photoId, false, SueetiePhotoSize.Medium, SueetiePhotoType.ProductPhoto);
                string smallSizePath = ImageHelper.GetFilePath(photoId, false, SueetiePhotoSize.Small, SueetiePhotoType.ProductPhoto);

                ImageHelper.WriteToFile(fullSizePath, bytesFull);
                ImageHelper.WriteToFile(mediumSizePath, bytesMedium);
                ImageHelper.WriteToFile(smallSizePath, bytesSmall);

                CommerceCommon.ClearMarketplaceCache();
            }

            return photoId;
        }

        public static string ProductPhotoListCacheKey()
        {
            return string.Format("ProductPhotoList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static byte[] ResizeImageFile(byte[] imageFile, SueetiePhotoSize size)
        {
            byte[] buffer;

            using (var sourceImage = Image.FromStream(new MemoryStream(imageFile)))
            {
                int height;
                int width;

                if (size == SueetiePhotoSize.Small || size == SueetiePhotoSize.Medium)
                {
                    if (size == SueetiePhotoSize.Small)
                    {
                        width = CommerceSettings.Instance.FixedSmallImageWidth;
                    }
                    else
                    {
                        width = CommerceSettings.Instance.FixedMediumImageWidth;
                    }

                    height = (int)(sourceImage.Height * (((float)width) / ((float)sourceImage.Width)));
                }
                else if (sourceImage.Height > sourceImage.Width)
                {
                    height = Math.Min(sourceImage.Height, CommerceSettings.Instance.MaxFullImageSize);
                    width = (int)(sourceImage.Width * (((float)height) / ((float)sourceImage.Height)));
                }
                else
                {
                    width = Math.Min(sourceImage.Width, CommerceSettings.Instance.MaxFullImageSize);
                    height = (int)(sourceImage.Height * (((float)width) / ((float)sourceImage.Width)));
                }

                using (var targetImage = Image.FromStream(new MemoryStream(imageFile)))
                {
                    using (var bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb))
                    {
                        bitmap.SetResolution(72f, 72f);

                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            graphics.DrawImage(targetImage, new Rectangle(0, 0, width, height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel);
                            MemoryStream stream = new MemoryStream();
                            bitmap.Save(stream, ImageFormat.Jpeg);
                            buffer = stream.GetBuffer();
                        }
                    }
                }
            }
            return buffer;
        }

        public static void SetAdPreviewPhoto(int productID, int photoID)
        {
            CommerceDataProvider provider = CommerceDataProvider.LoadProvider();
            ProductPhoto productPhoto = new ProductPhoto
            {
                PhotoID = photoID,
                ProductID = productID
            };
            provider.SetPreviewPhoto(productPhoto);
            CommerceCommon.ClearMarketplaceCache();
        }
    }
}

