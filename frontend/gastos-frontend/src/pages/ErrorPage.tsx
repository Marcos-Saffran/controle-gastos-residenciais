import { Link, useLocation } from "react-router-dom";
import type { ErrorPageState } from "../api/handleApiError";

const fallbackState: ErrorPageState = {
  title: "Página indisponível",
  message: "A página solicitada não foi encontrada ou houve uma falha ao carregá-la.",
};

export function ErrorPage() {
  const location = useLocation();
  const state = (location.state as ErrorPageState | null) ?? fallbackState;

  return (
    <main className="error-page">
      <div className="error-card">
        <p className="error-eyebrow">Falha de navegação</p>
        <h1>{state.title}</h1>
        {state.statusCode ? <span className="error-status">HTTP {state.statusCode}</span> : null}
        <p>{state.message}</p>

        <div className="error-actions">
          <Link to="/pessoas" className="error-link primary-link">Voltar para pessoas</Link>
          <Link to="/transacoes" className="error-link secondary-link">Ir para transações</Link>
        </div>
      </div>
    </main>
  );
}