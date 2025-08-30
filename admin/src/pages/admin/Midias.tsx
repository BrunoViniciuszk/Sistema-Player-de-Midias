import React from "react";
import { Row, Col, Card, Spin, Empty } from "antd";
import useMidias from "../../hooks/useMidias";
import MidiaForm from "../../../src/components/midias/MidiaForm";
import MidiaCard from "../../../src/components/midias/MidiaCard";

const MidiasPage: React.FC = () => {
  const { midias, loading, editingMidia, create, update, remove, edit } =
    useMidias();

  const handleCreate = async (data: FormData) => {
    await create(data);
  };

  const handleUpdate = async (id: number, data: FormData) => {
    await update(id, data);
  };

  const handleDelete = async (id: number) => {
    await remove(id);
  };

  const handleEdit = (m: any) => {
    edit(m);
  };

  const handleCancelEdit = () => edit(null);

  return (
    <div style={{ padding: 24 }}>
      <h1 style={{ marginBottom: 24 }}>Gerenciar Mídias</h1>

      <Card style={{ marginBottom: 24 }}>
        <MidiaForm
          editingMidia={editingMidia}
          onCreate={handleCreate}
          onUpdate={handleUpdate}
          onCancelEdit={handleCancelEdit}
        />
      </Card>

      {loading ? (
        <Spin
          tip="Carregando mídias..."
          size="large"
          style={{ display: "block", margin: "40px auto" }}
        />
      ) : midias.length === 0 ? (
        <Empty description="Nenhuma mídia disponível" />
      ) : (
        <Row gutter={[16, 16]}>
          {midias.map((m) => (
            <Col key={m.id} xs={24} sm={12} lg={8}>
              <MidiaCard
                midia={m}
                onEdit={handleEdit}
                onDelete={handleDelete}
              />
            </Col>
          ))}
        </Row>
      )}
    </div>
  );
};

export default MidiasPage;
