import React from "react";
import { Row, Col, Spin, Empty, Form } from "antd";
import { usePlaylistsManager } from "../../hooks/usePlaylistsManager";
import PlaylistForm from "../../components/playlists/PlaylistForm";
import PlaylistCard from "../../components/playlists/PlaylistCard";
import { uiActions } from "../../stores/playlistsStore";

const Playlists: React.FC = () => {
  const [form] = Form.useForm();
  const {
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
  } = usePlaylistsManager(form);

  return (
    <div style={{ padding: 24 }}>
      <h1 style={{ marginBottom: 20 }}>Gerenciar Playlists</h1>

      <PlaylistForm
        form={form}
        editingPlaylist={editingPlaylist}
        onSubmit={handleSubmit}
      />

      {loading ? (
        <Spin
          tip="Carregando playlists..."
          size="large"
          style={{ display: "block", margin: "100px auto" }}
        />
      ) : playlists.length === 0 ? (
        <Empty description="Nenhuma playlist disponível" />
      ) : (
        <Row gutter={[16, 16]}>
          {playlists.map((pl) => (
            <Col key={pl.id} xs={24} sm={12} lg={8}>
              <PlaylistCard
                playlist={pl}
                midias={midias}
                selectedMidiaId={selectedMidias[pl.id]}
                onEdit={() => {
                  uiActions.setEditingPlaylist(pl);
                  form.setFieldsValue({ nome: pl.nome });
                }}
                onDelete={() => handleDeletePlaylist(pl.id)}
                onAddMidia={() => handleAddMidia(pl.id)}
                onRemoveMidia={(midiaId) => handleRemoveMidia(pl.id, midiaId)}
                onToggleExibir={(midiaId, checked) =>
                  handleToggleExibir(pl.id, midiaId, checked)
                }
                onSelectMidia={(value) =>
                  uiActions.setSelectedMidia(pl.id, value)
                }
              />
            </Col>
          ))}
        </Row>
      )}
    </div>
  );
};

export default Playlists;
