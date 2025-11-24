// src/pages/admin/Categories.tsx

import { useEffect, useState } from "react";
import {
  Table,
  Button,
  Group,
  Text,
  Modal,
  TextInput,
  Stack,
} from "@mantine/core";
import { IconPlus } from "@tabler/icons-react";
import { categoryService } from "@services/categories";
import type { Category } from "@_types/category";
import { notifications } from "@mantine/notifications";

export const AdminCategories = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [formData, setFormData] = useState({ name: "" });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await categoryService.getAll();
      setCategories(data);
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to load categories",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setFormData({ name: "" });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      await categoryService.create({
        id: crypto.randomUUID(),
        name: formData.name,
      });
      notifications.show({
        title: "Success",
        message: "Category created successfully",
        color: "green",
      });
      setModalOpen(false);
      loadCategories();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to save category",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Loading...</Text>;

  return (
    <Stack>
      <Group justify="space-between">
        <Text size="xl" fw={700}>
          Categories
        </Text>
        <Button leftSection={<IconPlus size={16} />} onClick={handleCreate}>
          Add Category
        </Button>
      </Group>

      <Table striped highlightOnHover>
        <thead>
          <tr>
            <th>Name</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((category) => (
            <tr key={category.id}>
              <td>{category.name}</td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal
        opened={modalOpen}
        onClose={() => setModalOpen(false)}
        title="Create Category"
      >
        <Stack>
          <TextInput
            label="Name"
            value={formData.name}
            onChange={(e) => setFormData({ name: e.target.value })}
            required
          />
          <Button onClick={handleSubmit}>Create</Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminCategories;
