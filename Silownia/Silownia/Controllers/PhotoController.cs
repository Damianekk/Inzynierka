using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Silownia.DAL;
using Silownia.Models;
using System.Drawing;
using System.Text;

namespace Silownia.Controllers
{
    public class PhotoController : Controller
    {
        private SilowniaContext db = new SilowniaContext();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index(Object osoba)
        {
     
            ViewBag.komuID = osoba;
            return View(osoba);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult PokazZapisaneZdjecie(long idOsoby)
        {
            
            
                Osoba os = db.Osoby.Find(idOsoby);
                if (os.ZdjecieProfilowe != null)
                {
                    ViewBag.pic = os.ZdjecieProfilowe;
                    return View();
                }
                else
                {
                   // TempData["msg"] = "<script>alert('Brak zdjęcia');</script>";
                    return null;
                }
   
        }


        [HttpPost]
        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            var idOsoby = Request.AppRelativeCurrentExecutionFilePath.Remove(0,Request.AppRelativeCurrentExecutionFilePath.LastIndexOf("/")+1);
            
            string dump;

            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();

                DateTime nm = DateTime.Now;

                string date = nm.ToString("yyyymmddMMss");

                var path = Server.MapPath("~/WebImages/" + date + "test.jpg");


                Osoba os =  db.Osoby.Find(Convert.ToInt64(idOsoby));
                os.ZdjecieProfilowe = (String_To_Bytes2(dump));
                db.SaveChanges();

              //  System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

              
            }

            return null;
        }

      
        private byte[] String_To_Bytes2(string strInput)
        {
        // allocate byte array based on half of string length
        int numBytes = (strInput.Length) / 2;
        byte[] bytes = new byte[numBytes];

        // loop through the string - 2 bytes at a time converting it to decimal equivalent and store in byte array
        // x variable used to hold byte array element position
        for(int x=0; x<numBytes; ++x)
        {
        bytes[x] = Convert.ToByte(strInput.Substring(x*2, 2), 16);
        }

        // return the finished byte array of decimal values
        return bytes;
        }

      

    }
}
