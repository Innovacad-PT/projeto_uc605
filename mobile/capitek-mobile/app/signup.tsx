import { SafeAreaView } from "react-native-safe-area-context";
import { Text, Button, YStack, XStack, Label, ScrollView } from "tamagui";
import { TouchableOpacity } from "react-native";
import { Controller, useForm } from "react-hook-form";
import { Link, useRouter } from "expo-router";
import { useState } from "react";
import { Feather } from "@expo/vector-icons";
import { Input } from "../components/ui/Input";

type FormData = {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
};

export default function Signup() {
  const router = useRouter();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const {
    control,
    handleSubmit,
    watch,
    formState: { errors, isSubmitting },
  } = useForm<FormData>({
    defaultValues: {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  const password = watch("password");

  const onSubmit = async (data: FormData) => {
    try {
      console.log("Signup data:", data);
      // Here you would call your registration API
      // await signUp(data);
      // router.replace("/(tabs)"); // or navigate to login
    } catch (error) {
      console.error("Signup error:", error);
    }
  };

  return (
    <SafeAreaView style={{ flex: 1, backgroundColor: "white" }}>
      <ScrollView
        contentContainerStyle={{
          flexGrow: 1,
          justifyContent: "center",
          padding: 16,
        }}
      >
        <YStack space="$4">
          <YStack marginBottom="$6">
            <Text
              fontFamily="MontserratBold"
              fontSize={32}
              color="$blue10"
              textAlign="left"
            >
              Crie a sua
            </Text>
            <Text
              fontFamily="MontserratBold"
              fontSize={32}
              color="$blue10"
              textAlign="left"
            >
              conta!
            </Text>
            <Text
              fontFamily="Montserrat"
              fontSize={16}
              color="$gray10"
              marginTop="$2"
            >
              Preencha os dados para começar.
            </Text>
          </YStack>

          <Controller
            control={control}
            name="name"
            rules={{ required: "Nome é obrigatório" }}
            render={({ field: { onChange, onBlur, value } }) => (
              <YStack>
                <Label
                  fontFamily="MontserratBold"
                  fontSize={14}
                  color="$gray11"
                  marginBottom="$2"
                >
                  Nome Completo
                </Label>
                <Input
                  placeholder="Seu nome"
                  value={value}
                  onChangeText={onChange}
                  onBlur={onBlur}
                  borderColor={errors.name ? "$red10" : "$gray5"}
                />
                {errors.name && (
                  <Text
                    color="$red10"
                    fontSize={12}
                    marginTop="$1"
                    fontFamily="Montserrat"
                  >
                    {errors.name.message}
                  </Text>
                )}
              </YStack>
            )}
          />

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
            rules={{
              required: "Palavra-passe é obrigatória",
              minLength: {
                value: 6,
                message: "A palavra-passe deve ter pelo menos 6 caracteres",
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

          <Controller
            control={control}
            name="confirmPassword"
            rules={{
              required: "Confirmação de palavra-passe é obrigatória",
              validate: (value) =>
                value === password || "As palavras-passe não coincidem",
            }}
            render={({ field: { onChange, onBlur, value } }) => (
              <YStack>
                <Label
                  fontFamily="MontserratBold"
                  fontSize={14}
                  color="$gray11"
                  marginBottom="$2"
                >
                  Confirmar Palavra-passe
                </Label>
                <Input
                  placeholder="********"
                  value={value}
                  onChangeText={onChange}
                  onBlur={onBlur}
                  secureTextEntry={!showConfirmPassword}
                  borderColor={errors.confirmPassword ? "$red10" : "$gray5"}
                  suffix={
                    <TouchableOpacity
                      onPress={() =>
                        setShowConfirmPassword(!showConfirmPassword)
                      }
                    >
                      <Feather
                        name={showConfirmPassword ? "eye" : "eye-off"}
                        size={20}
                        color="#666"
                      />
                    </TouchableOpacity>
                  }
                />
                {errors.confirmPassword && (
                  <Text
                    color="$red10"
                    fontSize={12}
                    marginTop="$1"
                    fontFamily="Montserrat"
                  >
                    {errors.confirmPassword.message}
                  </Text>
                )}
              </YStack>
            )}
          />

          <Button
            onPress={handleSubmit(onSubmit)}
            backgroundColor="#007AFF"
            pressStyle={{ opacity: 0.8, backgroundColor: "#0056b3" }}
            borderRadius="$4"
            height={50}
            marginTop="$4"
            disabled={isSubmitting}
          >
            <Text color="white" fontFamily="MontserratBold" fontSize={16}>
              {isSubmitting ? "A criar conta..." : "Registar"}
            </Text>
          </Button>

          <XStack
            justifyContent="center"
            marginTop="$4"
            space="$2"
            marginBottom="$4"
          >
            <Text fontFamily="Montserrat" fontSize={14} color="$gray10">
              Já tem conta?
            </Text>
            <Link href="/login" asChild>
              <Text fontFamily="MontserratBold" fontSize={14} color="$blue10">
                Entrar
              </Text>
            </Link>
          </XStack>
        </YStack>
      </ScrollView>
    </SafeAreaView>
  );
}
