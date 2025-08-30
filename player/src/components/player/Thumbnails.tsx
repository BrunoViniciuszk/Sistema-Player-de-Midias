import React from "react";
import { Row, Col } from "antd";
import { Midia } from "../../stores/playerStore";

interface Props {
  exibiveis: Midia[];
  currentMidiaIndex: number;
  getMidiaUrl: (path: string) => string;
  onSelect: (index: number) => void;
}

const Thumbnails: React.FC<Props> = ({
  exibiveis,
  currentMidiaIndex,
  getMidiaUrl,
  onSelect,
}) => (
  <Row gutter={[8, 8]} style={{ marginTop: 16 }}>
    {exibiveis.map((m, index) => (
      <Col key={m.id} xs={6} sm={4} md={3} lg={2}>
        {m.urlMidia.endsWith(".mp4") ? (
          <video
            src={getMidiaUrl(m.urlMidia)}
            style={{
              width: "100%",
              cursor: "pointer",
              border:
                index === currentMidiaIndex ? "2px solid #1890ff" : "none",
            }}
            onClick={() => onSelect(index)}
          />
        ) : (
          <img
            src={getMidiaUrl(m.urlMidia)}
            alt={m.nome}
            style={{
              width: "100%",
              cursor: "pointer",
              border:
                index === currentMidiaIndex ? "2px solid #1890ff" : "none",
            }}
            onClick={() => onSelect(index)}
          />
        )}
      </Col>
    ))}
  </Row>
);

export default Thumbnails;
