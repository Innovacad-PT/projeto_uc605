import { TouchableOpacity, Dimensions } from "react-native";
import { YStack, Text, XStack, Image, Button, Card } from "tamagui";
import { Link } from "expo-router";

const { width } = Dimensions.get("window");
const cardWidth = (width - 48) / 2;

import { useProduct } from "@/context/use_product_context";
import { Product } from "../../data/products";

export function ProductGrid() {
  const { products } = useProduct();
  const recommendedProducts = [...products];
  // Random products
  recommendedProducts.sort(() => Math.random() - 0.5);

  return (
    <YStack padding="$4">
      <Text fontFamily="MontserratBold" fontSize={18} marginBottom="$3">
        Recomendados para ti
      </Text>
      <XStack flexWrap="wrap" justifyContent="space-between">
        {recommendedProducts.slice(0, 12).map((product: Product) => (
          <Link key={product.id} href={`/products/${product.id}`} asChild>
            <TouchableOpacity>
              <Card
                width={cardWidth}
                backgroundColor="white"
                borderRadius="$4"
                marginBottom="$4"
                elevation={2}
                overflow="hidden"
              >
                <Image
                  source={{ uri: product.imageUrl }}
                  width="100%"
                  height={120}
                  resizeMode="contain"
                />
                <YStack padding="$3" space="$1">
                  <XStack alignItems="center" marginBottom="$2">
                    <Text fontFamily="MontserratBold" fontSize={14}>
                      {product.name}
                    </Text>
                    {product.discount && (
                      <Text
                        fontFamily="MontserratRegular"
                        fontSize={12}
                        color="white"
                        width="auto"
                        borderRadius="$2"
                        backgroundColor="red"
                        paddingHorizontal="$2"
                        paddingVertical="$1"
                        marginLeft="$1"
                      >
                        -{product.discount.percentage}%
                      </Text>
                    )}
                  </XStack>

                  {product.discount ? (
                    <XStack flex={1} alignItems="flex-end">
                      <Text fontFamily="MontserratBold" fontSize={16}>
                        {(
                          product.price -
                          (product.price * product.discount.percentage) / 100
                        ).toFixed(2)}
                        €
                      </Text>
                      <Text
                        fontFamily="MontserratRegular"
                        fontSize={12}
                        color="gray"
                        textDecorationLine="line-through"
                      >
                        {product.price.toFixed(2)} €
                      </Text>
                    </XStack>
                  ) : (
                    <Text
                      fontFamily="MontserratBold"
                      fontSize={16}
                      color="$blue10"
                    >
                      {product.price.toFixed(2)} €
                    </Text>
                  )}
                  <Button size="$2" backgroundColor="$blue10" marginTop="$2">
                    <Text fontSize={12} fontWeight="bold" color="white">
                      Adicionar
                    </Text>
                  </Button>
                </YStack>
              </Card>
            </TouchableOpacity>
          </Link>
        ))}
      </XStack>
    </YStack>
  );
}
