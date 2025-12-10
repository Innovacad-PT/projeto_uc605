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
    { technicalSpecsId: string; key: string; value: string }[]
  >([]);
  const [newSpec, setNewSpec] = useState({ technicalSpecsId: "", value: "" });

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
    setNewSpec({ technicalSpecsId: "", value: "" });
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
      product.technicalSpecs?.map((spec) => ({
        technicalSpecsId: spec.id,
        key: spec.name,
        value: spec.value || "",
      })) || []
    );
    setNewSpec({ technicalSpecsId: "", value: "" });
    setModalOpen(true);
  };

  const handleSubmit = async () => {
    try {
      if (editingProduct) {
        const payload: any = { ...formData };
        if (imageFile) {
          payload.imageUrl = imageFile;
        }

        // Handle Tech Specs separate or if backend accepts them in update
        // Assuming FormData update is preferred or we need to send JSON with images separately
        // Since original update was JSON, let's keep it uniform or switch to FormData if needed.
        // If API expects FormData for update now... let's check `productService.update`
        // It seems `productService.update` takes `any` payload.
        // If we need to update specs, we likely need to send them.
        // Let's assume the backend handles list of specs in JSON or FormData.
        // Given mixed usage, let's check if we can just append them.

        // Actually, for Tech Specs to update correctly, we likely need to send them.
        // Let's stick to the previous implementation style but add specs.
        payload.technicalSpecs = productSpecs.map((spec) => ({
          technicalSpecsId: spec.technicalSpecsId,
          value: spec.value,
        }));

        await productService.update(editingProduct.id, payload);
        notifications.show({
          title: "Success",
          message: "Product updated successfully",
          color: "green",
        });
      } else {
        const formDataObj = new FormData();
        formDataObj.append("Id", uuidv4().toString());
        formDataObj.append("Name", formData.name);
        formDataObj.append("Price", formatPriceForApi(formData.price));
        formDataObj.append("Details", formData.description);
        formDataObj.append("BrandId", formData.brandId);
        formDataObj.append("CategoryId", formData.categoryId);

        if (imageFile) {
          formDataObj.append("Image", imageFile);
        }

        productSpecs.forEach((spec, index) => {
          formDataObj.append(
            `TechnicalSpecs[${index}].TechnicalSpecsId`,
            spec.technicalSpecsId
          );
          formDataObj.append(`TechnicalSpecs[${index}].Value`, spec.value);
        });

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
    if (!newSpec.technicalSpecsId || !newSpec.value) return;
    const specKey =
      availableSpecs.find(
        (s) => s.technicalSpecsId === newSpec.technicalSpecsId
      )?.key || "";
    setProductSpecs([...productSpecs, { ...newSpec, key: specKey }]);
    setNewSpec({ technicalSpecsId: "", value: "" });
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
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Brand</th>
            <th>Category</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.id}>
              <td>{product.id}</td>
              <td>{product.name}</td>
              <td>€{formatPriceForApi(product.price)}</td>
              <td>{product.stock || 0}</td>
              <td>{product.brand?.name || "-"}</td>
              <td>{product.category?.name || "-"}</td>
              <td>
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
              </td>
            </tr>
          ))}
        </tbody>
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
          <Button onClick={handleSubmit}>
            {editingProduct ? "Update" : "Create"}
          </Button>

          <Text size="sm" fw={700} mt="md">
            Technical Specifications
          </Text>
          <Group align="flex-end">
            <Select
              label="Spec Key"
              placeholder="Select spec"
              data={availableSpecs.map((s) => ({
                value: s.technicalSpecsId,
                label: s.key,
              }))}
              value={newSpec.technicalSpecsId}
              onChange={(val) =>
                setNewSpec({ ...newSpec, technicalSpecsId: val || "" })
              }
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
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminProducts;
