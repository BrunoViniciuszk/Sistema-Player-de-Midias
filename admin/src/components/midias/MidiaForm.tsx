import React, { useEffect, useState } from "react";

import { Form, Input, Button, Upload } from "antd";
import { UploadOutlined } from "@ant-design/icons";

// Tipos usados pelo componente de Upload do Ant Design
import { RcFile, UploadFile } from "antd/es/upload/interface";

import { Midia } from "../../stores/midiasStore";

import { getMidiaUrl } from "../midias/MidiaCard";

// Tipos suportados
const SUPPORTED_VIDEO_TYPES = ["video/mp4", "video/webm", "video/ogg"];
const SUPPORTED_IMAGE_TYPES = ["image/png", "image/jpeg", "image/jpg"];

// Props esperadas pelo componente
interface Props {
  editingMidia?: Midia | null; // mídia em edição (opcional)
  onCreate: (data: FormData) => Promise<any>; // callback ao criar
  onUpdate: (id: number, data: FormData) => Promise<any>; // callback ao editar
  onCancelEdit?: () => void; // callback ao cancelar edição (opcional)
}

// Função auxiliar que detecta se um arquivo ou caminho é de vídeo
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

// Componente principal do formulário de mídia
const MidiaForm: React.FC<Props> = ({
  editingMidia,
  onCreate,
  onUpdate,
  onCancelEdit,
}) => {
  // Hook do Ant Design para manipular o form
  const [form] = Form.useForm();

  // Estado para o arquivo selecionado
  const [file, setFile] = useState<File | null>(null);

  // Estado para o preview local (imagem/vídeo)
  const [localPreview, setLocalPreview] = useState<string | null>(null);

  // Atualiza o form quando `editingMidia` mudar
  useEffect(() => {
    if (editingMidia) {
      // Preenche o form com os dados existentes
      form.setFieldsValue({
        nome: editingMidia.nome,
        descricao: editingMidia.descricao,
      });
      // Se já existe uma mídia salva, monta a URL dela
      setLocalPreview(
        editingMidia.urlMidia ? getMidiaUrl(editingMidia.urlMidia) : null
      );
      setFile(null);
    } else {
      // Reset se não estiver editando
      form.resetFields();
      setFile(null);
      setLocalPreview(null);
    }
  }, [editingMidia]);

  // Gera um preview local quando o usuário seleciona um novo arquivo
  useEffect(() => {
    if (file instanceof File) {
      const url = URL.createObjectURL(file);
      setLocalPreview(url);

      // Limpa o objeto URL quando o componente desmonta ou arquivo troca
      return () => URL.revokeObjectURL(url);
    }
  }, [file]);

  // Função chamada antes do upload
  const beforeUpload = (f: RcFile) => {
    const isImage = SUPPORTED_IMAGE_TYPES.includes(f.type);
    const isVideo = SUPPORTED_VIDEO_TYPES.includes(f.type);

    // Se não for imagem nem vídeo suportado → ignora
    if (!isImage && !isVideo) {
      return Upload.LIST_IGNORE;
    }

    // Salva no estado mas não faz upload automático
    setFile(f as File);
    return false;
  };

  // Submissão do formulário
  const handleFinish = async (values: any) => {
    const data = new FormData();
    data.append("nome", values.nome);
    data.append("descricao", values.descricao);
    if (file) data.append("file", file);

    try {
      if (editingMidia) {
        // Atualiza mídia existente
        await onUpdate(editingMidia.id, data);
        onCancelEdit?.();
      } else {
        // Cria nova mídia
        await onCreate(data);
        form.resetFields();
        setFile(null);
        setLocalPreview(null);
      }
    } catch (err) {
      // Erro silenciado, mas poderia ter tratamento (ex: notificação)
    }
  };

  return (
    <Form form={form} layout="vertical" onFinish={handleFinish}>
      {/* Campo Nome */}
      <Form.Item
        name="nome"
        label="Nome"
        rules={[{ required: true, message: "Informe o nome" }]}
      >
        <Input placeholder="Nome da mídia" />
      </Form.Item>

      {/* Campo Descrição */}
      <Form.Item
        name="descricao"
        label="Descrição"
        rules={[{ required: true, message: "Informe a descrição" }]}
      >
        <Input placeholder="Descrição" />
      </Form.Item>

      {/* Upload do arquivo */}
      <Form.Item label="Arquivo">
        <Upload
          beforeUpload={beforeUpload}
          showUploadList={false} // esconde lista padrão do Ant Design
          accept=".png,.jpg,.jpeg,.mp4,.webm,.ogg" // restringe os tipos
        >
          <Button icon={<UploadOutlined />}>Selecionar arquivo</Button>
        </Upload>
      </Form.Item>

      {/* Preview da mídia (imagem ou vídeo) */}
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

      {/* Botões de ação */}
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
