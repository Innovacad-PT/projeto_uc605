// src/pages/admin/Discounts.tsx

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
} from "@mantine/core";

import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import { discountService } from "@services/discounts";
import { productService } from "@services/products";
import type { Discount } from "@_types/discount";
import type { Product } from "@_types/product";
import { notifications } from "@mantine/notifications";

export const AdminDiscounts = () => {
  const [discounts, setDiscounts] = useState<Discount[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingDiscount, setEditingDiscount] = useState<Discount | null>(null);
  const [formData, setFormData] = useState({
    productId: "",
    percentage: 0,
    startDate: "",
    endDate: "",
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
        title: "Error",
        message: "Failed to load data",
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
      startDate: "",
      endDate: "",
    });
    setModalOpen(true);
  };

  const handleEdit = (discount: Discount) => {
    setEditingDiscount(discount);
    setFormData({
      productId: discount.productId,
      percentage: discount.percentage,
      startDate: discount.startDate.substring(0, 16),
      endDate: discount.endDate.substring(0, 16),
    });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      const payload = {
        productId: formData.productId,
        percentage: formData.percentage,
        startDate: new Date(formData.startDate).toISOString(),
        endDate: new Date(formData.endDate).toISOString(),
      };

      if (editingDiscount) {
        await discountService.update(editingDiscount.id, payload);
        notifications.show({
          title: "Success",
          message: "Discount updated successfully",
          color: "green",
        });
      } else {
        await discountService.create({
          id: crypto.randomUUID(),
          ...payload,
        });
        notifications.show({
          title: "Success",
          message: "Discount created successfully",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to save discount",
        color: "red",
      });
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this discount?")) return;

    try {
      await discountService.delete(id);
      notifications.show({
        title: "Success",
        message: "Discount deleted successfully",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to delete discount",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Loading...</Text>;

  return (
    <Stack>
      <Group justify="space-between">
        <Text size="xl" fw={700}>
          Discounts
        </Text>
        <Button leftSection={<IconPlus size={16} />} onClick={handleCreate}>
          Add Discount
        </Button>
      </Group>

      <Table striped highlightOnHover>
        <thead>
          <tr>
            <th>Product</th>
            <th>Percentage</th>
            <th>Start Date</th>
            <th>End Date</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {discounts.map((discount) => {
            const product = products.find((p) => p.id === discount.productId);
            return (
              <tr key={discount.id}>
                <td>{product?.name || discount.productId}</td>
                <td>{discount.percentage}%</td>
                <td>{new Date(discount.startDate).toLocaleDateString()}</td>
                <td>{new Date(discount.endDate).toLocaleDateString()}</td>
                <td>
                  <Group gap="xs">
                    <ActionIcon
                      color="blue"
                      onClick={() => handleEdit(discount)}
                    >
                      <IconEdit size={16} />
                    </ActionIcon>
                    <ActionIcon
                      color="red"
                      onClick={() => handleDelete(discount.id)}
                    >
                      <IconTrash size={16} />
                    </ActionIcon>
                  </Group>
                </td>
              </tr>
            );
          })}
        </tbody>
      </Table>

      <Modal
        opened={modalOpen}
        onClose={() => setModalOpen(false)}
        title={editingDiscount ? "Edit Discount" : "Create Discount"}
      >
        <Stack>
          <Select
            label="Product"
            value={formData.productId}
            onChange={(val) =>
              setFormData({ ...formData, productId: val || "" })
            }
            data={products.map((p) => ({ value: p.id, label: p.name }))}
            required
          />
          <NumberInput
            label="Percentage"
            value={formData.percentage}
            onChange={(val) =>
              setFormData({ ...formData, percentage: Number(val) || 0 })
            }
            min={0}
            max={100}
            required
          />
          <TextInput
            label="Start Date"
            type="datetime-local"
            value={formData.startDate}
            onChange={(e) =>
              setFormData({ ...formData, startDate: e.target.value })
            }
            required
          />
          <TextInput
            label="End Date"
            type="datetime-local"
            value={formData.endDate}
            onChange={(e) =>
              setFormData({ ...formData, endDate: e.target.value })
            }
            required
          />
          <Button onClick={handleSubmit}>
            {editingDiscount ? "Update" : "Create"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminDiscounts;
