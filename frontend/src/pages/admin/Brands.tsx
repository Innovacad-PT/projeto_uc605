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
        title: "Error",
        message: "Failed to load brands",
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
        await brandService.update(editingBrand.id, formData);
        notifications.show({
          title: "Success",
          message: "Brand updated successfully",
          color: "green",
        });
      } else {
        await brandService.create({
          id: crypto.randomUUID(),
          name: formData.name,
        });
        notifications.show({
          title: "Success",
          message: "Brand created successfully",
          color: "green",
        });
      }
      setModalOpen(false);
      loadBrands();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to save brand",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this brand?")) return;

    try {
      await brandService.delete(id);
      notifications.show({
        title: "Success",
        message: "Brand deleted successfully",
        color: "green",
      });
      loadBrands();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to delete brand",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Loading...</Text>;

  return (
    <Stack>
      <Group justify="space-between">
        <Text size="xl" fw={700}>
          Brands
        </Text>
        <Button leftSection={<IconPlus size={16} />} onClick={handleCreate}>
          Add Brand
        </Button>
      </Group>

      <Table striped highlightOnHover>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Name</Table.Th>
            <Table.Th>Actions</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {brands.map((brand) => (
            <Table.Tr key={brand.id}>
              <Table.Td>{brand.name}</Table.Td>
              <Table.Td>
                <Group gap="xs">
                  <ActionIcon color="blue" onClick={() => handleEdit(brand)}>
                    <IconEdit size={16} />
                  </ActionIcon>
                  <ActionIcon
                    color="red"
                    onClick={() => handleDelete(brand.id)}
                  >
                    <IconTrash size={16} />
                  </ActionIcon>
                </Group>
              </Table.Td>
            </Table.Tr>
          ))}
        </Table.Tbody>
      </Table>

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
