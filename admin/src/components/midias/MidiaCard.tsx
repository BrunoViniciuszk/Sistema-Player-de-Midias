import React from "react";
import { Card, Button, Popconfirm } from "antd";
import { Midia } from "../../stores/midiasStore";

interface Props {
  midia: Midia;
  onEdit: (m: Midia) => void;
  onDelete: (id: number) => void;
  apiUrl?: string;
}

const isVideo = (path: string) => {
  return [".mp4", ".webm", ".ogg"].some((ext) =>
    path.toLowerCase().endsWith(ext)
  );
};

// Função utilitária para montar a URL completa de uma mídia
export const getMidiaUrl = (path: string) => {
  // Se o parâmetro "path" vier vazio ou nulo, retorna string vazia
  if (!path) return "";

  // Pega a URL base da API definida no .env (ex: http://localhost:3000/api)
  // e remove o "/api" do final, para formar a raiz (ex: http://localhost:3000)
  const base = process.env.REACT_APP_API_URL?.replace("/api", "");

  // Monta a URL final:
  // - Se "path" já começa com "/", só concatena (base + path)
  // - Se não começa com "/", adiciona "/" antes para evitar erro
  return `${base}${path.startsWith("/") ? path : "/" + path}`;
};

// Declaração de um componente funcional React chamado "MidiaCard"
// Ele recebe props tipadas pela interface "Props"
const MidiaCard: React.FC<Props> = ({ midia, onEdit, onDelete }) => {
  return (
    <Card
      title={midia.nome}
      extra={
        <>
          <Button type="link" onClick={() => onEdit(midia)}>
            Editar
          </Button>
          <Popconfirm
            title="Deseja deletar?"
            onConfirm={() => onDelete(midia.id)}
            okText="Sim"
            cancelText="Não"
          >
            <Button type="link" danger>
              Deletar
            </Button>
          </Popconfirm>
        </>
      }
    >
      <p>{midia.descricao}</p>
      {isVideo(midia.urlMidia) ? (
        <video
          src={getMidiaUrl(midia.urlMidia)}
          controls
          style={{
            width: "100%",
            maxHeight: 200,
            objectFit: "contain",
            borderRadius: 8,
          }}
        />
      ) : (
        <img
          src={getMidiaUrl(midia.urlMidia)}
          alt={midia.nome}
          style={{
            width: "100%",
            maxHeight: 200,
            objectFit: "contain",
            borderRadius: 8,
          }}
        />
      )}
    </Card>
  );
};

export default MidiaCard;
