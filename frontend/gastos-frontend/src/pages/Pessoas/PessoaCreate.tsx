import { useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

export function PessoaCreate() {
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState(0);
  const navigate = useNavigate();

  async function salvar() {
    try {
      await api.post("/pessoas", { nome, idade });
      navigate("/pessoas");
    } catch (error) {
      navigateToErrorPage(navigate, error);
    }
  }

  return (
    <div className="page-shell">
      <h2>Nova Pessoa</h2>

      <input placeholder="Nome" onChange={e => setNome(e.target.value)} />
      <div className="field-spacer" />

      <input
        type="number"
        placeholder="Idade"
        onChange={e => setIdade(Number(e.target.value))}
      />
      <div className="field-spacer" />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}