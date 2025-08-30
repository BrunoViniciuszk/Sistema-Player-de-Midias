import React from "react";
import { Empty } from "antd";
import { Midia } from "../../stores/playerStore";

interface Props {
  midiaAtual?: Midia;
  tipoAtual: "video" | "imagem";
  fade: boolean;
  exibiveis: Midia[];
  getMidiaUrl: (path: string) => string;
}

const MidiaViewer: React.FC<Props> = ({
  midiaAtual,
  tipoAtual,
  fade,
  exibiveis,
  getMidiaUrl,
}) => (
  <div
    style={{
      display: "flex",
      alignItems: "center",
      justifyContent: "center",
      height: 400,
      backgroundColor: "#111",
      borderRadius: 8,
      overflow: "hidden",
      marginBottom: 16,
      opacity: fade ? 0 : 1,
      transition: "opacity 0.3s ease-in-out",
    }}
  >
    {exibiveis.length === 0 ? (
      <Empty
        description={
          <span style={{ color: "#fff" }}>
            Nenhuma mídia marcada para exibir
          </span>
        }
      />
    ) : tipoAtual === "imagem" ? (
      <img
        src={getMidiaUrl(midiaAtual!.urlMidia)}
        alt={midiaAtual!.nome}
        style={{ maxWidth: "100%", maxHeight: "100%", objectFit: "contain" }}
      />
    ) : (
      <video
        src={getMidiaUrl(midiaAtual!.urlMidia)}
        controls
        autoPlay
        style={{ maxWidth: "100%", maxHeight: "100%" }}
      />
    )}
  </div>
);

export default MidiaViewer;
