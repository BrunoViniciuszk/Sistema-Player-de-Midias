import { createStore } from "luffie";

export interface Midia {
  id: number;
  nome: string;
  urlMidia: string;
  exibirNoPlayer: boolean;
}

export interface Playlist {
  id: number;
  nome: string;
  midias: Midia[];
}

export interface PlayerState {
  playlists: Playlist[];
  currentPlaylistIndex: number;
  currentMidiaIndex: number;
  loading: boolean;
}

const initialState: PlayerState = {
  playlists: [],
  currentPlaylistIndex: 0,
  currentMidiaIndex: 0,
  loading: true,
};

export const { state$, updateState, getCurrentState } =
  createStore<PlayerState>(initialState);

const getExibiveis = (playlist: Playlist | undefined): Midia[] =>
  playlist?.midias?.filter((m) => m.exibirNoPlayer) || [];

export const playerActions = {
  setPlaylists: (playlists: Playlist[]) =>
    updateState((prev: PlayerState) => ({
      ...prev,
      playlists,
      loading: false,
    })),

  setLoading: (loading: boolean) =>
    updateState((prev: PlayerState) => ({ ...prev, loading })),

  setCurrentPlaylistIndex: (index: number) =>
    updateState((prev: PlayerState) => ({
      ...prev,
      currentPlaylistIndex: index,
      currentMidiaIndex: 0,
    })),

  setCurrentMidiaIndex: (index: number) =>
    updateState((prev: PlayerState) => ({ ...prev, currentMidiaIndex: index })),

  prevMidia: () =>
    updateState((prev: PlayerState) => {
      const exibiveis = getExibiveis(prev.playlists[prev.currentPlaylistIndex]);
      if (!exibiveis.length) return prev;
      const newIndex =
        (prev.currentMidiaIndex - 1 + exibiveis.length) % exibiveis.length;
      return { ...prev, currentMidiaIndex: newIndex };
    }),

  nextMidia: () =>
    updateState((prev: PlayerState) => {
      const exibiveis = getExibiveis(prev.playlists[prev.currentPlaylistIndex]);
      if (!exibiveis.length) return prev;
      const newIndex = (prev.currentMidiaIndex + 1) % exibiveis.length;
      return { ...prev, currentMidiaIndex: newIndex };
    }),
};
