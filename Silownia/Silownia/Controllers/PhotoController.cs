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
        private long? komuID;
        [HttpGet]
        public ActionResult Index(Object osoba)
        {
            Session["val"] = "";
            ViewBag.komuID = osoba;
            return View(osoba);
        }

        [HttpPost]
        public ActionResult Index(string Imagename)
        {
            string sss = Session["val"].ToString();

            ViewBag.pic = "http://localhost:55694/WebImages/" + Session["val"].ToString();

            return View();
        }

        [HttpGet]
        public ActionResult Changephoto()
        {        
                Osoba os = db.Osoby.Find(1);
                ViewBag.pic = os.ZdjecieProfilowe ;
            return View();
        }



        public JsonResult Rebind()
        {
            string path = "http://localhost:55694/WebImages/" + Session["val"].ToString();

            return Json(path, JsonRequestBehavior.AllowGet);
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

            return View("Index");
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

     //   Bytes_To_String on the other hand, is more of a mess:

        // convert the byte array back to a true string
        private string Bytes_To_String2(byte[] bytes_Input)
        {
        StringBuilder strTemp = new StringBuilder(bytes_Input.Length *2);
        foreach(byte b in bytes_Input)
        {
        strTemp.Append(b.ToString("X02"));
        }
        return strTemp.ToString();
        }

    

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }


    }
}
