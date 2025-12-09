import { ProductProvider } from "@/context/use_product_context";
import { SafeAreaView } from "react-native-safe-area-context";
import { ScrollView, View } from "tamagui";
import { Categories } from "../components/home/Categories";
import { FeaturedCarousel } from "../components/home/FeaturedCarousel";
import { Header } from "../components/home/Header";
import { ProductGrid } from "../components/home/ProductGrid";

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
