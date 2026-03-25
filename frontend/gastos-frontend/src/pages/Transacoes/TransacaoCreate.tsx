import { useEffect, useMemo, useState } from "react";
import { api } from "../../api/api";
import { useNavigate } from "react-router-dom";
import { navigateToErrorPage } from "../../api/handleApiError";

type Categoria = {
  id: number;
  descricao: string;
  finalidade: number;
};

type Pessoa = {
  id: number;
  nome: string;
};

export function TransacaoCreate() {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState(0);
  const [tipo, setTipo] = useState(1);
  const [categoriaId, setCategoriaId] = useState(0);
  const [pessoaId, setPessoaId] = useState(0);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);

  const navigate = useNavigate();

  const categoriasFiltradas = useMemo(() => {
    const finalidadePermitida = tipo === 1 ? [1, 3] : [2, 3];
    return categorias.filter(categoria => finalidadePermitida.includes(categoria.finalidade));
  }, [categorias, tipo]);

  const valorInvalido = valor <= 0;

  useEffect(() => {
    async function carregarDados() {
      try {
        const [categoriasRes, pessoasRes] = await Promise.all([
          api.get("/categorias"),
          api.get("/pessoas"),
        ]);

        setCategorias(categoriasRes.data);
        setPessoas(pessoasRes.data);
      } catch (error) {
        navigateToErrorPage(navigate, error, true);
      }
    }

    carregarDados();
  }, [navigate]);

  useEffect(() => {
    if (categoriaId <= 0) {
      return;
    }

    const categoriaAindaPermitida = categoriasFiltradas.some(categoria => categoria.id === categoriaId);

    if (!categoriaAindaPermitida) {
      setCategoriaId(0);
    }
  }, [categoriaId, categoriasFiltradas]);

  async function salvar() {
    if (valorInvalido) {
      return;
    }

    try {
      await api.post("/transacoes", {
        descricao,
        valor,
        tipo,
        categoriaId,
        pessoaId
      });

      navigate("/transacoes");
    } catch (error) {
      navigateToErrorPage(navigate, error);
    }
  }

  return (
    <div className="page-shell">
      <h2>Nova Transação</h2>

      <input placeholder="Descrição" onChange={e => setDescricao(e.target.value)} />
      <div className="field-spacer" />

      <input
        type="number"
        placeholder="Valor"
        min="0.01"
        step="0.01"
        value={valor}
        onChange={e => setValor(Number(e.target.value))}
      />
      {valorInvalido ? <p className="validation-error">O valor da transação deve ser maior que zero.</p> : null}
      <div className="field-spacer" />

      <select value={tipo} onChange={e => setTipo(Number(e.target.value))}>
        <option value="1">Despesa</option>
        <option value="2">Receita</option>
      </select>
      <div className="field-spacer" />

      <select value={categoriaId} onChange={e => setCategoriaId(Number(e.target.value))}>
        <option value="0">Selecione uma categoria</option>
        {categoriasFiltradas.map(categoria => (
          <option key={categoria.id} value={categoria.id}>
            {categoria.descricao}
          </option>
        ))}
      </select>
      <div className="field-spacer" />

      <select value={pessoaId} onChange={e => setPessoaId(Number(e.target.value))}>
        <option value="0">Selecione uma pessoa</option>
        {pessoas.map(pessoa => (
          <option key={pessoa.id} value={pessoa.id}>
            {pessoa.nome}
          </option>
        ))}
      </select>
      <div className="field-spacer" />

      <button onClick={salvar} disabled={categoriaId <= 0 || pessoaId <= 0 || valorInvalido}>Salvar</button>
    </div>
  );
}