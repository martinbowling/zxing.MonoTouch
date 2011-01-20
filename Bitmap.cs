using System;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Runtime.InteropServices;
namespace System.Drawing
{
	public class Bitmap
	{
		private UIImage _backingImage = null;
		byte[] pixelData = new byte[0];
		int width = 0;
		int height = 0;
		
		public Bitmap (UIImage image)
		{
			_backingImage = image;
			CGImage imageRef = _backingImage.CGImage;
			width = imageRef.Width;
			height = imageRef.Height;
			CGColorSpace colorSpace = CGColorSpace.CreateDeviceRGB();
			
			IntPtr rawData = Marshal.AllocHGlobal(height*width*4);
			CGContext context = new CGBitmapContext(
				rawData, width, height, 8, 4*width, colorSpace, CGImageAlphaInfo.PremultipliedLast
			);
			context.DrawImage(new RectangleF(0.0f,0.0f,(float)width,(float)height),imageRef);
			
			pixelData = new byte[height*width*4];
			Marshal.Copy(rawData,pixelData,0,pixelData.Length);
		}
		
		public Color GetPixel(int x, int y)
		{
			try {				
				byte bytesPerPixel = 4;
				int bytesPerRow = width * bytesPerPixel;
				int rowOffset = y * bytesPerRow;
				int colOffset = x * bytesPerPixel;
				int pixelDataLoc = rowOffset + colOffset;
				
				Color ret = Color.FromArgb(pixelData[pixelDataLoc+3],pixelData[pixelDataLoc+0],pixelData[pixelDataLoc+1],pixelData[pixelDataLoc+2]);
				return ret;
			} catch (Exception ex) {
				Console.WriteLine("Orig: {0}x{1}", _backingImage.Size.Width,_backingImage.Size.Height);
				Console.WriteLine("Req:  {0}x{1}", x, y);
				throw ex;
			}
			
			/*
    NSMutableArray *result = [NSMutableArray arrayWithCapacity:count];

    // First get the image into your data buffer
    CGImageRef imageRef = [image CGImage];
    NSUInteger width = CGImageGetWidth(imageRef);
    NSUInteger height = CGImageGetHeight(imageRef);
    CGColorSpaceRef colorSpace = CGColorSpaceCreateDeviceRGB();
    unsigned char *rawData = malloc(height * width * 4);
    NSUInteger bytesPerPixel = 4;
    NSUInteger bytesPerRow = bytesPerPixel * width;
    NSUInteger bitsPerComponent = 8;
    CGContextRef context = CGBitmapContextCreate(rawData, width, height,
                    bitsPerComponent, bytesPerRow, colorSpace,
                    kCGImageAlphaPremultipliedLast | kCGBitmapByteOrder32Big);
    CGColorSpaceRelease(colorSpace);

    CGContextDrawImage(context, CGRectMake(0, 0, width, height), imageRef);
    CGContextRelease(context);

    // Now your rawData contains the image data in the RGBA8888 pixel format.
    int byteIndex = (bytesPerRow * yy) + xx * bytesPerPixel;
    for (int ii = 0 ; ii < count ; ++ii)
    {
        CGFloat red   = (rawData[byteIndex]     * 1.0) / 255.0;
        CGFloat green = (rawData[byteIndex + 1] * 1.0) / 255.0;
        CGFloat blue  = (rawData[byteIndex + 2] * 1.0) / 255.0;
        CGFloat alpha = (rawData[byteIndex + 3] * 1.0) / 255.0;
        byteIndex += 4;

        UIColor *acolor = [UIColor colorWithRed:red green:green blue:blue alpha:alpha];
        [result addObject:acolor];
    }

  free(rawData);

  return result;
			 * */
			return Color.FromArgb(0,0,0,0);
		}
	}
}

