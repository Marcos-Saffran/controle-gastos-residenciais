using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public interface ICategoriaService
    {
        Task AlterarDescricaoAsync(int categoriaId, string novaDescricao);
        Task AlterarFinalidadeAsync(int categoriaId, FinalidadeCategoria novaFinalidade);
    }

}
