import { Grid, Text } from "@mantine/core";

export default function Benefits() {
  return (
    <Grid mt={60}>
      <Grid.Col span={{ base: 12, sm: 4 }}>
        <Text fw={700} size="lg">
          🚚 Entrega Rápida
        </Text>
        <Text c="dimmed">24-48h em Portugal Continental</Text>
      </Grid.Col>

      <Grid.Col span={{ base: 12, sm: 4 }}>
        <Text fw={700} size="lg">
          🛡 Garantia Oficial
        </Text>
        <Text c="dimmed">Assistência técnica autorizada</Text>
      </Grid.Col>

      <Grid.Col span={{ base: 12, sm: 4 }}>
        <Text fw={700} size="lg">
          ↩️ Devoluções Fáceis
        </Text>
        <Text c="dimmed">Até 14 dias</Text>
      </Grid.Col>
    </Grid>
  );
}
