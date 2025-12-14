import { useEffect, useState } from "react";
import {
  Table,
  Button,
  Group,
  Text,
  Modal,
  TextInput,
  NumberInput,
  Stack,
  ActionIcon,
  Select,
  FileInput,
} from "@mantine/core";
import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import { productService } from "@services/products";
import { brandService } from "@services/brands";
import { categoryService } from "@services/categories";
import type { Product } from "@_types/product";
import type { Brand } from "@_types/brand";
import type { Category } from "@_types/category";
import { notifications } from "@mantine/notifications";
import { v4 as uuidv4 } from "uuid";
import {
  getAllTechSpecs,
  type TechnicalSpecsEntity,
} from "@services/techSpecs";

const formatPriceForApi = (price: string | number): string => {
  price.toString().replace("€", "");
  return price.toString().replace(/\./g, ",");
};

export const AdminProducts = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [brands, setBrands] = useState<Brand[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalOpen, setModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [imageFile, setImageFile] = useState<File | null>(null);

  const [availableSpecs, setAvailableSpecs] = useState<TechnicalSpecsEntity[]>(
    []
  );
  const [productSpecs, setProductSpecs] = useState<
    { id: string; key: string; value: string }[]
  >([]);
  const [newSpec, setNewSpec] = useState({ id: "", value: "" });

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: "0,00",
    stock: 0,
    brandId: "",
    categoryId: "",
    imageUrl: "null | string",
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [productsData, brandsData, categoriesData, specsData] =
        await Promise.all([
          productService.getAll(),
          brandService.getAll(),
          categoryService.getAll(),
          getAllTechSpecs(),
        ]);
      setProducts(productsData);
      setBrands(brandsData);
      setCategories(categoriesData);
      setAvailableSpecs(specsData);
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
    setEditingProduct(null);
    setFormData({
      name: "",
      description: "",
      price: "0,00",
      stock: 0,
      brandId: "",
      categoryId: "",
      imageUrl: "",
    });
    setImageFile(null);
    setProductSpecs([]);
    setNewSpec({ id: "", value: "" });
    setModalOpen(true);
  };

  const handleEdit = (product: Product) => {
    setEditingProduct(product);
    setFormData({
      name: product.name,
      description: product.description || "",
      price: formatPriceForApi(product.price),
      stock: product.stock || 0,
      brandId: product.brand?.id || "",
      categoryId: product.category?.id || "",
      imageUrl: product.imageUrl || "",
    });
    setImageFile(null);
    setProductSpecs(
      product.technicalSpecs?.map((spec) => {
        // Use id if present (definition ID), otherwise fallback to id
        // This is crucial because 'id' might be the instance ID, but we need definition ID for updates
        const defId = spec.id || spec.id;
        return {
          id: defId,
          key:
            spec.name || availableSpecs.find((s) => s.id === defId)?.key || "",
          value: spec.value || "",
        };
      }) || []
    );
    setNewSpec({ id: "", value: "" });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      const formDataObj = new FormData();
      formDataObj.append(
        "Id",
        editingProduct ? editingProduct.id : uuidv4().toString()
      );
      formDataObj.append("Name", formData.name);
      formDataObj.append("Price", formatPriceForApi(formData.price));
      formDataObj.append("Stock", (formData.stock || 0).toString());
      formDataObj.append("Details", formData.description);
      formDataObj.append("BrandId", formData.brandId);
      formDataObj.append("CategoryId", formData.categoryId);

      if (imageFile) {
        formDataObj.append("Image", imageFile);
      }

      productSpecs.forEach((spec, index) => {
        formDataObj.append(`TechnicalSpecs[${index}].id`, spec.id);
        formDataObj.append(`TechnicalSpecs[${index}].Value`, spec.value);
      });

      if (editingProduct) {
        await productService.update(editingProduct.id, formDataObj);
        notifications.show({
          title: "Success",
          message: "Product updated successfully",
          color: "green",
        });
      } else {
        await productService.create(formDataObj);
        notifications.show({
          title: "Success",
          message: "Product created successfully",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to save product",
        color: "red",
      });
    }
  };

  const handleAddSpec = () => {
    if (!newSpec.id || !newSpec.value) return;
    const specKey = availableSpecs.find((s) => s.id === newSpec.id)?.key || "";
    setProductSpecs([...productSpecs, { ...newSpec, key: specKey }]);
    setNewSpec({ id: "", value: "" });
  };

  const handleRemoveSpec = (index: number) => {
    setProductSpecs(productSpecs.filter((_, i) => i !== index));
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Are you sure you want to delete this product?")) return;

    try {
      await productService.delete(id);
      notifications.show({
        title: "Success",
        message: "Product deleted successfully",
        color: "green",
      });
      loadData();
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to delete product",
        color: "red",
      });
    }
  };

  if (loading) return <Text>Loading...</Text>;

  return (
    <Stack>
      <Group justify="space-between">
        <Text size="xl" fw={700}>
          Products
        </Text>
        <Button leftSection={<IconPlus size={16} />} onClick={handleCreate}>
          Add Product
        </Button>
      </Group>

      <Table striped highlightOnHover>
        <Table.Thead>
          <Table.Tr>
            <Table.Th>Id</Table.Th>
            <Table.Th>Name</Table.Th>
            <Table.Th>Price</Table.Th>
            <Table.Th>Stock</Table.Th>
            <Table.Th>Brand</Table.Th>
            <Table.Th>Category</Table.Th>
            <Table.Th>Actions</Table.Th>
          </Table.Tr>
        </Table.Thead>
        <Table.Tbody>
          {products.map((product) => (
            <Table.Tr key={product.id}>
              <Table.Td>{product.id}</Table.Td>
              <Table.Td>{product.name}</Table.Td>
              <Table.Td>€{formatPriceForApi(product.price)}</Table.Td>
              <Table.Td>{product.stock || 0}</Table.Td>
              <Table.Td>{product.brand?.name || "-"}</Table.Td>
              <Table.Td>{product.category?.name || "-"}</Table.Td>
              <Table.Td>
                <Group gap="xs">
                  <ActionIcon color="blue" onClick={() => handleEdit(product)}>
                    <IconEdit size={16} />
                  </ActionIcon>
                  <ActionIcon
                    color="red"
                    onClick={() => handleDelete(product.id)}
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
        title={editingProduct ? "Edit Product" : "Create Product"}
      >
        <Stack>
          <TextInput
            label="Name"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
            required
          />
          <TextInput
            label="Description"
            value={formData.description}
            onChange={(e) =>
              setFormData({ ...formData, description: e.target.value })
            }
          />
          <FileInput
            label="Product Image"
            placeholder="Choose image file"
            value={imageFile}
            onChange={(file) => setImageFile(file)}
            accept="image/*"
          />
          <NumberInput
            label="Price"
            value={Number(formData.price.replace(",", "."))}
            onChange={(val) =>
              setFormData({
                ...formData,
                price: formatPriceForApi(val.toString()) || "0,00",
              })
            }
            min={0}
            decimalScale={2}
            required
          />
          <NumberInput
            label="Stock"
            value={formData.stock}
            onChange={(val) =>
              setFormData({ ...formData, stock: Number(val) || 0 })
            }
            min={0}
          />
          <Select
            label="Brand"
            value={formData.brandId}
            onChange={(val) => setFormData({ ...formData, brandId: val || "" })}
            data={brands.map((b) => ({ value: b.id, label: b.name }))}
          />
          <Select
            label="Category"
            value={formData.categoryId}
            onChange={(val) =>
              setFormData({ ...formData, categoryId: val || "" })
            }
            data={categories.map((c) => ({ value: c.id, label: c.name }))}
          />

          <Text size="sm" fw={700} mt="md">
            Technical Specifications
          </Text>
          <Group align="flex-end">
            <Select
              label="Spec Key"
              placeholder="Select spec"
              data={availableSpecs.map((s) => ({
                value: s.id,
                label: s.key,
              }))}
              value={newSpec.id}
              onChange={(val) => setNewSpec({ ...newSpec, id: val || "" })}
              style={{ flex: 1 }}
            />
            <TextInput
              label="Value"
              placeholder="Value"
              value={newSpec.value}
              onChange={(e) =>
                setNewSpec({ ...newSpec, value: e.target.value })
              }
              style={{ flex: 1 }}
            />
            <Button onClick={handleAddSpec} variant="outline">
              <IconPlus size={16} />
            </Button>
          </Group>
          <Stack gap="xs" mt="sm">
            {productSpecs.map((spec, index) => (
              <Group
                key={index}
                justify="space-between"
                bg="gray.1"
                p="xs"
                style={{ borderRadius: 4 }}
              >
                <Text size="sm">
                  {spec.key}: {spec.value}
                </Text>
                <ActionIcon
                  color="red"
                  size="sm"
                  onClick={() => handleRemoveSpec(index)}
                >
                  <IconTrash size={14} />
                </ActionIcon>
              </Group>
            ))}
          </Stack>
          <Button onClick={handleSubmit}>
            {editingProduct ? "Update" : "Create"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminProducts;
