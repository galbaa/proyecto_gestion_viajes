using Gevi.Api.Models;
using System;
using System.Collections.Generic;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IViajesManager
    {
        HttpResponse<ViajeResponse> NuevoViaje(ViajeRequest request);
        HttpResponse<ViajeResponse> ValidarViaje(ValidacionRequest request);
        HttpResponse<List<ViajeResponse>> Historial(ViajeRequest request);
        HttpResponse<List<ViajeResponse>> Todos();
        HttpResponse<List<ViajeResponse>> EntreFechas(DateTime inicio, DateTime fin);
    }
}
