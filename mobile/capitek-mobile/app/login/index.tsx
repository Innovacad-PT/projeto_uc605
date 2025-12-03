import { SafeAreaView } from "react-native-safe-area-context";
import { Text, Button, YStack, XStack, Label } from "tamagui";
import { TouchableOpacity } from "react-native";
import Toast from "react-native-toast-message";
import { Controller, useForm } from "react-hook-form";
import { useAuth } from "../../context/use_auth_context";
import { Link, useRouter } from "expo-router";
import { useState } from "react";
import { Feather } from "@expo/vector-icons";
import { Input } from "../../components/ui/Input";

type FormData = {
  email: string;
  password: string;
};

export default function Login() {
  const { signIn, token } = useAuth();
  const router = useRouter();
  const [showPassword, setShowPassword] = useState(false);
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormData>({
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      const success = await signIn(data.email, data.password);
      if (!success) {
        //Alert.alert("Erro", "Erro ao fazer login");
        return;
      }

      // Mostrar mensagem de sucesso nativo do android
      Toast.show({
        type: "success",
        text1: "Sucesso",
        text2: "Login realizado com sucesso",
      });
      router.replace("/home");
    } catch (error) {
      console.error("Login error:", error);
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: "white" }}>
      <YStack flex={1} padding="$4" justifyContent="center" space="$4">
        <YStack marginBottom="$6">
          <Text
            fontFamily="MontserratBold"
            fontSize={32}
            color="$blue10"
            textAlign="left"
          >
            Bem-vindo
          </Text>
          <Text
            fontFamily="MontserratBold"
            fontSize={32}
            color="$blue10"
            textAlign="left"
          >
            de volta!
          </Text>
          <Text
            fontFamily="Montserrat"
            fontSize={16}
            color="$gray10"
            marginTop="$2"
          >
            Preencha os dados para continuar.
          </Text>
        </YStack>

        <YStack space="$4">
          <Controller
            control={control}
            name="email"
            rules={{
              required: "Email é obrigatório",
              pattern: {
                value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                message: "Email inválido",
              },
            }}
            render={({ field: { onChange, onBlur, value } }) => (
              <YStack>
                <Label
                  fontFamily="MontserratBold"
                  fontSize={14}
                  color="$gray11"
                  marginBottom="$2"
                >
                  Email
                </Label>
                <Input
                  placeholder="exemplo@email.com"
                  value={value}
                  onChangeText={onChange}
                  onBlur={onBlur}
                  autoCapitalize="none"
                  keyboardType="email-address"
                  borderColor={errors.email ? "$red10" : "$gray5"}
                />
                {errors.email && (
                  <Text
                    color="$red10"
                    fontSize={12}
                    marginTop="$1"
                    fontFamily="Montserrat"
                  >
                    {errors.email.message}
                  </Text>
                )}
              </YStack>
            )}
          />

          <Controller
            control={control}
            name="password"
            rules={{ required: "Palavra-passe é obrigatória" }}
            render={({ field: { onChange, onBlur, value } }) => (
              <YStack>
                <Label
                  fontFamily="MontserratBold"
                  fontSize={14}
                  color="$gray11"
                  marginBottom="$2"
                >
                  Palavra-passe
                </Label>
                <Input
                  placeholder="********"
                  value={value}
                  onChangeText={onChange}
                  onBlur={onBlur}
                  secureTextEntry={!showPassword}
                  borderColor={errors.password ? "$red10" : "$gray5"}
                  suffix={
                    <TouchableOpacity
                      onPress={() => setShowPassword(!showPassword)}
                    >
                      <Feather
                        name={showPassword ? "eye" : "eye-off"}
                        size={20}
                        color="#666"
                      />
                    </TouchableOpacity>
                  }
                />
                {errors.password && (
                  <Text
                    color="$red10"
                    fontSize={12}
                    marginTop="$1"
                    fontFamily="Montserrat"
                  >
                    {errors.password.message}
                  </Text>
                )}
              </YStack>
            )}
          />

          <XStack justifyContent="flex-end">
            <Link href="/forgot-password" asChild>
              <Text
                fontFamily="Montserrat"
                fontSize={14}
                color="$blue10"
                fontWeight="600"
              >
                Esqueceu a palavra-passe?
              </Text>
            </Link>
          </XStack>

          <Button
            onPress={handleSubmit(onSubmit)}
            backgroundColor="#007AFF"
            pressStyle={{ opacity: 0.8, backgroundColor: "#0056b3" }}
            borderRadius="$4"
            height={50}
            marginTop="$2"
            disabled={isSubmitting}
          >
            <Text color="white" fontFamily="MontserratBold" fontSize={16}>
              {isSubmitting ? "A entrar..." : "Entrar"}
            </Text>
          </Button>
        </YStack>

        <XStack justifyContent="center" marginTop="$6" space="$2">
          <Text fontFamily="Montserrat" fontSize={14} color="$gray10">
            Não tem conta?
          </Text>
          <Link href="/signup" asChild>
            <Text fontFamily="MontserratBold" fontSize={14} color="$blue10">
              Registe-se
            </Text>
          </Link>
        </XStack>
      </YStack>
    </SafeAreaView>
  );
}
