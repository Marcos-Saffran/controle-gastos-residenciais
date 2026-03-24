import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { useNavigate, useParams } from "react-router-dom";

export function PessoaEdit() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState(0);

  useEffect(() => {
    api.get(`/pessoas/${id}`).then(res => {
      setNome(res.data.nome);
      setIdade(res.data.idade);
    });
  }, [id]);

  function salvar() {
    api.put(`/pessoas/${id}/nome`, { nome });
    api.put(`/pessoas/${id}/idade`, { idade }).then(() => navigate("/pessoas"));
  }

  return (
    <div style={{ padding: 20 }}>
      <h2>Editar Pessoa</h2>

      <input value={nome} onChange={e => setNome(e.target.value)} />
      <br /><br />

      <input
        type="number"
        value={idade}
        onChange={e => setIdade(Number(e.target.value))}
      />
      <br /><br />

      <button onClick={salvar}>Salvar</button>
    </div>
  );
}