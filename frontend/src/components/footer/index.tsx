import { Container, Grid, Text, Group, Box } from "@mantine/core";
import {
  IconBrandFacebook,
  IconBrandInstagram,
  IconBrandTwitter,
} from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";

export default function AppFooter() {
  const nav = useNavigate();
  return (
    <Box
      component="footer"
      style={{
        marginTop: "60px",
        paddingTop: "40px",
        paddingBottom: "40px",
        backgroundColor: "#f3f3f3",
        borderTop: "1px solid #ddd",
      }}
    >
      <Container size="lg">
        <Grid>
          {/* COLUNA 1 */}
          <Grid.Col span={{ base: 12, md: 4 }}>
            <Text fw={700} size="lg">
              CAPITEK
            </Text>
            <Text c="dimmed" mt="xs">
              Tecnologia ao melhor preço. Hardware, gaming, gadgets e muito
              mais.
            </Text>
          </Grid.Col>

          {/* COLUNA 2 */}
          <Grid.Col span={{ base: 12, md: 4 }}>
            <Text fw={700} size="lg">
              Links úteis
            </Text>
            <Text mt="xs" style={{ cursor: "pointer" }} onClick={() => nav("/")}>
              Início
            </Text>
            <Text style={{ cursor: "pointer" }} onClick={() => nav("/products")}>Produtos</Text>
            <Text style={{ cursor: "pointer" }}>Sobre Nós</Text>
            <Text style={{ cursor: "pointer" }} onClick={() => nav("/contacts")}>Contactos</Text>
          </Grid.Col>

          {/* COLUNA 3 */}
          <Grid.Col span={{ base: 12, md: 4 }}>
            <Text fw={700} size="lg">
              Segue-nos
            </Text>

            <Group mt="xs">
              <IconBrandFacebook size={22} style={{ cursor: "pointer" }} />
              <IconBrandInstagram size={22} style={{ cursor: "pointer" }} />
              <IconBrandTwitter size={22} style={{ cursor: "pointer" }} />
            </Group>
          </Grid.Col>
        </Grid>

        <Text ta="center" c="dimmed" mt={40} size="sm">
          © {new Date().getFullYear()} CAPITEK. Todos os direitos reservados.
        </Text>
      </Container>
    </Box>
  );
}
