import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Navbar } from "./components/Navbar";

import { PessoasList } from "./pages/Pessoas/PessoasList";
import { PessoaCreate } from "./pages/Pessoas/PessoaCreate";
import { PessoaEdit } from "./pages/Pessoas/PessoaEdit";

import { CategoriasList } from "./pages/Categorias/CategoriasList";
import { CategoriaCreate } from "./pages/Categorias/CategoriaCreate";

import { TransacoesList } from "./pages/Transacoes/TransacoesList";
import { TransacaoCreate } from "./pages/Transacoes/TransacaoCreate";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        {/* Pessoas */}
        <Route path="/pessoas" element={<PessoasList />} />
        <Route path="/pessoas/novo" element={<PessoaCreate />} />
        <Route path="/pessoas/editar/:id" element={<PessoaEdit />} />

        {/* Categorias */}
        <Route path="/categorias" element={<CategoriasList />} />
        <Route path="/categorias/novo" element={<CategoriaCreate />} />

        {/* Transações */}
        <Route path="/transacoes" element={<TransacoesList />} />
        <Route path="/transacoes/novo" element={<TransacaoCreate />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;