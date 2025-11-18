import { Box } from "@mantine/core";

export default function HorizontalScroll({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <Box
      style={{
        display: "flex",
        gap: "16px",
        overflowX: "auto",
        paddingBottom: "10px",
        scrollSnapType: "x mandatory",
      }}
    >
      {children}
    </Box>
  );
}
