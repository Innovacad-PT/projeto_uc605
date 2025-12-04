import {
  Input as TamaguiInput,
  InputProps as TamaguiInputProps,
  XStack,
  styled,
} from "tamagui";
import { forwardRef } from "react";
import { TextInput } from "react-native";

export type InputProps = TamaguiInputProps & {
  suffix?: React.ReactNode;
};

const StyledInput = styled(TamaguiInput, {
  flex: 1,
  unstyled: true,
  fontFamily: "Montserrat",
});

export const Input = forwardRef<TextInput, InputProps>(
  ({ suffix, ...props }, ref) => {
    return (
      <XStack
        alignItems="center"
        backgroundColor="$gray4"
        borderColor={props.borderColor || "$gray5"}
        borderWidth={1}
        borderRadius="$4"
        paddingHorizontal="$2"
        height={50}
      >
        <StyledInput ref={ref} {...props} color={"$gray10"} />
        {suffix}
      </XStack>
    );
  }
);
