import { Link } from "react-router-dom";

export function Navbar() {
  return (
    <nav className="app-navbar">
      <Link to="/pessoas" className="nav-link">Pessoas</Link>
      <Link to="/pessoas/totais" className="nav-link">Totais Pessoas</Link>
      <Link to="/categorias" className="nav-link">Categorias</Link>
      <Link to="/categorias/totais" className="nav-link">Totais Categorias</Link>
      <Link to="/transacoes" className="nav-link">Transações</Link>
    </nav>
  );
}