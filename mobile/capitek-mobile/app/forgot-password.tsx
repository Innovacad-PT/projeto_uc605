import { SafeAreaView } from "react-native-safe-area-context";
import { Text, Button, YStack, Input, Label } from "tamagui";
import { Controller, useForm } from "react-hook-form";
import { Link, useRouter } from "expo-router";

type FormData = {
  email: string;
};

export default function ForgotPassword() {
  const router = useRouter();
  const {
    control,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<FormData>({
    defaultValues: {
      email: "",
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      console.log("Forgot Password data:", data);
      // Here you would call your password recovery API
      // await resetPassword(data.email);
      // router.back(); // or show success message
    } catch (error) {
      console.error("Forgot Password error:", error);
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
            Recuperar
          </Text>
          <Text
            fontFamily="MontserratBold"
            fontSize={32}
            color="$blue10"
            textAlign="left"
          >
            Palavra-passe
          </Text>
          <Text
            fontFamily="Montserrat"
            fontSize={16}
            color="$gray10"
            marginTop="$2"
          >
            Insira o seu email para receber as instruções.
          </Text>
        </YStack>

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
                backgroundColor="$gray2"
                borderColor={errors.email ? "$red10" : "$gray5"}
                borderWidth={1}
                borderRadius="$4"
                paddingHorizontal="$3"
                height={50}
                fontFamily="Montserrat"
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
            {isSubmitting ? "A enviar..." : "Recuperar Palavra-passe"}
          </Text>
        </Button>

        <YStack alignItems="center" marginTop="$4">
          <Link href="/login" asChild>
            <Text fontFamily="MontserratBold" fontSize={14} color="$blue10">
              Voltar ao Login
            </Text>
          </Link>
        </YStack>
      </YStack>
    </SafeAreaView>
  );
}
