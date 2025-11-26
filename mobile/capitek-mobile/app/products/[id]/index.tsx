import { useLocalSearchParams } from "expo-router";
import { Text, View } from "tamagui";

export default function ProductId() {
  const searchParams = useLocalSearchParams();

  console.log(searchParams);

  return (
    <View>
      <Text>Product {searchParams.id}</Text>
    </View>
  );
}
