using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Silownia.Helpers
{
    //Szyfr Rijndael - szyfrowanie symetryczne
    public class SzyfrowanieHasel
    {
          /* -----Deklaracja zmiennych-----
      * Deklaracja danych do szyfrowania, klucza oraz wektora 
      * inicjującego w postaci stringa i w postaci tablicy bajtów*/     
     private string data_str;
     private byte[] data_byte;
     private string _key;
     private string _iv;
     private byte[] key;
     private byte[] iv;
     /* Dlugosc klucza. */
     private int _KeyByteLength;

     /* -----Konstruktor-----
      * Konstruktor przyjmuje wartości klucza, wektora inicjującego oraz 
      * długość klucza, która dla tego algorytmu wyniesie 32 (256 bitów)*/
     public SzyfrowanieHasel(string key_val, string iv_val, int KeyByteLength)
     {
          _key = key_val;
          _iv = iv_val;
          _KeyByteLength=KeyByteLength;
          key = new byte[_KeyByteLength];
          iv = new byte[_KeyByteLength];

          /* Konwersja klucza i wektora inicjującego ze stringa do 
           * tablicy bajtów.*/ 
          for(int i=0;i < _key.Length;i++)
          {
               key[i] = Convert.ToByte(_key[i]);
          }
          for(int i=0;i < _iv.Length;i++)
          {
               iv[i] = Convert.ToByte(_iv[i]);
          }
     }
     
     /* -----Metoda szyfrujaca----- */
     public string Encrypt(string s)
     {
          /* Stworzenie obiektu Algorithm klasy RijndaelManaged*/
          RijndaelManaged Algorithm = new RijndaelManaged();
          /* Maksymalna dlugość wiadomości i klucza.*/
          Algorithm.BlockSize = _KeyByteLength*8;
          Algorithm.KeySize = _KeyByteLength*8;
          /* Instancja strumienia w pamięci.*/
          MemoryStream memStream = new MemoryStream();
          /* Stworzenie instancji Encryptor metodą obiektu Algorithm.*/
          ICryptoTransform Encryptor = Algorithm.CreateEncryptor(key,iv);
          /* Instancja strumienia szyfrującego.
           * Szyfrowanie to może być przeprowadzone bez pozostawienia 
           * niechcianego dostępu do pamięci. Szyfrowanie odbywa się w 
           * miarę odczytu z memStream.
           * Ostatni parametr wskazuje na tryb - w tym przypadku zapis.*/
          CryptoStream crStream = new CryptoStream(memStream, Encryptor, 
               CryptoStreamMode.Write);
          /* Stworzenie instancji StreamWriter, która będzie skojarzona  
           * ze strumieniem crStream.*/
          StreamWriter strWriter = new StreamWriter(crStream);
          /* Dopisanie stringa s do skojarzonego strumienia z strWriter, 
           * czyli crStream.*/
          strWriter.Write(s);
          /* Aktualizowanie źródła danych z aktualnym stanem bufora i 
           * czyszczenie bufora.*/
          strWriter.Flush();
          crStream.FlushFinalBlock();
          /* Zapisywanie w postaci tablicy bajtów zakodowanego tekstu.*/
          data_byte = new byte[memStream.Length];
          memStream.Position = 0;
          memStream.Read(data_byte,0,(int)data_byte.Length);
          /* Zapisanie danych przy użyciu klasy UnicodeEncoding. Pozwala 
           * to na przedstawienie stringa w postaci nieczytelnej dla 
           * uzytkownika. */
          data_str = new UnicodeEncoding().GetString(data_byte);
          return data_str;
     }

     /* -----Metoda deszyfrująca----- 
      * Działa analogicznie do szyfrującej. 
      * Używamy tu oczywiście metody CreateDecryptor zamiast
      * CreateEncryptor klasy RijndaelManaged. Strumień będziemy 
      * odczytywać za pomocą klasy StreamReader.*/
     public string Decrypt(string s)
     {
          RijndaelManaged Algorithm = new RijndaelManaged();
          Algorithm.BlockSize = _KeyByteLength*8;
          Algorithm.KeySize = _KeyByteLength*8;    
          MemoryStream memStream = new MemoryStream(new UnicodeEncoding().GetBytes(s));
          ICryptoTransform Decryptor = Algorithm.CreateDecryptor(key,iv);
          memStream.Position = 0;
          CryptoStream crStream = new CryptoStream(memStream,Decryptor,CryptoStreamMode.Read);
          StreamReader strReader = new StreamReader(crStream);
          UnicodeEncoding g = new UnicodeEncoding();
          return strReader.ReadToEnd();
     }
    }
}