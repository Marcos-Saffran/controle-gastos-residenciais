import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { useNavigate, useParams } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

export function PessoaEdit() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState(0);

  useEffect(() => {
    async function carregarPessoa() {
      try {
        const res = await api.get(`/pessoas/${id}`);
        setNome(res.data.nome);
        setIdade(res.data.idade);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarPessoa();
  }, [id, navigate]);

  async function salvar() {
    try {
      await api.put(`/pessoas/${id}/nome`, { nome });
      await api.put(`/pessoas/${id}/idade`, { idade });
      navigate("/pessoas");
    } catch (error) {
      navigateToErrorPage(navigate, error);
    }
  }

  return (
    <div className="page-shell">
      <h2>Editar Pessoa</h2>

      <input placeholder="Nome" value={nome} onChange={e => setNome(e.target.value)} />
      <div className="field-spacer" />

      <input
        type="number"
        placeholder="Idade"
        value={idade}
        onChange={e => setIdade(Number(e.target.value))}
      />
      <div className="field-spacer" />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}