﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Responses
{
    public class GastoResponse
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Moneda { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public int ViajeId { get; set; }
        public TipoGasto Tipo { get; set; }
    }
}