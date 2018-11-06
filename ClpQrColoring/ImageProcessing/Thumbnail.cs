using System;
using System.Drawing;

namespace ClpQrColoring.ImageProcessing
{
    // https://www.dotnetperls.com/getthumbnailimage
    public class Thumbnail
    {
        // Maximum size of any dimension
        private const int MaxPixels = 100;

        private static Size GetThumbnailSize(Image original)
        {
            // Original width and height
            int originalWidth = original.Width;
            int originalHeight = original.Height;

            // Compute best factor to scale entire image based on larger dimension
            double scaleFactor;
            if (originalWidth > originalHeight)
            {
                scaleFactor = (double)MaxPixels / originalWidth;
            }
            else
            {
                scaleFactor = (double)MaxPixels / originalHeight;
            }

            // Return thumbnail size
            return new Size((int)(originalWidth * scaleFactor),
                (int)(originalHeight * scaleFactor));
        }

        public static void SaveThumbnailImage(string inputPath, string outputPath)
        {
            /* Important:
             * Always good practice to use the using structure
             * so that Image.Dispose() will always be called
             * to release the image file handle */

            // Load image
            using (Image originalImg = Image.FromFile(inputPath))
            {
                // Compute thumbnail size
                Size thumbnailSize = GetThumbnailSize(originalImg);

                // Get thumbnail
                using (Image thumbnail = originalImg.GetThumbnailImage(thumbnailSize.Width,
                    thumbnailSize.Height, null, IntPtr.Zero))
                {
                    // Save thumbnail
                    thumbnail.Save(outputPath);
                }
            }
        }
    }
}