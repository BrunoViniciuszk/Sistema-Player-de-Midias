import { useStream } from "luffie";
import { notification } from "antd";
import { state$, authActions } from "../stores/authStore";
import { loginRequest, registerRequest } from "../api/auth";
import { useNavigate } from "react-router-dom";

export const useAuth = () => {
  const auth = useStream(state$) ?? {
    token: null,
    userName: null,
    loading: false,
    isAuthenticated: false,
  };

  const navigate = useNavigate();

  const login = async (username: string, password: string) => {
    authActions.setLoading(true);

    try {
      const res = await loginRequest({ username, password });

      authActions.loginSuccess(res.data.token, res.data.name ?? username);

      notification.success({
        message: "Login realizado",
        description: `Bem-vindo de volta, ${res.data.name ?? username}! 🚀`,
        placement: "topRight",
        duration: 4,
        style: { zIndex: 9999 },
      });

      navigate("/admin", { replace: true });
    } catch (e: any) {
      console.error("Erro no login:", e);

      const status = e?.response?.status;
      const backendMessage = e?.response?.data?.message;

      notification.error({
        message: "Falha no login",
        description:
          backendMessage ||
          (status === 401
            ? "Usuário ou senha inválidos. Verifique e tente novamente."
            : "Erro inesperado no servidor."),
        placement: "topRight",
        duration: 4,
        style: { zIndex: 9999 },
      });
    } finally {
      authActions.setLoading(false);
    }
  };

  const register = async (username: string, password: string) => {
    authActions.setLoading(true);

    try {
      const res = await registerRequest({ username, password });

      notification.success({
        message: "Cadastro realizado",
        description:
          "Cadastro realizado com sucesso! Faça login para continuar.",
        placement: "topRight",
        duration: 4,
        style: { zIndex: 9999 },
      });

      authActions.setLoading(false);
      navigate("/login", { replace: true });
    } catch (e: any) {
      const backendMessage = e?.response?.data?.message;

      notification.error({
        message: "Falha no cadastro",
        description: backendMessage || "Erro inesperado no servidor.",
        placement: "topRight",
        duration: 4,
        style: { zIndex: 9999 },
      });

      authActions.setLoading(false);
    }
  };

  const logout = () => {
    authActions.logout();

    notification.info({
      message: "Logout",
      description: "Você saiu da aplicação.",
      placement: "topRight",
      duration: 4,
      style: { zIndex: 9999 },
    });

    navigate("/login", { replace: true });
  };

  return { ...auth, login, register, logout };
};
