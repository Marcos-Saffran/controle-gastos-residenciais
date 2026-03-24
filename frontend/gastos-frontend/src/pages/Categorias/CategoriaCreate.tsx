import { useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";

export function CategoriaCreate() {
  const [descricao, setDescricao] = useState("");
  const [finalidade, setFinalidade] = useState(0);
  const navigate = useNavigate();

  function salvar() {
    api.post("/categorias", { descricao, finalidade })
       .then(() => navigate("/categorias"));
  }

  return (
    <div style={{ padding: 20 }}>
      <h2>Nova Categoria</h2>

      <input placeholder="Descrição" onChange={e => setDescricao(e.target.value)} />
      <br /><br />

      <select onChange={e => setFinalidade(Number(e.target.value))}>
        <option value="0">Despesa</option>
        <option value="1">Receita</option>
      </select>
      <br /><br />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}