import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import { Navbar } from "./components/Navbar";
import { ErrorPage } from "./pages/ErrorPage";

import { PessoasList } from "./pages/Pessoas/PessoasList";
import { PessoaCreate } from "./pages/Pessoas/PessoaCreate";
import { PessoaEdit } from "./pages/Pessoas/PessoaEdit";
import { PessoasTotais } from "./pages/Pessoas/PessoasTotais";

import { CategoriasList } from "./pages/Categorias/CategoriasList";
import { CategoriaCreate } from "./pages/Categorias/CategoriaCreate";

import { TransacoesList } from "./pages/Transacoes/TransacoesList";
import { TransacaoCreate } from "./pages/Transacoes/TransacaoCreate";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/" element={<Navigate to="/pessoas" replace />} />

        {/* Pessoas */}
        <Route path="/pessoas" element={<PessoasList />} />
        <Route path="/pessoas/novo" element={<PessoaCreate />} />
        <Route path="/pessoas/editar/:id" element={<PessoaEdit />} />
        <Route path="/pessoas/totais" element={<PessoasTotais />} />

        {/* Categorias */}
        <Route path="/categorias" element={<CategoriasList />} />
        <Route path="/categorias/novo" element={<CategoriaCreate />} />

        {/* Transações */}
        <Route path="/transacoes" element={<TransacoesList />} />
        <Route path="/transacoes/novo" element={<TransacaoCreate />} />

        <Route path="/erro" element={<ErrorPage />} />
        <Route path="*" element={<ErrorPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;