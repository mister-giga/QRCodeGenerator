using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using QRCoder;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace QRCodeGenerator.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetQr(string v)
        {
            Response.Headers.Add("cache-control", new Microsoft.Extensions.Primitives.StringValues(new string[] { "public", "max-age=604800" }));
            return File(GetQrBytes(v), "image/png");

        }

        private static byte[] GetQrBytes(string qrContent)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCoder.QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(30);

            using(MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }


    }
}
