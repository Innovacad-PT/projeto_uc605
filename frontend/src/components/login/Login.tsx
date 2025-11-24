/* eslint-disable @typescript-eslint/no-unused-vars */
import {
  TextInput,
  PasswordInput,
  Checkbox,
  Paper,
  Title,
  Button,
  Container,
} from "@mantine/core";
import { useState } from "react";
import { useCookies } from "react-cookie";
import { login } from "@services/auth";
import { Notify } from "@utils/notify";

const Login = () => {
  const [user, setUser] = useState("");
  const [password, setPassword] = useState("");
  const [_, setCookie, __] = useCookies();

  const handleLogin = async (user: string, password: string) => {
    if (user.length <= 0 || password.length <= 0) {
      Notify(
        "Erro ao iniciar sessão!",
        "Verifique as credenciais e tente novamente.",
        "error"
      );
      return;
    }

    try {
      await login(user, password);
      Notify("Sucesso!", "Inicio de sessão efetuado com sucesso.", "success");
      // Redirect or update UI as needed
      window.location.href = "/"; // or use router navigation
    } catch (error) {
      Notify(
        "Erro ao iniciar sessão!",
        "Verifique as credenciais e tente novamente.",
        "error"
      );
    }
  };

  return (
    <Container size={420} my={40}>
      <Title ta="center">Welcome Back</Title>
      <Paper withBorder shadow="md" p={30} mt={30} radius="md">
        <form
          onSubmit={(e) => {
            e.preventDefault();
            handleLogin(user, password);
          }}
        >
          <TextInput
            label="Email or Username"
            placeholder="Your email or username"
            value={user}
            onChange={(e) => setUser(e.currentTarget.value)}
          />
          <PasswordInput
            label="Password"
            placeholder="Your password"
            mt="md"
            value={password}
            onChange={(e) => setPassword(e.currentTarget.value)}
          />
          <Checkbox label="Remember me" mt="md" />
          <Button fullWidth mt="xl" type="submit">
            Login
          </Button>
        </form>
      </Paper>
    </Container>
  );
};

export default Login;
