import { Routes, Route, Navigate } from "react-router-dom";
import Player from "../pages/player/PlayerScreen";

export default function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/player" replace />} />

      <Route path="/player" element={<Player />} />

      <Route path="*" element={<Navigate to="/player" replace />} />
    </Routes>
  );
}
