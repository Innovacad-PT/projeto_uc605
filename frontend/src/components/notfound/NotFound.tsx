import { Center, Container, Title, Text, Image, Button, Stack, Transition } from "@mantine/core";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";

const NotFoundPage = () => {
  const navigate = useNavigate();
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    setVisible(true);
  }, []);

  return (
    <Center
      style={{
        minHeight: "100vh",
        padding: 20,
      }}
      role="alert"
    >
      <Transition mounted={visible} transition="fade" duration={400} timingFunction="ease">
        {(styles) => (
          <Container style={styles}>
            <Stack align="center">
              <Title order={1} style={{ fontSize: "calc(4rem + 2vw)", color: "#ff4c4c" }}>
                404
              </Title>
              <Title order={2}>A página não foi encontrada</Title>
              <Text size="lg" color="dimmed">
                A página que está à procura não existe.
              </Text>
  
              <Button
                size="md"
                onClick={() => navigate("/")}
                title="Return to home page"
                variant="filled"
                color="indigo"
              >
                Ir para o Inicio
              </Button>
            </Stack>
          </Container>
        )}
      </Transition>
    </Center>
  );
};

export default NotFoundPage;
