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
import { brandService } from "@services/brands";
import type { Brand } from "@_types/brand";
import { notifications } from "@mantine/notifications";

export const AdminBrands = () => {
  const [brands, setBrands] = useState<Brand[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingBrand, setEditingBrand] = useState<Brand | null>(null);
  const [formData, setFormData] = useState({ name: "" });

  useEffect(() => {
    loadBrands();
  }, []);

  const loadBrands = async () => {
    try {
      const data = await brandService.getAll();
      setBrands(data);
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao carregar marcas",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingBrand(null);
    setFormData({ name: "" });
    setModalOpen(true);
  };

  const handleEdit = (brand: Brand) => {
    setEditingBrand(brand);
    setFormData({ name: brand.name });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingBrand) {
        console.log(editingBrand);
        await brandService.update(editingBrand.id, formData);
        notifications.show({
          title: "Sucesso",
          message: "Marca atualizada com sucesso",
          color: "green",
        });
      } else {
        await brandService.create({
          id: crypto.randomUUID(),
          name: formData.name,
        });
        notifications.show({
          title: "Sucesso",
          message: "Marca criada com sucesso",
          color: "green",
        });
      }
      setModalOpen(false);
      loadBrands();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao guardar marca",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tens a certeza que queres eliminar esta marca?")) return;

    try {
      await brandService.delete(id);
      notifications.show({
        title: "Sucesso",
        message: "Marca eliminada com sucesso",
        color: "green",
      });
      loadBrands();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao eliminar marca",
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
            Marcas
          </Text>
          <Text size="sm" c="dimmed">
            Gerir marcas de produtos
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Marca
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={600}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Nome</Table.Th>
                <Table.Th style={{ width: 100 }}>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {brands.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={2} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhuma marca encontrada
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                brands.length > 0 &&
                brands.map((brand) => (
                  <Table.Tr key={brand.id}>
                    <Table.Td>
                      <Text fw={500} size="sm">
                        {brand.name}
                      </Text>
                    </Table.Td>
                    <Table.Td>
                      <Group gap="xs">
                        <ActionIcon
                          variant="subtle"
                          color="blue"
                          onClick={() => handleEdit(brand)}
                        >
                          <IconEdit size={16} />
                        </ActionIcon>
                        <ActionIcon
                          variant="subtle"
                          color="red"
                          onClick={() => handleDelete(brand.id)}
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
        title={editingBrand ? "Edit Brand" : "Create Brand"}
      >
        <Stack>
          <TextInput
            label="Name"
            value={formData.name}
            onChange={(e) => setFormData({ name: e.target.value })}
            required
          />
          <Button onClick={handleSubmit}>
            {editingBrand ? "Update" : "Create"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminBrands;
