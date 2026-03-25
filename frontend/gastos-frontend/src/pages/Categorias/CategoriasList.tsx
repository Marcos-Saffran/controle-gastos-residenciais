import { useEffect, useState } from "react";
import { api } from "../../api/api";
import { Link, useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

const finalidadeLabels: Record<number, string> = {
  1: "Despesa",
  2: "Receita",
  3: "Ambas",
};

export function CategoriasList() {
  const [categorias, setCategorias] = useState<any[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    async function carregarCategorias() {
      try {
        const res = await api.get("/categorias");
        setCategorias(res.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarCategorias();
  }, [navigate]);

  return (
    <div className="page-shell">
      <h2>Categorias</h2>
      <Link to="/categorias/novo">Nova Categoria</Link>

      <ul>
        {categorias.map(c => (
          <li key={c.id}>
            {c.descricao} (Finalidade: {finalidadeLabels[c.finalidade] ?? "Desconhecida"})
          </li>
        ))}
      </ul>
    </div>
  );
}