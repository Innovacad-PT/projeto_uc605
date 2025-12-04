import { ScrollView, Dimensions } from "react-native";
import { YStack, Text, XStack, Button } from "tamagui";
import { LinearGradient } from "@tamagui/linear-gradient";

const { width } = Dimensions.get("window");

const promotions = [
  {
    id: 1,
    title: "PC Gaming Extreme",
    subtitle: "Até 20% de desconto",
    colors: ["#4c669f", "#3b5998", "#192f6a"],
  },
  {
    id: 2,
    title: "Periféricos Razer",
    subtitle: "Novidades em stock",
    colors: ["#00b09b", "#96c93d"],
  },
  {
    id: 3,
    title: "Portáteis ASUS",
    subtitle: "Oferta de mochila",
    colors: ["#833ab4", "#fd1d1d", "#fcb045"],
  },
];

export function FeaturedCarousel() {
  return (
    <YStack paddingVertical="$4">
      <Text
        fontFamily="MontserratBold"
        fontSize={18}
        paddingHorizontal="$4"
        marginBottom="$3"
      >
        Destaques
      </Text>
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={{ paddingHorizontal: 16 }}
        pagingEnabled
      >
        <XStack space="$4">
          {promotions.map((promo) => (
            <LinearGradient
              key={promo.id}
              width={width - 40}
              height={180}
              borderRadius="$4"
              colors={promo.colors}
              start={[0, 0]}
              end={[1, 1]}
              padding="$4"
              justifyContent="center"
            >
              <YStack space="$2">
                <Text
                  fontFamily="MontserratBold"
                  fontSize={24}
                  color="white"
                  textShadowColor="rgba(0,0,0,0.3)"
                  textShadowOffset={{ width: 1, height: 1 }}
                  textShadowRadius={5}
                >
                  {promo.title}
                </Text>
                <Text
                  fontFamily="Montserrat"
                  fontSize={16}
                  color="white"
                  opacity={0.9}
                >
                  {promo.subtitle}
                </Text>
                <Button
                  backgroundColor="white"
                  color="#000"
                  size="$3"
                  width={120}
                  marginTop="$2"
                  pressStyle={{ opacity: 0.9 }}
                >
                  <Text fontFamily="MontserratBold" fontSize={12}>
                    Ver Oferta
                  </Text>
                </Button>
              </YStack>
            </LinearGradient>
          ))}
        </XStack>
      </ScrollView>
    </YStack>
  );
}
