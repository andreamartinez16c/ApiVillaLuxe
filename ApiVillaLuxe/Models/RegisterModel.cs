﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiVillaLuxe.Models
{
    public class RegisterModel
    {
        public Usuario Usuario { get; set; }
        public string Password { get; set; }

    }
}
