import {
  createContext,
  PropsWithChildren,
  useContext,
  useEffect,
  useState,
} from "react";
import { Product } from "../data/products";
import Toast from "react-native-toast-message";

type ProductContextType = {
  products: Product[];
  setProducts: (products: Product[]) => void;
  filterProducts: (category: string) => void;
  searchProducts: (category: string) => void;
  isLoading: boolean;
  filteredProducts: Product[];
};

const ProductContext = createContext<ProductContextType>({
  products: [],
  setProducts: () => {},
  filterProducts: () => {},
  searchProducts: () => {},
  isLoading: false,
  filteredProducts: [],
});

export function ProductProvider({ children }: PropsWithChildren) {
  const [products, setProducts] = useState<Product[]>([]);
  const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    async function loadProducts() {
      try {
        const response = await fetch("http://148.63.1.66:7000/products");
        const data = await response.json();

        if (response.status == 500) {
          throw new Error("Error no servidor");
        }

        if (data.hasError) {
          Toast.show({
            type: "error",
            text1: "Erro",
            text2: data.message,
          });
          return;
        }

        data.value.forEach(async (product: Product) => {
          product.imageUrl = "http://148.63.1.66:7000/" + product.imageUrl;

          const discountResponse = await fetch(
            "http://148.63.1.66:7000/products/" + product.id + "/discount"
          );
          const discountData = await discountResponse.json();

          if (discountData.hasError) {
            console.log(discountData.message);
            return;
          }

          product.discount = discountData.value;
        });

        setProducts(data.value);
        setIsLoading(false);
      } catch (error) {
        console.error("Failed to load products", error);
        setIsLoading(false);
      }
    }

    loadProducts();
  }, []);

  const searchProducts = (text: string) => {
    // search parameters are wrong
    const filtered = products.filter((product) =>
      product.name.toLowerCase().includes(text.toLowerCase())
    );
    console.log(filtered);
    setFilteredProducts(filtered);
  };

  const filterProducts = (category: string) => {
    if (category === "all") {
      setFilteredProducts(products);
    } else {
      const filtered = products.filter((product) =>
        product.category.name.toLowerCase().includes(category.toLowerCase())
      );
      setFilteredProducts(filtered);
    }
  };

  return (
    <ProductContext.Provider
      value={{
        products,
        setProducts,
        filterProducts,
        searchProducts,
        isLoading,
        filteredProducts,
      }}
    >
      {children}
    </ProductContext.Provider>
  );
}

export const useProduct = () => useContext(ProductContext);
