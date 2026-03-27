import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../../api/api";
import { navigateToErrorPage } from "../../api/handleApiError";

type CategoriaTotais = {
  categoriaId: number;
  descricao: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

type TotalGeral = {
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
};

type CategoriasTotaisResponse = {
  categorias: CategoriaTotais[];
  totalGeral: TotalGeral;
};

const moedaFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

export function CategoriasTotais() {
  const [dados, setDados] = useState<CategoriasTotaisResponse | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarTotais() {
      try {
        const response = await api.get<CategoriasTotaisResponse>("/categorias/totais");
        setDados(response.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarTotais();
  }, [navigate]);

  return (
    <div className="page-shell">
      <h2>Totais por Categoria</h2>

      {dados ? (
        <div className="table-wrap">
          <table className="totais-table">
            <thead>
              <tr>
                <th>Categoria</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {dados.categorias.map((categoria) => (
                <tr key={categoria.categoriaId}>
                  <td>{categoria.descricao}</td>
                  <td>{moedaFormatter.format(categoria.totalReceitas)}</td>
                  <td>{moedaFormatter.format(categoria.totalDespesas)}</td>
                  <td>{moedaFormatter.format(categoria.saldo)}</td>
                </tr>
              ))}
            </tbody>
            <tfoot>
              <tr>
                <td>Total geral</td>
                <td>{moedaFormatter.format(dados.totalGeral.totalReceitas)}</td>
                <td>{moedaFormatter.format(dados.totalGeral.totalDespesas)}</td>
                <td>{moedaFormatter.format(dados.totalGeral.saldoLiquido)}</td>
              </tr>
            </tfoot>
          </table>
        </div>
      ) : (
        <p>Carregando totais...</p>
      )}
    </div>
  );
}