import { notifications } from "@mantine/notifications";
import {
  IconAlertSquareFilled,
  IconSquareCheckFilled,
} from "@tabler/icons-react";

export const Notify = (title: string, message: string, type: string) => {
  notifications.show({
    autoClose: 5000,
    withCloseButton: true,
    position: "top-right",
    color: "transparent",
    title: title,
    message,
    icon:
      type == "error" ? (
        <IconAlertSquareFilled color="red" />
      ) : (
        <IconSquareCheckFilled color="lime" />
      ),
  });
};
