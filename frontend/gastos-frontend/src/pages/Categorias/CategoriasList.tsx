import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link } from "react-router-dom";

export function CategoriasList() {
  const [categorias, setCategorias] = useState<any[]>([]);

  useEffect(() => {
    api.get("/categorias").then(res => setCategorias(res.data));
  }, []);

  return (
    <div style={{ padding: 20 }}>
      <h2>Categorias</h2>
      <Link to="/categorias/novo">Nova Categoria</Link>

      <ul>
        {categorias.map(c => (
          <li key={c.id}>
            {c.descricao} (Finalidade: {c.finalidade})
          </li>
        ))}
      </ul>
    </div>
  );
}