import { useState } from "react";
import { Layout, Menu, Grid, Drawer, Button } from "antd";
import {
  MenuOutlined,
  VideoCameraOutlined,
  OrderedListOutlined,
  LogoutOutlined,
} from "@ant-design/icons";
import Midias from "../../pages/admin/Midias";
import Playlists from "../../pages/admin/Playlists";
import { useAuth } from "../../hooks/useAuth";


const { Header, Sider, Content } = Layout;

const { useBreakpoint } = Grid;

const AdminLayout = () => {

  const screens = useBreakpoint();


  const [drawerVisible, setDrawerVisible] = useState(false);


  const [selectedMenu, setSelectedMenu] = useState("midias");


  const { logout, userName } = useAuth();


  const menuItems = [
    { key: "midias", icon: <VideoCameraOutlined />, label: "Mídias" },
    { key: "playlists", icon: <OrderedListOutlined />, label: "Playlists" },
    { key: "logout", icon: <LogoutOutlined />, label: "Sair" }, 
  ];


  const renderContent = () => {
    switch (selectedMenu) {
      case "midias":
        return <Midias />;
      case "playlists":
        return <Playlists />;
      default:
        return null;
    }
  };

  
  const handleMenuClick = (key: string) => {
    if (key === "logout") {
      logout(); 
      return;
    }
    setSelectedMenu(key); 
  };

  return (
    <Layout style={{ minHeight: "100vh" }}>
      {/* --- Layout em telas médias ou grandes (desktop) --- */}
      {screens.md ? (
        <Sider collapsible width={200} style={{ background: "#fff" }}>
          {/* Mostra nome do usuário ou "Admin" */}
          <div style={{ padding: "16px", fontWeight: "bold" }}>
            {userName ? `Olá, ${userName}` : "Admin"}
          </div>

          {/* Menu lateral */}
          <Menu
            mode="inline"
            selectedKeys={[selectedMenu]}
            onClick={(e) => handleMenuClick(e.key)}
            items={menuItems}
          />
        </Sider>
      ) : (
        <>
          {/* --- Layout em telas pequenas (mobile) --- */}
          {/* Drawer lateral que aparece quando clica no menu */}
          <Drawer
            title={userName ? `Olá, ${userName}` : "Menu"}
            placement="left"
            onClose={() => setDrawerVisible(false)}
            open={drawerVisible}
            bodyStyle={{ padding: 0 }}
          >
            <Menu
              mode="inline"
              selectedKeys={[selectedMenu]}
              onClick={(e) => {
                handleMenuClick(e.key);
                setDrawerVisible(false);
              }}
              items={menuItems}
            />
          </Drawer>

          {/* Header com botão do menu hamburguer */}
          <Header style={{ background: "#fff", padding: "0 16px" }}>
            <Button
              icon={<MenuOutlined />}
              onClick={() => setDrawerVisible(true)}
            />
          </Header>
        </>
      )}

      {/* Área principal do conteúdo */}
      <Layout>
        {!screens.md && <Header style={{ height: 0, background: "#fff" }} />}
        <Content style={{ margin: "16px" }}>{renderContent()}</Content>
      </Layout>
    </Layout>
  );
};

export default AdminLayout;
