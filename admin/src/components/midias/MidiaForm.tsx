import React, { useEffect, useState } from "react";
import { Form, Input, Button, Upload } from "antd";
import { UploadOutlined } from "@ant-design/icons";
import { RcFile, UploadFile } from "antd/es/upload/interface";
import { Midia } from "../../stores/midiasStore";
import { getMidiaUrl } from "../midias/MidiaCard";

const SUPPORTED_VIDEO_TYPES = ["video/mp4", "video/webm", "video/ogg"];
const SUPPORTED_IMAGE_TYPES = ["image/png", "image/jpeg", "image/jpg"];

interface Props {
  editingMidia?: Midia | null;
  onCreate: (data: FormData) => Promise<any>;
  onUpdate: (id: number, data: FormData) => Promise<any>;
  onCancelEdit?: () => void;
}

const isVideoFile = (fileOrPath: File | string | null | UploadFile) => {
  if (!fileOrPath) return false;
  if (typeof fileOrPath === "string") {
    return [".mp4", ".webm", ".ogg"].some((ext) =>
      fileOrPath.toLowerCase().endsWith(ext)
    );
  }
  const maybe = fileOrPath as File;
  return SUPPORTED_VIDEO_TYPES.includes(maybe.type);
};

const MidiaForm: React.FC<Props> = ({
  editingMidia,
  onCreate,
  onUpdate,
  onCancelEdit,
}) => {
  const [form] = Form.useForm();
  const [file, setFile] = useState<File | null>(null);
  const [localPreview, setLocalPreview] = useState<string | null>(null);

  useEffect(() => {
    if (editingMidia) {
      form.setFieldsValue({
        nome: editingMidia.nome,
        descricao: editingMidia.descricao,
      });
      setLocalPreview(
        editingMidia.urlMidia ? getMidiaUrl(editingMidia.urlMidia) : null
      );
      setFile(null);
    } else {
      form.resetFields();
      setFile(null);
      setLocalPreview(null);
    }
  }, [editingMidia]);

  useEffect(() => {
    if (file instanceof File) {
      const url = URL.createObjectURL(file);
      setLocalPreview(url);
      return () => URL.revokeObjectURL(url);
    }
  }, [file]);

  const beforeUpload = (f: RcFile) => {
    const isImage = SUPPORTED_IMAGE_TYPES.includes(f.type);
    const isVideo = SUPPORTED_VIDEO_TYPES.includes(f.type);
    if (!isImage && !isVideo) {
      return Upload.LIST_IGNORE;
    }
    setFile(f as File);
    return false;
  };

  const handleFinish = async (values: any) => {
    const data = new FormData();
    data.append("nome", values.nome);
    data.append("descricao", values.descricao);
    if (file) data.append("file", file);

    try {
      if (editingMidia) {
        await onUpdate(editingMidia.id, data);
        onCancelEdit?.();
      } else {
        await onCreate(data);
        form.resetFields();
        setFile(null);
        setLocalPreview(null);
      }
    } catch (err) {}
  };

  return (
    <Form form={form} layout="vertical" onFinish={handleFinish}>
      <Form.Item
        name="nome"
        label="Nome"
        rules={[{ required: true, message: "Informe o nome" }]}
      >
        <Input placeholder="Nome da mídia" />
      </Form.Item>

      <Form.Item
        name="descricao"
        label="Descrição"
        rules={[{ required: true, message: "Informe a descrição" }]}
      >
        <Input placeholder="Descrição" />
      </Form.Item>

      <Form.Item label="Arquivo">
        <Upload
          beforeUpload={beforeUpload}
          showUploadList={false}
          accept=".png,.jpg,.jpeg,.mp4,.webm,.ogg"
        >
          <Button icon={<UploadOutlined />}>Selecionar arquivo</Button>
        </Upload>
      </Form.Item>

      {localPreview && (
        <div style={{ marginBottom: 16, maxHeight: 300, overflow: "hidden" }}>
          {isVideoFile(file || localPreview) ? (
            <video
              controls
              style={{
                width: "100%",
                maxHeight: 300,
                objectFit: "contain",
                borderRadius: 8,
              }}
            >
              <source src={localPreview} />
              Seu navegador não suporta vídeo.
            </video>
          ) : (
            <img
              src={localPreview}
              alt="Preview"
              style={{
                width: "100%",
                maxHeight: 300,
                objectFit: "contain",
                borderRadius: 8,
              }}
            />
          )}
        </div>
      )}

      <Form.Item>
        <Button type="primary" htmlType="submit">
          {editingMidia ? "Salvar Alterações" : "Criar Mídia"}
        </Button>
        {editingMidia && (
          <Button style={{ marginLeft: 8 }} onClick={() => onCancelEdit?.()}>
            Cancelar
          </Button>
        )}
      </Form.Item>
    </Form>
  );
};

export default MidiaForm;
