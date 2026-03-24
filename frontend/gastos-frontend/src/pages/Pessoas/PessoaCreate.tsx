import { useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";

export function PessoaCreate() {
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState(0);
  const navigate = useNavigate();

  function salvar() {
    api.post("/pessoas", { nome, idade }).then(() => navigate("/pessoas"));
  }

  return (
    <div style={{ padding: 20 }}>
      <h2>Nova Pessoa</h2>

      <input placeholder="Nome" onChange={e => setNome(e.target.value)} />
      <br /><br />

      <input
        type="number"
        placeholder="Idade"
        onChange={e => setIdade(Number(e.target.value))}
      />
      <br /><br />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}