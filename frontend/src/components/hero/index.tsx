import { Grid, Stack, Title, Text } from "@mantine/core";

export default function Hero({
  onScrollToProducts,
}: {
  onScrollToProducts: () => void;
}) {
  return (
    <Grid mt="xl" align="center">
      <Grid.Col span={{ base: 12, md: 6 }}>
        <Stack gap="sm">
          <Title order={1} size={40} fw={800}>
            Tecnologia ao melhor preço
          </Title>

          <Text c="dimmed" size="lg">
            Descobre ofertas em informática, gaming, smartphones e muito mais.
          </Text>
        </Stack>
      </Grid.Col>
    </Grid>
  );
}
