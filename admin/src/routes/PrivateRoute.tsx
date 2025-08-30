import React from "react";
import { Navigate } from "react-router-dom";
import { useStream } from "luffie";
import { state$ } from "../stores/authStore";

interface Props {
  children: React.ReactElement;
}

const PrivateRoute: React.FC<Props> = ({ children }) => {
  const auth = useStream(state$) ?? {
    isAuthenticated: false,
    loading: false,
    initialized: false,
  };

  if (!auth.initialized) {
    return <div>Carregando...</div>;
  }

  if (!auth.isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default PrivateRoute;
