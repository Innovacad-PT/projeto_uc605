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
import { LoginUser } from "@services/auth/index";
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

    const type: string = user
      .toLowerCase()
      .match(
        /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|.(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
      )
      ? "email"
      : "username";

    const val = await LoginUser(user, password, type);

    console.log(val);

    if (val == null) {
      Notify(
        "Erro ao iniciar sessão!",
        "Verifique as credenciais e tente novamente.",
        "error"
      );
      return;
    }

    Notify("Sucesso!", "Inicio de sessão efetuado com sucesso.", "success");
    setCookie("token", val);
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
