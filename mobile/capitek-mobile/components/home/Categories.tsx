import { ScrollView, TouchableOpacity } from "react-native";
import { YStack, Text, Circle, XStack } from "tamagui";
import { Feather } from "@expo/vector-icons";

type IconName = keyof typeof Feather.glyphMap;

const categories: {
  id: number;
  name: string;
  icon: IconName;
  color: string;
}[] = [
  { id: 1, name: "Computadores", icon: "monitor", color: "#FF6B6B" },
  { id: 2, name: "Componentes", icon: "cpu", color: "#4ECDC4" },
  { id: 3, name: "Armazenamento", icon: "hard-drive", color: "#45B7D1" },
  { id: 4, name: "Periféricos", icon: "headphones", color: "#96CEB4" },
  { id: 5, name: "Smartphones", icon: "smartphone", color: "#FFEEAD" },
  { id: 6, name: "Áudio", icon: "speaker", color: "#D4A5A5" },
];

export function Categories() {
  return (
    <YStack paddingVertical="$4" backgroundColor="white">
      <Text
        fontFamily="MontserratBold"
        fontSize={18}
        paddingHorizontal="$4"
        marginBottom="$3"
      >
        Categorias
      </Text>
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={{ paddingHorizontal: 16 }}
      >
        <XStack space="$4">
          {categories.map((cat) => (
            <TouchableOpacity key={cat.id}>
              <YStack alignItems="center" space="$2" width={80}>
                <Circle
                  size={60}
                  backgroundColor={cat.color}
                  justifyContent="center"
                  alignItems="center"
                >
                  <Feather name={cat.icon} size={24} color="white" />
                </Circle>
                <Text
                  fontFamily="Montserrat"
                  fontSize={12}
                  textAlign="center"
                  numberOfLines={1}
                >
                  {cat.name}
                </Text>
              </YStack>
            </TouchableOpacity>
          ))}
        </XStack>
      </ScrollView>
    </YStack>
  );
}
