import { TextInput, Button, Group, Box, Title, Text, Stack } from '@mantine/core';
import { useForm } from '@mantine/form';
import { IconAt } from '@tabler/icons-react';

export default function Newsletter() {
  const form = useForm({
    initialValues: {
      email: '',
    },
    validate: {
      email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Email inválido'),
    },
  });

  const handleSubmit = (values: typeof form.values) => {
    console.log('Newsletter subscription:', values);
    form.reset();
  };

  return (
    <Box py="xl">
      <Stack align="center" gap="md">
        <Title order={2}>Subscreve a nossa Newsletter</Title>
        <Text c="dimmed" ta="center" maw={500}>
          Recebe as últimas novidades, promoções exclusivas e atualizações diretamente na tua caixa de entrada.
        </Text>

        <form onSubmit={form.onSubmit(handleSubmit)} style={{ width: '100%', maxWidth: 400 }}>
          <Group align="flex-start">
            <TextInput
              placeholder="O teu email"
              leftSection={<IconAt size={16} />}
              {...form.getInputProps('email')}
              style={{ flex: 1 }}
            />
            <Button type="submit">Subscrever</Button>
          </Group>
        </form>
      </Stack>
    </Box>
  );
}
