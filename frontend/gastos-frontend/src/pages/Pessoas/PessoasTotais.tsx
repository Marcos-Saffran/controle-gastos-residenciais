import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../../api/api";
import { navigateToErrorPage } from "../../api/handleApiError";

type PessoaTotais = {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
};

type TotalGeral = {
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
};

type PessoasTotaisResponse = {
  pessoas: PessoaTotais[];
  totalGeral: TotalGeral;
};

const moedaFormatter = new Intl.NumberFormat("pt-BR", {
  style: "currency",
  currency: "BRL",
});

export function PessoasTotais() {
  const [dados, setDados] = useState<PessoasTotaisResponse | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarTotais() {
      try {
        const response = await api.get<PessoasTotaisResponse>("/pessoas/totais");
        setDados(response.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarTotais();
  }, [navigate]);

  return (
    <div className="page-shell">
      <h2>Totais por Pessoa</h2>

      {dados ? (
        <div className="table-wrap">
          <table className="totais-table">
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Receitas</th>
                <th>Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {dados.pessoas.map((pessoa) => (
                <tr key={pessoa.pessoaId}>
                  <td>{pessoa.nome}</td>
                  <td>{moedaFormatter.format(pessoa.totalReceitas)}</td>
                  <td>{moedaFormatter.format(pessoa.totalDespesas)}</td>
                  <td>{moedaFormatter.format(pessoa.saldo)}</td>
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