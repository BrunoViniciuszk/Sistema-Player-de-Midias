import React from "react";
import { Card, Form, Input, Button, Typography } from "antd";
import { useAuth } from "../../hooks/useAuth";
import { Link } from "react-router-dom";

const { Title, Text } = Typography;

const Login: React.FC = () => {
  const { login, loading } = useAuth();
  const [form] = Form.useForm();

  const onFinish = async (values: { username: string; password: string }) => {
    await login(values.username, values.password);
  };

  return (
    <div
      style={{
        minHeight: "100vh",
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        padding: 16,
        background: "#0b0b0b",
      }}
    >
      <Card style={{ width: 360 }}>
        <Title level={3} style={{ textAlign: "center", marginBottom: 24 }}>
          Acessar
        </Title>
        <Form form={form} layout="vertical" onFinish={onFinish}>
          <Form.Item
            label="Usuário"
            name="username"
            rules={[{ required: true, message: "Informe o usuário" }]}
          >
            <Input placeholder="seu usuário" />
          </Form.Item>

          <Form.Item
            label="Senha"
            name="password"
            rules={[{ required: true, message: "Informe a senha" }]}
          >
            <Input.Password placeholder="sua senha" />
          </Form.Item>

          <Form.Item>
            <Button type="primary" htmlType="submit" block loading={loading}>
              Entrar
            </Button>
          </Form.Item>
        </Form>

        <div style={{ textAlign: "center", marginTop: 12 }}>
          <Text type="secondary">
            Não tem conta? <Link to="/register">Cadastre-se</Link>
          </Text>
        </div>
      </Card>
    </div>
  );
};

export default Login;
