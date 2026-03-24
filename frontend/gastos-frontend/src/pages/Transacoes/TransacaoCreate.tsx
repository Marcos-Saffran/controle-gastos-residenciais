import { useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";

export function TransacaoCreate() {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState(0);
  const [tipo, setTipo] = useState(0);
  const [categoriaId, setCategoriaId] = useState(0);
  const [pessoaId, setPessoaId] = useState(0);

  const navigate = useNavigate();

  function salvar() {
    api.post("/transacoes", {
      descricao,
      valor,
      tipo,
      categoriaId,
      pessoaId
    }).then(() => navigate("/transacoes"));
  }

  return (
    <div style={{ padding: 20 }}>
      <h2>Nova Transação</h2>

      <input placeholder="Descrição" onChange={e => setDescricao(e.target.value)} />
      <br /><br />

      <input type="number" placeholder="Valor" onChange={e => setValor(Number(e.target.value))} />
      <br /><br />

      <select onChange={e => setTipo(Number(e.target.value))}>
        <option value="0">Despesa</option>
        <option value="1">Receita</option>
      </select>
      <br /><br />

      <input type="number" placeholder="Categoria ID" onChange={e => setCategoriaId(Number(e.target.value))} />
      <br /><br />

      <input type="number" placeholder="Pessoa ID" onChange={e => setPessoaId(Number(e.target.value))} />
      <br /><br />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}