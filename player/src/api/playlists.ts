import axios from "axios";
import { Playlist, Midia } from "../stores/playerStore";

const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

export interface CreatePlaylistDTO {
  nome: string;
}
export interface UpdatePlaylistDTO {
  nome: string;
}

export const getPlaylists = () => api.get<Playlist[]>("/playlists");
export const getPlaylistById = (id: number) =>
  api.get<Playlist>(`/playlists/${id}`);
export const createPlaylist = (data: CreatePlaylistDTO) =>
  api.post<Playlist>("/playlists", data);
export const updatePlaylist = (id: number, payload: UpdatePlaylistDTO) =>
  api.put<Playlist>(`/playlists/${id}`, payload);
export const deletePlaylist = (id: number) =>
  api.delete<void>(`/playlists/${id}`);

export const addMidiaToPlaylist = (
  playlistId: number,
  midiaId: number,
  exibirNoPlayer = true
) =>
  api.post<Midia>(
    `/playlists/${playlistId}/midias/${midiaId}?exibirNoPlayer=${exibirNoPlayer}`
  );

export const removeMidiaFromPlaylist = (playlistId: number, midiaId: number) =>
  api.delete<void>(`/playlists/${playlistId}/midias/${midiaId}`);

export const updateExibirNoPlayer = (
  playlistId: number,
  midiaId: number,
  exibir: boolean
) =>
  api.patch<void>(
    `/playlists/${playlistId}/midias/${midiaId}?exibirNoPlayer=${exibir}`
  );
