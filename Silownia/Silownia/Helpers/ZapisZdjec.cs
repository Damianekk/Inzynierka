using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Silownia.Models;
using System.IO;
using Silownia.DAL;

namespace Silownia.Helpers
{
    public class ZapisZdjec
    {
        private SilowniaContext db = new SilowniaContext();
        public void ZapiszPlik(HttpPostedFileBase file)
        {
            Sala noweFoto = new Sala();
            noweFoto.FotoLokalizacja = file.ContentType;
            noweFoto.FotoBytes = ConvertToBytes(file);

            db.Sale.Add(noweFoto);
            db.SaveChanges();

        }


        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] FotoBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            FotoBytes = reader.ReadBytes((int)file.ContentLength);

            return FotoBytes;
        }
    }
}