import { ScrollView, View } from "tamagui";
import { SafeAreaView } from "react-native-safe-area-context";
import { Header } from "../components/home/Header";
import { Categories } from "../components/home/Categories";
import { FeaturedCarousel } from "../components/home/FeaturedCarousel";
import { ProductGrid } from "../components/home/ProductGrid";
import { ProductProvider } from "@/context/use_product_context";

export default function Home() {
  return (
    <ProductProvider>
      <SafeAreaView>
        <Header />
        <ScrollView showsVerticalScrollIndicator={false}>
          <Categories />
          <FeaturedCarousel />
          <ProductGrid />
          <View height={120} />
        </ScrollView>
      </SafeAreaView>
    </ProductProvider>
  );
}
