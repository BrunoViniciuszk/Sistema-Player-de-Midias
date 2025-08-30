import React from "react";
import { Spin, Empty, Card } from "antd";
import { usePlayerManager } from "../../hooks/usePlayerManager";
import PlaylistSelector from "../../components/player/PlaylistSelector";
import MidiaViewer from "../../components/player/MidiaViewer";
import PlayerControls from "../../components/player/PlayerControls";
import Thumbnails from "../../components/player/Thumbnails";

const PlayerScreen: React.FC = () => {
  const {
    playlists,
    currentPlaylist,
    currentPlaylistIndex,
    currentMidiaIndex,
    exibiveis,
    midiaAtual,
    tipoAtual,
    loading,
    fade,
    getMidiaUrl,
    playerActions,
  } = usePlayerManager();

  if (loading)
    return (
      <Spin
        tip="Carregando playlists..."
        size="large"
        style={{ display: "block", margin: "100px auto" }}
      />
    );

  if (!playlists.length)
    return <Empty description="Nenhuma playlist disponível" />;

  return (
    <div style={{ padding: 24, backgroundColor: "#000", minHeight: "100vh" }}>
      <PlaylistSelector
        playlists={playlists}
        currentPlaylistId={currentPlaylist?.id}
        onChange={(id) => {
          const newIndex = playlists.findIndex((p) => p.id === id);
          playerActions.setCurrentPlaylistIndex(newIndex);
        }}
      />

      <Card
        title={currentPlaylist?.nome}
        style={{ backgroundColor: "#1f1f1f", color: "#fff" }}
        headStyle={{ color: "#fff" }}
      >
        <MidiaViewer
          midiaAtual={midiaAtual}
          tipoAtual={tipoAtual}
          fade={fade}
          exibiveis={exibiveis}
          getMidiaUrl={getMidiaUrl}
        />

        <PlayerControls
          hasMidias={exibiveis.length > 0}
          onPrev={playerActions.prevMidia}
          onNext={playerActions.nextMidia}
        />

        <Thumbnails
          exibiveis={exibiveis}
          currentMidiaIndex={currentMidiaIndex}
          getMidiaUrl={getMidiaUrl}
          onSelect={playerActions.setCurrentMidiaIndex}
        />
      </Card>
    </div>
  );
};

export default PlayerScreen;
