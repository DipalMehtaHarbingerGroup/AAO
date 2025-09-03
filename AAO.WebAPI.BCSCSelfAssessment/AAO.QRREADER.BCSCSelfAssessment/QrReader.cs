using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZXing;

namespace AAO.QRREADER.BCSCSelfAssessment
{
    public static class QrReader
    {
        public static string ReadQrCode(string Imagepath)
        {
            var result = string.Empty;
            try
            {
                // create a barcode reader instance
                var barcodeReader = new BarcodeReader();

                // create an in memory bitmap
                var barcodeBitmap = (Bitmap)Bitmap.FromFile(Imagepath);

                // decode the barcode from the in memory bitmap
                var barcodeResult = barcodeReader.Decode(barcodeBitmap);

                if (barcodeResult != null)
                {
                    result = barcodeResult.Text;
                }
            }
            catch (Exception ex)
            {
                return "Error while generating barcode" + ex;
            }

            return result;
        }
    }
}
