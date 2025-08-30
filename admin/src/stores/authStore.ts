import { createStore } from "luffie";

export interface AuthState {
  token: string | null;
  userName: string | null;
  loading: boolean;
  isAuthenticated: boolean;
  initialized: boolean;
}

const initialState: AuthState = {
  token: localStorage.getItem("token"),
  userName: localStorage.getItem("userName"),
  loading: false,
  isAuthenticated: !!localStorage.getItem("token"),
  initialized: true,
};

export const { state$, updateState, getCurrentState } =
  createStore<AuthState>(initialState);

export const authActions = {
  setLoading: (loading: boolean) =>
    updateState((prev: AuthState) => ({ ...prev, loading })),

  loginSuccess: (token: string, userName?: string | null) => {
    localStorage.setItem("token", token);
    if (userName) localStorage.setItem("userName", userName);
    updateState((prev: AuthState) => ({
      ...prev,
      token,
      userName: userName ?? prev.userName ?? null,
      isAuthenticated: true,
      loading: false,
      initialized: true,
    }));
  },

  logout: () => {
    localStorage.removeItem("token");
    localStorage.removeItem("userName");
    updateState(() => ({
      token: null,
      userName: null,
      isAuthenticated: false,
      loading: false,
      initialized: true,
    }));
  },
};
