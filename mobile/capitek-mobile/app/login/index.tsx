import { SafeAreaView } from "react-native-safe-area-context";
import { Text, View, Button } from "tamagui";
import { StyleSheet, TextInput } from "react-native";
import { Controller, useForm } from "react-hook-form";
import { useAuth } from "../../context/use_auth_context";

type FormData = {
  email: string;
  password: string;
};

export default function Login() {
  const { signIn } = useAuth();
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const onSubmit = async (data: FormData) => {
    await signIn(data.email, data.password);
  };

  return (
    <SafeAreaView style={[styles.container, { padding: 16 }]}>
      <View style={styles.titleContainer}>
        <Text style={styles.title}>Bem-vindo de volta!</Text>
      </View>

      <View style={styles.container}>
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
            <View style={styles.inputWrapper}>
              <Text style={styles.inputLabel}>Email</Text>
              <TextInput
                style={styles.input}
                placeholder="Email"
                value={value}
                onChangeText={onChange}
                onBlur={onBlur}
                autoCapitalize="none"
              />
              {errors.email && (
                <Text style={styles.errorText}>{errors.email.message}</Text>
              )}
            </View>
          )}
        />

        <Controller
          control={control}
          name="password"
          rules={{ required: "Palavra-passe é obrigatória" }}
          render={({ field: { onChange, onBlur, value } }) => (
            <View style={styles.inputWrapper}>
              <Text style={styles.inputLabel}>Palavra-passe</Text>
              <TextInput
                style={styles.input}
                placeholder="Palavra-passe"
                value={value}
                onChangeText={onChange}
                onBlur={onBlur}
                secureTextEntry
              />
              {errors.password && (
                <Text style={styles.errorText}>{errors.password.message}</Text>
              )}
            </View>
          )}
        />

        <Button
          onPress={handleSubmit(onSubmit)}
          width="100%"
          backgroundColor="#007AFF"
          opacity={1}
          pressStyle={{ opacity: 0.8, backgroundColor: "#0056b3" }}
        >
          <Text color="white" fontWeight="bold">
            Entrar
          </Text>
        </Button>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    width: "100%",
  },
  inputWrapper: {
    width: "100%",
    marginBottom: 24,
  },
  titleContainer: {
    display: "flex",
    flexDirection: "row",
    width: "100%",
    justifyContent: "flex-start",
    alignItems: "flex-start",
    marginBottom: 24,
  },
  title: {
    fontFamily: "Montserrat",
    fontSize: 48,
    fontWeight: "bold",
    textAlign: "left",
    maxWidth: 250,
  },
  input: {
    borderWidth: 1,
    borderColor: "#A8A8A9",
    borderRadius: 5,
    padding: 10,
    marginBottom: 5,
    width: "100%",
  },
  inputLabel: {
    fontFamily: "Montserrat",
    fontSize: 16,
    fontWeight: "bold",
    marginBottom: 8,
  },
  errorText: {
    color: "red",
    fontSize: 12,
    fontFamily: "Montserrat",
  },
});
