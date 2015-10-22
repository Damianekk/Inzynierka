using Silownia.DAL;
using Silownia.Models;
using System;
using System.Text;

namespace Silownia
{
    public class Test
    {
        private SilowniaContext db = new SilowniaContext();
        public Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                Osoba klient = new Klient();
                klient.Imie = RandomString(5);
                klient.Nazwisko = RandomString(6);
                klient.DataUrodzenia = Convert.ToDateTime("1991-01-01");
               // db.Klienci.Add(klient);
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