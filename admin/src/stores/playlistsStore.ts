import { createStore } from "luffie";

export interface Midia {
  id: number;
  nome: string;
  exibirNoPlayer: boolean;
}

export interface Playlist {
  id: number;
  nome: string;
  midias: Midia[];
}

export interface PlaylistsState {
  playlists: Playlist[];
  loading: boolean;
  editingPlaylist: Playlist | null;
  selectedMidias: Record<number, number | null>;
}

const initialState: PlaylistsState = {
  playlists: [],
  loading: false,
  editingPlaylist: null,
  selectedMidias: {},
};

export const { state$, updateState, getCurrentState } =
  createStore<PlaylistsState>(initialState);

export const playlistActions = {
  setPlaylists: (playlists: Playlist[]) =>
    updateState((prev: PlaylistsState) => ({ ...prev, playlists })),

  addPlaylist: (playlist: Playlist) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: [...prev.playlists, playlist],
    })),

  updatePlaylist: (id: number, nome: string) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: prev.playlists.map((p) => (p.id === id ? { ...p, nome } : p)),
    })),

  removePlaylist: (id: number) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: prev.playlists.filter((p) => p.id !== id),
    })),
};

export const midiaActions = {
  addMidia: (playlistId: number, midia: Midia) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: prev.playlists.map((p) =>
        p.id === playlistId
          ? { ...p, midias: [...p.midias, { ...midia, exibirNoPlayer: true }] }
          : p
      ),
    })),

  removeMidia: (playlistId: number, midiaId: number) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: prev.playlists.map((p) =>
        p.id === playlistId
          ? { ...p, midias: p.midias.filter((m) => m.id !== midiaId) }
          : p
      ),
    })),

  toggleExibir: (playlistId: number, midiaId: number, exibir: boolean) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      playlists: prev.playlists.map((p) =>
        p.id === playlistId
          ? {
              ...p,
              midias: p.midias.map((m) =>
                m.id === midiaId ? { ...m, exibirNoPlayer: exibir } : m
              ),
            }
          : p
      ),
    })),
};

export const uiActions = {
  setLoading: (loading: boolean) =>
    updateState((prev: PlaylistsState) => ({ ...prev, loading })),
  setEditingPlaylist: (playlist: Playlist | null) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      editingPlaylist: playlist,
    })),
  setSelectedMidia: (playlistId: number, midiaId: number | null) =>
    updateState((prev: PlaylistsState) => ({
      ...prev,
      selectedMidias: { ...prev.selectedMidias, [playlistId]: midiaId },
    })),
};
