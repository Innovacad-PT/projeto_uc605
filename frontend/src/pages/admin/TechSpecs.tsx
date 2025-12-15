import { useEffect, useState } from "react";
import {
  Table,
  Button,
  Group,
  Text,
  Modal,
  TextInput,
  Stack,
  ActionIcon,
  Card,
} from "@mantine/core";
import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import {
  getAllTechSpecs,
  createTechSpec,
  updateTechSpec,
  deleteTechSpec,
  type TechnicalSpecsEntity,
} from "@services/techSpecs";
import { notifications } from "@mantine/notifications";

export const AdminTechSpecs = () => {
  const [specs, setSpecs] = useState<TechnicalSpecsEntity[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingSpec, setEditingSpec] = useState<TechnicalSpecsEntity | null>(
    null
  );

  const [formData, setFormData] = useState({
    key: "",
    value: "",
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const data = await getAllTechSpecs();
      setSpecs(data);
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao carregar especificações técnicas",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingSpec(null);
    setFormData({
      key: "",
      value: "",
    });
    setModalOpen(true);
  };

  const handleEdit = (spec: TechnicalSpecsEntity) => {
    setEditingSpec(spec);
    setFormData({
      key: spec.key,
      value: spec.value,
    });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingSpec) {
        await updateTechSpec(editingSpec.id, {
          key: formData.key,
          value: formData.value,
        });
        notifications.show({
          title: "Sucesso",
          message: "Especificação técnica atualizada com sucesso",
          color: "green",
        });
      } else {
        await createTechSpec({
          id: crypto.randomUUID(),
          key: formData.key,
          value: formData.value,
        });
        notifications.show({
          title: "Sucesso",
          message: "Especificação técnica criada com sucesso",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao guardar especificação técnica",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (
      !confirm("Tens a certeza que queres eliminar esta especificação técnica?")
    )
      return;

    try {
      await deleteTechSpec(id);
      notifications.show({
        title: "Sucesso",
        message: "Especificação técnica eliminada com sucesso",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao eliminar especificação técnica",
        color: "red",
      });
    }
  };

  if (loading) return <Text>A Carregar...</Text>;

  return (
    <Stack gap="lg">
      <Group justify="space-between" align="center">
        <div>
          <Text size="xl" fw={700}>
            Especificações Técnicas
          </Text>
          <Text size="sm" c="dimmed">
            Gerir as especificações técnicas disponíveis para os produtos
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Especificação Técnica
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={600}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Chave</Table.Th>
                <Table.Th>Valor</Table.Th>
                <Table.Th style={{ width: 100 }}>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {specs.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={2} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhuma especificação técnica encontrada
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                specs.length > 0 &&
                specs.map((spec) => (
                  <Table.Tr key={spec.id}>
                    <Table.Td>
                      <Text fw={500} size="sm">
                        {spec.key}
                      </Text>
                    </Table.Td>
                    <Table.Td>
                      <Text fw={500} size="sm">
                        {spec.value}
                      </Text>
                    </Table.Td>
                    <Table.Td>
                      <Group gap="xs">
                        <ActionIcon
                          variant="subtle"
                          color="blue"
                          onClick={() => handleEdit(spec)}
                        >
                          <IconEdit size={16} />
                        </ActionIcon>
                        <ActionIcon
                          variant="subtle"
                          color="red"
                          onClick={() => handleDelete(spec.id)}
                        >
                          <IconTrash size={16} />
                        </ActionIcon>
                      </Group>
                    </Table.Td>
                  </Table.Tr>
                ))
              )}
            </Table.Tbody>
          </Table>
        </Table.ScrollContainer>
      </Card>

      <Modal
        opened={modalOpen}
        onClose={() => setModalOpen(false)}
        title={
          editingSpec
            ? "Editar Especificação Técnica"
            : "Criar Especificação Técnica"
        }
      >
        <Stack>
          <TextInput
            label="Chave"
            placeholder="e.g., Processor, RAM"
            value={formData.key}
            onChange={(e) => setFormData({ ...formData, key: e.target.value })}
            required
          />

          <TextInput
            label="Valor"
            placeholder="e.g, Quantity"
            value={formData.value}
            onChange={(e) =>
              setFormData({ ...formData, value: e.target.value })
            }
            required
          />

          <Button onClick={handleSubmit}>
            {editingSpec ? "Atualizar" : "Criar"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminTechSpecs;
