import { View } from "tamagui";
import { Link } from "expo-router";
import { SafeAreaView } from "react-native-safe-area-context";

export default function Index() {
  return (
    <SafeAreaView>
      <View>
        <Link href="/splash">Splash</Link>
        <Link href="/products">Products</Link>
        <Link href="/products/1">Product ID: 1</Link>
        <Link href="/products/50">Product ID: 50</Link>
      </View>
    </SafeAreaView>
  );
}
