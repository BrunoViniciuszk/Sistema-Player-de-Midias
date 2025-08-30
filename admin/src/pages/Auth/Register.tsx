import React from "react";
import { Card, Form, Input, Button, Typography } from "antd";
import { useAuth } from "../../hooks/useAuth";
import { Link } from "react-router-dom";

const { Title, Text } = Typography;

const Register: React.FC = () => {
  const { register, loading } = useAuth();
  const [form] = Form.useForm();

  const onFinish = async (values: { username: string; password: string }) => {
    await register(values.username, values.password);
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
          Criar Conta
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
              Cadastrar
            </Button>
          </Form.Item>
        </Form>

        <div style={{ textAlign: "center", marginTop: 12 }}>
          <Text type="secondary">
            Já tem conta? <Link to="/login">Faça login</Link>
          </Text>
        </div>
      </Card>
    </div>
  );
};

export default Register;
