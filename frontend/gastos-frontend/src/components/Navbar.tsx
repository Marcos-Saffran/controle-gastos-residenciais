import { Link } from "react-router-dom";

export function Navbar() {
  return (
    <nav style={{ padding: 20, background: "#eee" }}>
      <Link to="/pessoas" style={{ marginRight: 20 }}>Pessoas</Link>
      <Link to="/categorias" style={{ marginRight: 20 }}>Categorias</Link>
      <Link to="/transacoes">Transações</Link>
    </nav>
  );
}