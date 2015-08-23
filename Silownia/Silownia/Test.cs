using Silownia.DAL;
using Silownia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Silownia
{
    public class Test
    {
        private SilowniaContext db = new SilowniaContext();
        public Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                Osoba osoba = new Klient();
                osoba.Imie = RandomString(5);
                osoba.Nazwisko = RandomString(6);
                osoba.DataUrodzenia = Convert.ToDateTime("1991-01-01");
                db.Osoby.Add(osoba);
                db.SaveChanges();
                
            }
           
        }
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}