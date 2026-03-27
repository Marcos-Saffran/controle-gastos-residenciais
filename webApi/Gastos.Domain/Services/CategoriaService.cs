using Gastos.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gastos.Domain.Services
{
    public class CategoriaService : ICategoriaService
    {
        public Task AlterarDescricaoAsync(int categoriaId, string novaDescricao)
        {
            //todo: implementar regras de validação para alteração de descrição (ex: descrição não pode ser vazia)
            throw new NotImplementedException();
        }

        public Task AlterarFinalidadeAsync(int categoriaId, FinalidadeCategoria novaFinalidade)
        {
            //todo: implementar regras de validação para alteração de finalidade (ex: não permitir alteração para categorias que já possuem transações associadas)
            throw new NotImplementedException();
        }
    }
}
