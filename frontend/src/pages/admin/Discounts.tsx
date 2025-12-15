import { useEffect, useState } from "react";
import {
  Table,
  Button,
  Group,
  Text,
  Modal,
  NumberInput,
  Stack,
  ActionIcon,
  TextInput,
  Select,
  Card,
  Badge,
} from "@mantine/core";

import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import { discountService } from "@services/discounts";
import { productService } from "@services/products";
import type { Discount } from "@_types/discount";
import type { Product } from "@_types/product";
import { notifications } from "@mantine/notifications";
import { LogType } from "@_types/debug";
import { logger } from "@utils/debug";

export const AdminDiscounts = () => {
  const [discounts, setDiscounts] = useState<Discount[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingDiscount, setEditingDiscount] = useState<Discount | null>(null);
  const [formData, setFormData] = useState({
    productId: "",
    percentage: 0,
    startDate: new Date().getTime(),
    endDate: new Date().getTime(),
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [discountsData, productsData] = await Promise.all([
        discountService.getAll(),
        productService.getAll(),
      ]);
      setDiscounts(discountsData);
      setProducts(productsData);
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao carregar dados",
        color: "red",
      });
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingDiscount(null);
    setFormData({
      productId: "",
      percentage: 0,
      startDate: new Date().getTime(),
      endDate: new Date().getTime(),
    });
    setModalOpen(true);
  };

  const handleEdit = (discount: Discount) => {
    setEditingDiscount(discount);
    setFormData({
      productId: discount.productId,
      percentage: discount.percentage,
      startDate: discount.startDate,
      endDate: discount.endDate,
    });

    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = {
        productId: formData.productId,
        percentage: formData.percentage,
        startDate: formData.startDate,
        endDate: formData.endDate,
      };

      logger(LogType.INFO, "Payload Discount", payload);

      if (editingDiscount) {
        const result = await discountService.update(
          editingDiscount.id,
          payload
        );

        if (!result) {
          return notifications.show({
            title: "Erro",
            message: "Falha ao atualizar desconto",
            color: "red",
          });
        }

        notifications.show({
          title: "Sucesso",
          message: "Desconto atualizado com sucesso",
          color: "green",
        });
      } else {
        const result = await discountService.create({
          id: crypto.randomUUID(),
          ...payload,
        });

        if (!result) {
          return notifications.show({
            title: "Erro",
            message: "Falha ao guardar desconto",
            color: "red",
          });
        }
      }

      notifications.show({
        title: "Sucesso",
        message: "Desconto criado com sucesso",
        color: "green",
      });

      setModalOpen(false);
      loadData();
    } catch (error) {
      logger(LogType.ERROR, "Failed to save discount", error);
      notifications.show({
        title: "Erro",
        message: "Falha ao guardar desconto",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tens a certeza que queres eliminar este desconto?")) return;

    try {
      await discountService.delete(id);
      notifications.show({
        title: "Sucesso",
        message: "Desconto eliminado com sucesso",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Erro",
        message: "Falha ao eliminar desconto",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Carregando...</Text>;

  return (
    <Stack gap="lg">
      <Group justify="space-between" align="center">
        <div>
          <Text size="xl" fw={700}>
            Descontos
          </Text>
          <Text size="sm" c="dimmed">
            Gerir descontos e campanhas
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Desconto
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={800}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Produto</Table.Th>
                <Table.Th>Percentagem</Table.Th>
                <Table.Th>Data de Início</Table.Th>
                <Table.Th>Data de Fim</Table.Th>
                <Table.Th>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {!discounts || discounts.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={5} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhum desconto encontrado
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                discounts.map((discount) => {
                  const product = products.find(
                    (p) => p.id === discount.productId
                  );
                  const startTime = new Date(discount.startDate);
                  const endTime = new Date(discount.endDate);

                  logger(LogType.INFO, "Start Time", startTime.getTime());
                  logger(LogType.INFO, "End Time", endTime.getTime());
                  return (
                    <Table.Tr key={discount.id}>
                      <Table.Td>
                        <Text fw={500} size="sm">
                          {product?.name || discount.productId}
                        </Text>
                      </Table.Td>
                      <Table.Td>
                        <Badge color="green" variant="light">
                          {discount.percentage}% de Desconto
                        </Badge>
                      </Table.Td>
                      <Table.Td>{startTime.toLocaleString("pt-PT")}</Table.Td>
                      <Table.Td>{endTime.toLocaleString("pt-PT")}</Table.Td>
                      <Table.Td>
                        <Group gap="xs">
                          <ActionIcon
                            variant="subtle"
                            color="blue"
                            onClick={() => handleEdit(discount)}
                          >
                            <IconEdit size={16} />
                          </ActionIcon>
                          <ActionIcon
                            variant="subtle"
                            color="red"
                            onClick={() => handleDelete(discount.id)}
                          >
                            <IconTrash size={16} />
                          </ActionIcon>
                        </Group>
                      </Table.Td>
                    </Table.Tr>
                  );
                })
              )}
            </Table.Tbody>
          </Table>
        </Table.ScrollContainer>
      </Card>

      <Modal
        opened={modalOpen}
        onClose={() => setModalOpen(false)}
        title={editingDiscount ? "Editar Desconto" : "Criar Desconto"}
      >
        <Stack>
          <Select
            label="Produto"
            value={formData.productId}
            onChange={(val) =>
              setFormData({ ...formData, productId: val || "" })
            }
            data={products.map((p) => ({ value: p.id, label: p.name }))}
            required
          />
          <NumberInput
            label="Percentagem"
            value={formData.percentage}
            onChange={(val) =>
              setFormData({ ...formData, percentage: Number(val) || 0 })
            }
            min={0}
            max={100}
            required
          />
          <TextInput
            label="Data de Início"
            type="datetime-local"
            value={new Date(formData.startDate).toISOString().slice(0, 16)}
            onChange={(e) => {
              const date = new Date(e.target.value);
              console.log(date.getTime());
              setFormData({ ...formData, startDate: date.getTime() });
            }}
            required
          />
          <TextInput
            label="Data de Fim"
            type="datetime-local"
            value={new Date(formData.endDate).toISOString().slice(0, 16)}
            onChange={(e) => {
              const date = new Date(e.target.value);
              console.log(date.getTime());
              setFormData({ ...formData, endDate: date.getTime() });
            }}
            required
          />
          <Button onClick={handleSubmit}>
            {editingDiscount ? "Atualizar" : "Criar"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminDiscounts;
