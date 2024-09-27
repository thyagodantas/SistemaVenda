﻿using System.ComponentModel.DataAnnotations;

namespace SistemaVenda.Entidades
{
    public class Usuario
    {
        [Key]
        public int? Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }

    }
}
