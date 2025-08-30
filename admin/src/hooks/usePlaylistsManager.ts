import { useEffect, useState } from "react";
import { message } from "antd";
import { useStream } from "luffie";
import {
  getPlaylists,
  createPlaylist,
  updatePlaylist,
  deletePlaylist,
  addMidiaToPlaylist,
  removeMidiaFromPlaylist,
  updateExibirNoPlayer,
} from "../api/playlists";
import { getMidias } from "../api/midias";
import {
  state$,
  playlistActions,
  midiaActions,
  uiActions,
  Playlist,
  Midia,
} from "../stores/playlistsStore";

export const usePlaylistsManager = (form: any) => {
  const stateData = useStream(state$) ?? {
    playlists: [],
    editingPlaylist: null,
    selectedMidias: {},
    loading: false,
  };

  const { playlists, editingPlaylist, selectedMidias, loading } = stateData;
  const [midias, setMidias] = useState<Midia[]>([]);

  useEffect(() => {
    const fetchData = async () => {
      uiActions.setLoading(true);
      try {
        const [pl, md] = await Promise.all([getPlaylists(), getMidias()]);
        playlistActions.setPlaylists(pl.data);
        setMidias(md.data);
      } catch {
        message.error("Erro ao carregar dados");
      } finally {
        uiActions.setLoading(false);
      }
    };
    fetchData();
  }, []);

  const handleSubmit = async ({ nome }: { nome: string }) => {
    try {
      if (editingPlaylist) {
        await updatePlaylist(editingPlaylist.id, { nome });
        playlistActions.updatePlaylist(editingPlaylist.id, nome);
        uiActions.setEditingPlaylist(null);
        message.success("Playlist atualizada!");
      } else {
        const response = await createPlaylist({ nome });
        playlistActions.addPlaylist(response.data);
        message.success("Playlist criada!");
      }
      form.resetFields();
    } catch {
      message.error("Erro ao salvar playlist");
    }
  };

  const handleAddMidia = async (playlistId: number) => {
    const midiaId = selectedMidias[playlistId];
    if (!midiaId) return message.error("Selecione uma mídia");

    const midia = midias.find((m) => m.id === midiaId);
    if (!midia) return;

    try {
      await addMidiaToPlaylist(playlistId, midiaId);
      midiaActions.addMidia(playlistId, midia);
      uiActions.setSelectedMidia(playlistId, null);
      message.success("Mídia adicionada");
    } catch {
      message.error("Erro ao adicionar mídia");
    }
  };

  const handleRemoveMidia = async (playlistId: number, midiaId: number) => {
    try {
      await removeMidiaFromPlaylist(playlistId, midiaId);
      midiaActions.removeMidia(playlistId, midiaId);
      message.success("Mídia removida");
    } catch {
      message.error("Erro ao remover mídia");
    }
  };

  const handleDeletePlaylist = async (playlistId: number) => {
    try {
      await deletePlaylist(playlistId);
      playlistActions.removePlaylist(playlistId);
      message.success("Playlist deletada");
    } catch {
      message.error("Erro ao deletar playlist");
    }
  };

  const handleToggleExibir = async (
    playlistId: number,
    midiaId: number,
    checked: boolean
  ) => {
    try {
      await updateExibirNoPlayer(playlistId, midiaId, checked);
      midiaActions.toggleExibir(playlistId, midiaId, checked);
    } catch {
      message.error("Erro ao atualizar visibilidade da mídia");
    }
  };

  return {
    playlists,
    editingPlaylist,
    selectedMidias,
    loading,
    midias,
    handleSubmit,
    handleAddMidia,
    handleRemoveMidia,
    handleDeletePlaylist,
    handleToggleExibir,
  };
};
