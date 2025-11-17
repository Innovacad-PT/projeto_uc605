import { Container, Title, Text, Image } from "@mantine/core";

const NotFoundPage = () => {
  return (
    <Container style={{ textAlign: "center", paddingTop: 50 }}>
      <Title order={1} style={{ fontSize: 80, color: "#ff4c4c" }}>
        404
      </Title>
      <Title order={2}>Page Not Found</Title>
      <Text size="lg" color="dimmed" mt="sm">
        The page you are looking for does not exist.
      </Text>

      <div style={{ maxWidth: 400, margin: "40px auto" }}>
        <Image
          src="https://cdn-icons-png.flaticon.com/512/564/564619.png"
          alt="Page Not Found"
          style={{ width: "100%", height: "auto" }}
        />
      </div>
    </Container>
  );
};

export default NotFoundPage;
