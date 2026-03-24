import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link } from "react-router-dom";

export function TransacoesList() {
  const [transacoes, setTransacoes] = useState<any[]>([]);

  useEffect(() => {
    api.get("/transacoes").then(res => setTransacoes(res.data));
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h2>Transações</h2>
      <Link to="/transacoes/novo">Nova Transação</Link>

      <ul>
        {transacoes.map(t => (
          <li key={t.id}>
            {t.descricao} — R$ {t.valor} — Tipo: {t.tipo}
          </li>
        ))}
      </ul>
    </div>
  );
}