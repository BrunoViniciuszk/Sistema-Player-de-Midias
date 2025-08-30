import React from "react";
import { Card, List, Button, Switch, Select } from "antd";
import { Playlist, Midia, uiActions } from "../../stores/playlistsStore";

const { Option } = Select;

interface Props {
  playlist: Playlist;
  midias: Midia[];
  selectedMidiaId?: number | null;
  onEdit: () => void;
  onDelete: () => void;
  onAddMidia: () => void;
  onRemoveMidia: (midiaId: number) => void;
  onToggleExibir: (midiaId: number, checked: boolean) => void;
  onSelectMidia: (midiaId: number) => void;
}

const PlaylistCard: React.FC<Props> = ({
  playlist,
  midias,
  selectedMidiaId,
  onEdit,
  onDelete,
  onAddMidia,
  onRemoveMidia,
  onToggleExibir,
  onSelectMidia,
}) => (
  <Card
    title={playlist.nome}
    extra={
      <>
        <Button type="link" onClick={onEdit}>
          Editar
        </Button>
        <Button type="link" danger onClick={onDelete}>
          Deletar
        </Button>
      </>
    }
  >
    <List
      size="small"
      dataSource={playlist.midias}
      renderItem={(m) => (
        <List.Item
          actions={[
            <Switch
              checked={m.exibirNoPlayer}
              onChange={(c) => onToggleExibir(m.id, c)}
            />,
            <Button type="link" danger onClick={() => onRemoveMidia(m.id)}>
              Remover
            </Button>,
          ]}
        >
          {m.nome}
        </List.Item>
      )}
    />

    <div style={{ marginTop: 10 }}>
      <Select
        style={{ width: "70%", marginRight: 8 }}
        placeholder="Selecionar mídia"
        value={selectedMidiaId || undefined}
        onChange={(value) => onSelectMidia(value)}
      >
        {midias.map((m) => (
          <Option key={m.id} value={m.id}>
            {m.nome}
          </Option>
        ))}
      </Select>
      <Button type="primary" onClick={onAddMidia}>
        Adicionar
      </Button>
    </div>
  </Card>
);

export default PlaylistCard;
