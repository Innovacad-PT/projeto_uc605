import "@mantine/core/styles.css";
import "@mantine/notifications/styles.css";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "@components/app/App";
import { MantineProvider } from "@mantine/core";
import { Notifications } from "@mantine/notifications";
import { CookiesProvider } from "react-cookie";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <CookiesProvider>
      <MantineProvider defaultColorScheme="auto">
        <Notifications />
        <App />
      </MantineProvider>
    </CookiesProvider>
  </React.StrictMode>
);
