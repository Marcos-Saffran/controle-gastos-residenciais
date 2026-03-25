import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link, useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

export function PessoasList() {
  const [pessoas, setPessoas] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarPessoas() {
      try {
        const res = await api.get("/pessoas");
        setPessoas(res.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarPessoas();
  }, [navigate]);

  async function excluirPessoa(id: number) {
    const confirmou = globalThis.confirm("Deseja realmente excluir esta pessoa?");

    if (!confirmou) {
      return;
    }

    try {
      await api.delete(`/pessoas/${id}`);
      setPessoas(listaAtual => listaAtual.filter(pessoa => pessoa.id !== id));
    } catch (error) {
      navigateToErrorPage(navigate, error);
    }
  }

  return (
    <div className="page-shell">
      <h2>Pessoas</h2>
      <Link to="/pessoas/novo">Nova Pessoa</Link>
      <Link to="/pessoas/totais" className="action-link">Ver Totais por Pessoa</Link>

      <ul>
        {pessoas.map(p => (
          <li key={p.id}>
            {p.nome} ({p.idade} anos)
            <Link to={`/pessoas/editar/${p.id}`} className="action-link">
              Editar
            </Link>
            <button type="button" className="action-button" onClick={() => excluirPessoa(p.id)}>
              Excluir
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}