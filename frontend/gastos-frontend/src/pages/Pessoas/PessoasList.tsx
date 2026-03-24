import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link } from "react-router-dom";

export function PessoasList() {
  const [pessoas, setPessoas] = useState<any[]>([]);

  useEffect(() => {
    api.get("/pessoas").then(res => setPessoas(res.data));
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h2>Pessoas</h2>
      <Link to="/pessoas/novo">Nova Pessoa</Link>

      <ul>
        {pessoas.map(p => (
          <li key={p.id}>
            {p.nome} ({p.idade} anos)
            <Link to={`/pessoas/editar/${p.id}`} style={{ marginLeft: 10 }}>
              Editar
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}