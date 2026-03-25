import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link, useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

type Transacao = {
  id: number;
  descricao: string;
  valor: number;
  tipo: number;
  pessoaId: number;
};

type Pessoa = {
  id: number;
  nome: string;
};

const tipoLabels: Record<number, string> = {
  1: "Despesa",
  2: "Receita",
};

export function TransacoesList() {
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarTransacoes() {
      try {
        const [transacoesRes, pessoasRes] = await Promise.all([
          api.get("/transacoes"),
          api.get("/pessoas"),
        ]);

        setTransacoes(transacoesRes.data);
        setPessoas(pessoasRes.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarTransacoes();
  }, [navigate]);

  return (
    <div className="page-shell">
      <h2>Transações</h2>
      <Link to="/transacoes/novo">Nova Transação</Link>

      <ul>
        {transacoes.map(t => {
          const pessoaNome = pessoas.find(p => p.id === t.pessoaId)?.nome ?? "Pessoa não encontrada";
          const tipoNome = tipoLabels[t.tipo] ?? "Tipo desconhecido";

          return (
            <li key={t.id}>
              {t.descricao} — R$ {t.valor} — Tipo: {tipoNome} — Pessoa: {pessoaNome}
            </li>
          );
        })}
      </ul>
    </div>
  );
}