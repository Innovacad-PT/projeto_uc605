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
  Card,
  Badge,
} from "@mantine/core";
import { IconEdit, IconTrash, IconPlus } from "@tabler/icons-react";
import { productService } from "@services/products";
import { brandService } from "@services/brands";
import { categoryService } from "@services/categories";
import type { Product, TechnicalSpec } from "@_types/product";
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
  const [productSpecs, setProductSpecs] = useState<TechnicalSpec[]>([]);
  const [newSpec, setNewSpec] = useState<TechnicalSpec>({
    id: "",
    value: "",
    key: "",
  });

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: "0,00",
    stock: 0,
    brandId: "",
    categoryId: "",
    imageUrl: "null | string",
    technicalSpecs: [] as TechnicalSpec[],
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
      console.log("Brands:", brandsData);
      console.log("Categories:", categoriesData);
      setBrands(brandsData || []);
      setCategories(categoriesData || []);
      setAvailableSpecs(specsData || []);
    } catch (error) {
      console.error("LoadData Error:", error);
      notifications.show({
        title: "Erro",
        message: "Erro ao carregar os dados",
        color: "red",
        autoClose: false,
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
      technicalSpecs: [],
    });
    setImageFile(null);
    setProductSpecs([]);
    setNewSpec({ id: "", value: "", key: "" });
    setModalOpen(true);
  };

  const handleEdit = (product: Product) => {
    setEditingProduct(product);
    setFormData({
      name: product.name,
      description: product.description || "",
      price: formatPriceForApi(product.price),
      stock: product.stock || 0,
      brandId: product.brand?.id ? String(product.brand.id) : "",
      categoryId: product.category?.id ? String(product.category.id) : "",
      imageUrl: product.imageUrl || "",
      technicalSpecs: product.technicalSpecs || [],
    });
    setImageFile(null);
    setProductSpecs(product.technicalSpecs || []);
    setNewSpec({ id: "", value: "", key: "" });
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

      if (formData.brandId) {
        formDataObj.append("BrandId", formData.brandId);
      }
      if (formData.categoryId) {
        formDataObj.append("CategoryId", formData.categoryId);
      }

      if (imageFile) {
        formDataObj.append("ImageFile", imageFile);
      }

      console.log("Submitting specs:", productSpecs);

      productSpecs.forEach((spec, index) => {
        formDataObj.append(`TechnicalSpecs[${index}]`, spec.id);
      });

      console.log(formDataObj);

      if (editingProduct) {
        await productService.update(editingProduct.id, formDataObj);
        notifications.show({
          title: "Sucesso",
          message: "Produto atualizado com sucesso",
          color: "green",
        });
      } else {
        await productService.create(formDataObj);
        notifications.show({
          title: "Sucesso",
          message: "Produto criado com sucesso",
          color: "green",
        });
      }
      setModalOpen(false);
      loadData();
    } catch {
      notifications.show({
        title: "Erro",
        message: "Erro ao guardar o produto",
        color: "red",
      });
    }
  };

  const handleAddSpec = () => {
    console.log(newSpec);
    if (!newSpec.key || !newSpec.value) return;

    console.log(productSpecs);
    setProductSpecs([...productSpecs, newSpec]);
    setNewSpec({ id: "", value: "", key: "" });
  };

  const handleRemoveSpec = (index: number) => {
    setProductSpecs(productSpecs.filter((_, i) => i !== index));
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Tem a certeza que deseja eliminar este produto?")) return;

    try {
      await productService.delete(id);
      notifications.show({
        title: "Sucesso",
        message: "Produto eliminado com sucesso",
        color: "green",
      });
      loadData();
    } catch {
      notifications.show({
        title: "Error",
        message: "Failed to delete product",
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
            Produtos
          </Text>
          <Text size="sm" c="dimmed">
            Gerir o inventário de produtos
          </Text>
        </div>
        <Button
          leftSection={<IconPlus size={16} />}
          onClick={handleCreate}
          variant="filled"
          color="blue"
        >
          Adicionar Produto
        </Button>
      </Group>

      <Card withBorder shadow="sm" radius="md" p="md">
        <Table.ScrollContainer minWidth={800}>
          <Table striped highlightOnHover verticalSpacing="sm">
            <Table.Thead>
              <Table.Tr>
                <Table.Th>Nome</Table.Th>
                <Table.Th>Preço</Table.Th>
                <Table.Th>Stock</Table.Th>
                <Table.Th>Marca</Table.Th>
                <Table.Th>Categoria</Table.Th>
                <Table.Th>Ações</Table.Th>
              </Table.Tr>
            </Table.Thead>
            <Table.Tbody>
              {products.length === 0 ? (
                <Table.Tr>
                  <Table.Td colSpan={7} align="center">
                    <Text c="dimmed" py="xl">
                      Nenhum produto encontrado
                    </Text>
                  </Table.Td>
                </Table.Tr>
              ) : (
                products.map((product) => (
                  <Table.Tr key={product.id}>
                    <Table.Td>
                      <Group gap="sm">
                        {product.imageUrl &&
                          product.imageUrl !== "null | string" && (
                            <img
                              src={product.imageUrl}
                              alt={product.name}
                              style={{
                                width: 32,
                                height: 32,
                                borderRadius: 4,
                                objectFit: "cover",
                              }}
                            />
                          )}
                        <Text fw={500} size="sm">
                          {product.name}
                        </Text>
                      </Group>
                    </Table.Td>
                    <Table.Td>
                      <Text fw={600} size="sm">
                        €{formatPriceForApi(product.price)}
                      </Text>
                    </Table.Td>
                    <Table.Td>
                      <Badge
                        color={product.stock > 0 ? "green" : "red"}
                        variant="light"
                      >
                        {product.stock || 0} em stock
                      </Badge>
                    </Table.Td>
                    <Table.Td>{product.brand?.name || "-"}</Table.Td>
                    <Table.Td>{product.category?.name || "-"}</Table.Td>
                    <Table.Td>
                      <Group gap="xs">
                        <ActionIcon
                          variant="subtle"
                          color="blue"
                          onClick={() => handleEdit(product)}
                        >
                          <IconEdit size={16} />
                        </ActionIcon>
                        <ActionIcon
                          variant="subtle"
                          color="red"
                          onClick={() => handleDelete(product.id)}
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
        title={editingProduct ? "Editar Produto" : "Criar Produto"}
      >
        <Stack>
          <TextInput
            label="Nome"
            value={formData.name}
            onChange={(e) => setFormData({ ...formData, name: e.target.value })}
            required
          />
          <TextInput
            label="Descrição"
            value={formData.description}
            onChange={(e) =>
              setFormData({ ...formData, description: e.target.value })
            }
          />
          <FileInput
            label="Imagem do Produto"
            placeholder="Escolha o ficheiro da imagem"
            value={imageFile}
            onChange={(file) => setImageFile(file)}
            accept="image/*"
          />
          <NumberInput
            label="Preço"
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
            label="Marca"
            value={formData.brandId}
            onChange={(val) => {
              console.log(val);
              return setFormData({ ...formData, brandId: val || "" });
            }}
            data={
              brands.length > 0
                ? brands.map((b) => ({
                    value: String(b.id),
                    label: b.name,
                  }))
                : []
            }
            comboboxProps={{ withinPortal: false }}
          />
          <Select
            label="Categoria"
            value={formData.categoryId}
            onChange={(val) => {
              console.log(val);
              return setFormData({ ...formData, categoryId: val || "" });
            }}
            data={
              categories.length > 0
                ? categories.map((c) => ({
                    value: String(c.id),
                    label: c.name,
                  }))
                : []
            }
            comboboxProps={{ withinPortal: false }}
          />

          <Text size="sm" fw={700} mt="md">
            Especificações Técnicas
          </Text>
          <Group align="flex-end">
            <Select
              label="Especificação"
              placeholder="Selecione a especificação"
              data={
                availableSpecs.length > 0
                  ? availableSpecs.map((s) => ({
                      value: JSON.stringify(s),
                      label: `${s.key} - ${s.value}`,
                    }))
                  : []
              }
              value={JSON.stringify(newSpec)}
              onChange={(val, opt) => {
                console.log(val, opt);
                console.log(JSON.parse(val!));
                return setNewSpec(JSON.parse(val!));
              }}
              style={{ flex: 1 }}
            />
            <Button onClick={() => handleAddSpec()} variant="outline">
              <IconPlus size={16} />
            </Button>
          </Group>
          <Stack gap="xs" mt="sm">
            {productSpecs.map((spec, index) => (
              <Group
                key={index}
                justify="space-between"
                bg="gray.9"
                p="xs"
                style={{ borderRadius: 4, border: "1px solid gray" }}
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
            {editingProduct ? "Atualizar" : "Criar"}
          </Button>
        </Stack>
      </Modal>
    </Stack>
  );
};

export default AdminProducts;
