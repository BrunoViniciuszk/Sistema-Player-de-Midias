import { useEffect, useMemo, useState } from "react";
import { useStream } from "luffie";
import { message } from "antd";
import { getPlaylists } from "../api/playlists";
import {
  state$,
  playerActions,
  PlayerState,
  Midia,
  Playlist,
} from "../stores/playerStore";

export const usePlayerManager = () => {
  const stateData = useStream<PlayerState>(state$);
  const { playlists, currentPlaylistIndex, currentMidiaIndex, loading } =
    stateData || {
      playlists: [],
      currentPlaylistIndex: 0,
      currentMidiaIndex: 0,
      loading: true,
    };

  const currentPlaylist: Playlist | undefined = playlists[currentPlaylistIndex];

  const exibiveis: Midia[] = useMemo(
    () => currentPlaylist?.midias?.filter((m) => m.exibirNoPlayer) || [],
    [currentPlaylist]
  );

  const [fade, setFade] = useState(false);
  useEffect(() => {
    setFade(true);
    const timer = setTimeout(() => setFade(false), 300);
    return () => clearTimeout(timer);
  }, [currentMidiaIndex]);

  useEffect(() => {
    if (currentMidiaIndex >= exibiveis.length) {
      playerActions.setCurrentMidiaIndex(0);
    }
  }, [exibiveis.length, currentMidiaIndex]);

  useEffect(() => {
    const fetchPlaylists = async () => {
      try {
        const response = await getPlaylists();
        playerActions.setPlaylists(response.data);
      } catch {
        message.error("Erro ao carregar playlists");
      } finally {
        playerActions.setLoading(false);
      }
    };

    fetchPlaylists();
    const intervalId = setInterval(fetchPlaylists, 5000);
    return () => clearInterval(intervalId);
  }, []);

  const getMidiaUrl = (path: string) => {
    if (!path) return "";
    const base = process.env.REACT_APP_API_URL?.replace("/api", "");
    return `${base}${path.startsWith("/") ? path : "/" + path}`;
  };

  const midiaAtual = exibiveis[currentMidiaIndex];
  const tipoAtual: "video" | "imagem" = midiaAtual?.urlMidia?.endsWith(".mp4")
    ? "video"
    : "imagem";

  return {
    playlists,
    currentPlaylist,
    currentPlaylistIndex,
    currentMidiaIndex,
    exibiveis,
    midiaAtual,
    tipoAtual,
    loading,
    fade,
    getMidiaUrl,
    playerActions,
  };
};
