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
        title: "Error",
        message: "Failed to load technical specs",
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
    });
    setModalOpen(true);
  };

  const handleEdit = (spec: TechnicalSpecsEntity) => {
    setEditingSpec(spec);
    setFormData({
      key: spec.key,
    });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingSpec) {
        await updateTechSpec(editingSpec.technicalSpecsId, {
          key: formData.key,
        });
        notifications.show({
          title: "Success",
          message: "Technical spec updated successfully",
          color: "green",
        });
      } else {
        await createTechSpec({
          technicalSpecsId: crypto.randomUUID(),
          key: formData.key,
        });
        notifications.show({
          title: "Success",
          message: "Technical spec created successfully",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to save technical spec",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this spec?")) return;

    try {
      await deleteTechSpec(id);
      notifications.show({
        title: "Success",
        message: "Technical spec deleted successfully",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to delete technical spec",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Loading...</Text>;

  return (
    <Stack>
      <Group justify="space-between">
        <Text size="xl" fw={700}>
          Technical Specifications
        </Text>
        <Button leftSection={<IconPlus size={16} />} onClick={handleCreate}>
          Add Tech Spec
        </Button>
      </Group>

      <Table striped highlightOnHover>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Key</Table.Th>
            <Table.Th>Actions</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {specs.map((spec) => (
            <Table.Tr key={spec.technicalSpecsId}>
              <Table.Td>{spec.key}</Table.Td>
              <Table.Td>
                <Group gap="xs">
                  <ActionIcon color="blue" onClick={() => handleEdit(spec)}>
                    <IconEdit size={16} />
                  </ActionIcon>
                  <ActionIcon
                    color="red"
                    onClick={() => handleDelete(spec.technicalSpecsId)}
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
        title={editingSpec ? "Edit Tech Spec" : "Create Tech Spec"}
      >
        <Stack>
          <TextInput
            label="Key"
            placeholder="e.g., Processor, RAM"
            value={formData.key}
            onChange={(e) => setFormData({ ...formData, key: e.target.value })}
            required
          />

          <Button onClick={handleSubmit}>
            {editingSpec ? "Update" : "Create"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminTechSpecs;
