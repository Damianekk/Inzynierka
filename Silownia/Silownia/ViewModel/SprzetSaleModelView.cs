using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Silownia.Models;

namespace Silownia.ViewModel
{
    public class SprzetSaleModelView
    {
        public int SalaId { get; set; }
        public string Nazwa { get; set; }
        public int SprzetId { get; set; }
        public IEnumerable<Sala> Sale { get; set; }
        public IEnumerable<Sprzet> Sprzety { get; set; }

        public SprzetSaleModelView()
        {

        }
    }
}