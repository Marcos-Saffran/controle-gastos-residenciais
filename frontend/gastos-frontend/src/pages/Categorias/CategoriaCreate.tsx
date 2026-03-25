import { useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

export function CategoriaCreate() {
  const [descricao, setDescricao] = useState("");
  const [finalidade, setFinalidade] = useState(1);
  const navigate = useNavigate();

  async function salvar() {
    try {
      await api.post("/categorias", { descricao, finalidade });
      navigate("/categorias");
    } catch (error) {
      navigateToErrorPage(navigate, error);
    }
  }

  return (
    <div className="page-shell">
      <h2>Nova Categoria</h2>

      <input placeholder="Descrição" onChange={e => setDescricao(e.target.value)} />
      <div className="field-spacer" />

      <select value={finalidade} onChange={e => setFinalidade(Number(e.target.value))}>
        <option value="1">Despesa</option>
        <option value="2">Receita</option>
        <option value="3">Ambas</option>
      </select>
      <div className="field-spacer" />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}