import { Image, Text, View } from "tamagui";
import { SafeAreaView } from "react-native-safe-area-context";
import { useState, useRef, useEffect } from "react";
import { StyleSheet, Animated } from "react-native";
import { SplashItem } from "@/types/splash_item";
import { useRouter } from "expo-router";

const Dot = ({ isActive }: { isActive: boolean }) => {
  const anim = useRef(new Animated.Value(isActive ? 1 : 0)).current;

  useEffect(() => {
    Animated.timing(anim, {
      toValue: isActive ? 1 : 0,
      duration: 300,
      useNativeDriver: false,
    }).start();
  }, [isActive]);

  const width = anim.interpolate({
    inputRange: [0, 1],
    outputRange: [10, 40],
  });

  const height = anim.interpolate({
    inputRange: [0, 1],
    outputRange: [10, 8],
  });

  const backgroundColor = anim.interpolate({
    inputRange: [0, 1],
    outputRange: ["#17223b1f", "#17223b"],
  });

  return (
    <Animated.View
      style={{
        marginHorizontal: 4,
        borderRadius: 50,
        width,
        height,
        backgroundColor,
      }}
    />
  );
};

export default function Splash() {
  const [page, setPage] = useState(1);
  const [pages, setPages] = useState<SplashItem[]>([
    {
      imagePath: require("@assets/images/splash1.png"),
      title: "Escolhe os Produtos",
      body: "Navegue por milhares de produtos e encontre os seus favoritos com apenas alguns toques!",
    },
    {
      imagePath: require("@assets/images/splash2.png"),
      title: "Faz o Pagamento",
      body: "Finalize a sua compra com total segurança e com as opções de pagamento mais convenientes.",
    },
    {
      imagePath: require("@assets/images/splash3.png"),
      title: "Receba o Pedido",
      body: "A sua satisfação é a nossa prioridade. Receba o seu pedido com a rapidez e conveniência que merece!",
    },
  ]);

  const router = useRouter();

  const handleSkipClick = () => {
    router.replace("/home");
  };

  const handleNextClick = () => {
    if (page == 3) {
      router.replace("/home");
      return;
    }
    setPage(page + 1);
  };

  const handlePreviousClick = () => {
    if (page == 1) return;
    setPage(page - 1);
  };

  return (
    <>
      <SafeAreaView
        style={[styles.flexColumn, styles.background, { padding: 16 }]}
      >
        <View style={[styles.header]}>
          {/*Splash Screen Page Count*/}
          <Text style={styles.text}>
            {page}
            <Text style={styles.textMuted}>/3</Text>
          </Text>
          {/*Splash Screen Skip Button*/}
          <Text style={styles.text} onPress={handleSkipClick}>
            Skip
          </Text>
        </View>

        <View
          style={[
            styles.flexBody,
            styles.flexColumn,
            styles.flexCenter,
            styles.background,
          ]}
        >
          <View style={{ alignItems: "center", justifyContent: "center" }}>
            <Image
              source={pages[page - 1].imagePath}
              style={{
                width: 350,
                height: 350,
                resizeMode: "contain",
              }}
            />
          </View>
          <Text style={styles.textTitle}>{pages[page - 1].title}</Text>
          <Text style={styles.textBody}>{pages[page - 1].body}</Text>
        </View>

        <View style={[styles.footer, styles.background]}>
          {/*Splash Screen Prev Button*/}
          <Text
            style={[
              styles.text,
              styles.textMuted,
              { opacity: page === 1 ? 0 : 1 },
            ]}
            onPress={handlePreviousClick}
          >
            Voltar
          </Text>

          {/* Current Splash Image Dots */}
          <View style={[styles.dotContainer]}>
            <Dot isActive={page === 1} />
            <Dot isActive={page === 2} />
            <Dot isActive={page === 3} />
          </View>

          {/*Splash Screen Next Button*/}
          <Text
            style={[styles.text, { color: "#37B1F8" }]}
            onPress={handleNextClick}
          >
            {page === 3 ? "Começar" : "Avançar"}
          </Text>
        </View>
      </SafeAreaView>
    </>
  );
}

const styles = StyleSheet.create({
  text: {
    fontFamily: "Montserrat",
    fontWeight: "700",
    fontSize: 18,
  },
  textTitle: {
    color: "#000",
    fontFamily: "Montserrat",
    fontWeight: "900",
    fontSize: 26,
    marginBottom: 16,
  },
  textBody: {
    fontFamily: "Montserrat",
    color: "#A8A8A9",
    fontWeight: "600",
    fontSize: 16,
    textAlign: "center",
  },
  background: {
    backgroundColor: "#f5f5f5",
  },
  flexRow: {
    flexDirection: "row",
  },
  flexColumn: {
    flex: 1,
    flexDirection: "column",
  },
  flexBody: {
    flex: 7,
    flexDirection: "row",
  },
  flexCenter: {
    alignItems: "center",
    justifyContent: "center",
  },
  header: {
    flexDirection: "row",
    alignItems: "flex-start",
    justifyContent: "space-between",
  },
  footer: {
    backgroundColor: "#fff",
    alignItems: "center",
    flexDirection: "row",
    justifyContent: "space-between",
    paddingVertical: 10,
  },
  flexBetween: {
    alignItems: "center",
    justifyContent: "space-between",
  },
  textMuted: {
    color: "#A0A0A1",
  },
  dotContainer: {
    flexDirection: "row",
    alignItems: "center",
    justifyContent: "center",
  },
});
