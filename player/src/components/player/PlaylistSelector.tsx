import React from "react";
import { Select } from "antd";
import { Playlist } from "../../stores/playerStore";

const { Option } = Select;

interface Props {
  playlists: Playlist[];
  currentPlaylistId?: number;
  onChange: (playlistId: number) => void;
}

const PlaylistSelector: React.FC<Props> = ({
  playlists,
  currentPlaylistId,
  onChange,
}) => (
  <Select
    value={currentPlaylistId}
    onChange={onChange}
    style={{ width: 300, marginBottom: 16 }}
  >
    {playlists.map((p) => (
      <Option key={p.id} value={p.id}>
        {p.nome}
      </Option>
    ))}
  </Select>
);

export default PlaylistSelector;
