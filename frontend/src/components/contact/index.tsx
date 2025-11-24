import { TextInput, Textarea, Button, Group, Title, Box, Stack } from '@mantine/core';
import { useForm } from '@mantine/form';

export default function Contact() {
  const form = useForm({
    initialValues: {
      name: '',
      email: '',
      subject: '',
      message: '',
    },
    validate: {
      name: (value) => (value.length < 2 ? 'O nome deve ter pelo menos 2 caracteres' : null),
      email: (value) => (/^\S+@\S+$/.test(value) ? null : 'Email inválido'),
      subject: (value) => (value.length < 5 ? 'O assunto deve ter pelo menos 5 caracteres' : null),
      message: (value) => (value.length < 10 ? 'A mensagem deve ter pelo menos 10 caracteres' : null),
    },
  });

  const handleSubmit = (values: typeof form.values) => {
    console.log('Contact form submitted:', values);
    // Here you would typically send this to your backend
    form.reset();
  };

  return (
    <Box py="xl" id="contact-section">
      <Stack gap="xl">
        <Title order={2} ta="center">Contacta-nos</Title>
        
        <Box maw={600} mx="auto" w="100%">
          <form onSubmit={form.onSubmit(handleSubmit)}>
            <Stack gap="md">
              <TextInput
                label="Nome"
                placeholder="O teu nome"
                withAsterisk
                {...form.getInputProps('name')}
              />
              
              <TextInput
                label="Email"
                placeholder="O teu email"
                withAsterisk
                {...form.getInputProps('email')}
              />

              <TextInput
                label="Assunto"
                placeholder="Assunto da mensagem"
                withAsterisk
                {...form.getInputProps('subject')}
              />

              <Textarea
                label="Mensagem"
                placeholder="A tua mensagem..."
                minRows={4}
                withAsterisk
                {...form.getInputProps('message')}
              />

              <Group justify="flex-end" mt="md">
                <Button type="submit">Enviar Mensagem</Button>
              </Group>
            </Stack>
          </form>
        </Box>
      </Stack>
    </Box>
  );
}
