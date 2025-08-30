import React from "react";
import { Button } from "antd";

interface Props {
  hasMidias: boolean;
  onPrev: () => void;
  onNext: () => void;
}

const PlayerControls: React.FC<Props> = ({ hasMidias, onPrev, onNext }) =>
  hasMidias ? (
    <div style={{ display: "flex", justifyContent: "center", gap: 16 }}>
      <Button type="primary" onClick={onPrev}>
        Anterior
      </Button>
      <Button type="primary" onClick={onNext}>
        Próximo
      </Button>
    </div>
  ) : null;

export default PlayerControls;
