import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { resolve } from "path";

// https://vite.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      "@_types": resolve(process.cwd(), "src/types"),
      "@components": resolve(process.cwd(), "src/components"),
      "@services": resolve(process.cwd(), "src/services"),
      "@utils": resolve(process.cwd(), "src/utils"),
    },
  },
  build: { outDir: "build" },
  plugins: [react()],
  server: {
    host: "0.0.0.0",
    port: 80,
  },
});
