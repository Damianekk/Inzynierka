﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Silownia.Models
{
    [Table("TreningWłasny")]
    public class TreningWlasny : Trening
    {
        public long TreningWlasnyID { get; set; }

        [Required]
        public virtual Klient Klient { get; set; }

    }
}