import {
  XStack,
  Input,
  Circle,
  Text,
  View,
  YStack,
  Card,
  Image,
} from "tamagui";
import { Feather } from "@expo/vector-icons";
import { TouchableOpacity, FlatList } from "react-native";
import { useState } from "react";
import { Link } from "expo-router";
import { Product } from "../../data/products";
import { useProduct } from "@/context/use_product_context";

export function Header() {
  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState<Product[]>([]);
  const [showResults, setShowResults] = useState(false);

  const { filteredProducts, searchProducts } = useProduct();

  const handleSearch = (text: string) => {
    setSearchQuery(text);
    searchProducts(text);
    if (text.length > 0) {
      setSearchResults(filteredProducts);
      setShowResults(true);
    } else {
      setSearchResults([]);
      setShowResults(false);
    }
  };

  return (
    <View zIndex={100}>
      <XStack
        padding="$4"
        alignItems="center"
        justifyContent="space-between"
        backgroundColor="white"
        elevation={2}
      >
        <TouchableOpacity>
          <Feather name="menu" size={24} color="#000" />
        </TouchableOpacity>

        <View flex={1} marginHorizontal="$4" zIndex={101}>
          <Input
            placeholder="Pesquisar produtos..."
            backgroundColor="$gray2"
            borderRadius="$4"
            paddingLeft="$8"
            height={40}
            borderWidth={0}
            value={searchQuery}
            onChangeText={handleSearch}
          />
          <View position="absolute" left={10} top={10}>
            <Feather name="search" size={20} color="#666" />
          </View>

          {showResults && (
            <Card
              position="absolute"
              top={45}
              left={0}
              right={0}
              backgroundColor="white"
              elevation={5}
              borderRadius="$4"
              padding="$2"
              maxHeight={200}
            >
              {searchResults.length > 0 ? (
                <FlatList
                  data={searchResults}
                  keyExtractor={(item: Product) => item.id}
                  renderItem={({ item }) => (
                    <Link href={`/products/${item.id}`} asChild>
                      <TouchableOpacity
                        onPress={() => {
                          setShowResults(false);
                          setSearchQuery("");
                        }}
                      >
                        <XStack
                          borderBottomWidth={1}
                          borderBottomColor="gray"
                          paddingVertical="$2"
                        >
                          <Image
                            source={{ uri: item.imageUrl }}
                            width={50}
                            height={50}
                            marginRight={10}
                          />
                          <YStack paddingVertical="$2">
                            <Text fontFamily="Montserrat" fontSize={14}>
                              {item.name}
                            </Text>
                            <Text
                              fontFamily="MontserratBold"
                              fontSize={12}
                              color="$blue10"
                            >
                              {item.price.toFixed(2)} €
                            </Text>
                          </YStack>
                        </XStack>
                      </TouchableOpacity>
                    </Link>
                  )}
                />
              ) : (
                <Text
                  fontFamily="Montserrat"
                  fontSize={14}
                  color="$gray10"
                  padding="$2"
                  textAlign="center"
                >
                  Nenhum produto encontrado.
                </Text>
              )}
            </Card>
          )}
        </View>

        <TouchableOpacity>
          <View>
            <Feather name="shopping-cart" size={24} color="#000" />
            <Circle
              position="absolute"
              top={-5}
              right={-5}
              size={16}
              backgroundColor="$red10"
              justifyContent="center"
              alignItems="center"
            >
              <Text color="white" fontSize={10} fontWeight="bold">
                2
              </Text>
            </Circle>
          </View>
        </TouchableOpacity>
      </XStack>
    </View>
  );
}
