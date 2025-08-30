import React from "react";
import { Form, Input, Button } from "antd";
import { uiActions, Playlist } from "../../stores/playlistsStore";

interface Props {
  form: any;
  editingPlaylist: Playlist | null;
  onSubmit: (values: { nome: string }) => void;
}

const PlaylistForm: React.FC<Props> = ({ form, editingPlaylist, onSubmit }) => (
  <Form
    form={form}
    layout="vertical"
    onFinish={onSubmit}
    style={{ marginBottom: 20 }}
  >
    <Form.Item
      name="nome"
      rules={[{ required: true, message: "Informe o nome da playlist" }]}
    >
      <Input placeholder="Nome da Playlist" style={{ width: 300 }} />
    </Form.Item>
    <Form.Item>
      <Button type="primary" htmlType="submit">
        {editingPlaylist ? "Salvar Alterações" : "Criar Playlist"}
      </Button>
      {editingPlaylist && (
        <Button
          style={{ marginLeft: 8 }}
          onClick={() => {
            uiActions.setEditingPlaylist(null);
            form.resetFields();
          }}
        >
          Cancelar
        </Button>
      )}
    </Form.Item>
  </Form>
);

export default PlaylistForm;
