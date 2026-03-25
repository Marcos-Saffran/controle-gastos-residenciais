import axios from "axios";
import type { NavigateFunction } from "react-router-dom";

export type ErrorPageState = {
  title: string;
  message: string;
  statusCode?: number;
};

type ProblemDetailsLike = {
  title?: string;
  detail?: string;
  status?: number;
};

export function buildErrorPageState(error: unknown): ErrorPageState {
  if (axios.isAxiosError(error)) {
    const problem = error.response?.data as ProblemDetailsLike | undefined;
    const statusCode = error.response?.status;

    return {
      title: problem?.title || "Falha na comunicação com a API",
      message:
        problem?.detail ||
        error.message ||
        "Não foi possível concluir a operação solicitada.",
      statusCode,
    };
  }

  if (error instanceof Error) {
    return {
      title: "Erro inesperado",
      message: error.message,
    };
  }

  return {
    title: "Erro inesperado",
    message: "Não foi possível concluir a operação solicitada.",
  };
}

export function navigateToErrorPage(
  navigate: NavigateFunction,
  error: unknown,
  replace = false
) {
  navigate("/erro", {
    replace,
    state: buildErrorPageState(error),
  });
}