import { Routes, Route, Navigate } from "react-router-dom";
import AdminLayout from "../components/layout/AdminLayout";
import Midias from "../pages/admin/Midias";
import Playlists from "../pages/admin/Playlists";
import Login from "../pages/Auth/Login";
import PrivateRoute from "./PrivateRoute";
import Register from "../pages/Auth/Register";

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/admin" replace />} />

      <Route path="/login" element={<Login />} />

      <Route path="/register" element={<Register />} />

      <Route
        path="/admin"
        element={
          <PrivateRoute>
            <AdminLayout />
          </PrivateRoute>
        }
      >
        <Route path="midias" element={<Midias />} />
        <Route path="playlists" element={<Playlists />} />
      </Route>
    </Routes>
  );
}
