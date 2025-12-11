import type { CartItem } from "@contexts/CartContext";
import type { OrderResult } from "@services/order";

interface ReceiptTemplateProps {
  order: OrderResult | { id: string; total: number; createdAt: string };
  items: CartItem[];
  userName?: string;
}

const COLORS = {
  primaryBlue: "#228be6",
  secondaryCyan: "#15aabf",
  backgroundLight: "#f8f9fa",
  backgroundCard: "#ffffff",
  textDimmed: "#868e96",
  textStrong: "#343a40",
};

const FONT_SIZES = {
  xs: "12px",
  sm: "14px",
  lg: "18px",
  xl: "20px",
};

const SPACING = {
  xs: "4px",
  sm: "8px",
  md: "10px",
  lg: "16px",
  xl: "20px",
};

export const ReceiptTemplateHtml = ({
  order,
  items,
  userName = "Valued Customer",
}: ReceiptTemplateProps) => {
  const formattedDate = new Date(
    order.createdAt || Date.now()
  ).toLocaleDateString("pt-BR");
  const subTotal = items.reduce(
    (sum, item) => sum + Number(item.product.price) * item.quantity,
    0
  );

  return (
    <table
      role="presentation"
      width="100%"
      cellPadding={0}
      cellSpacing={0}
      style={{
        fontFamily: "'Inter', sans-serif, Arial, sans-serif",
        backgroundColor: COLORS.backgroundLight,
        padding: "40px 20px",
        margin: "0 auto",
      }}
    >
      <tr>
        <td align="center">
          <table
            role="presentation"
            width="100%"
            cellPadding={SPACING.lg}
            cellSpacing={0}
            style={{
              backgroundColor: COLORS.backgroundCard,
              maxWidth: "600px",
              margin: "0 auto",
              borderRadius: "6px",
              border: "1px solid #dee2e6",
              boxShadow: "0 1px 3px rgba(0,0,0,0.05)",
            }}
          >
            <tr>
              <td style={{ textAlign: "center", paddingBottom: SPACING.lg }}>
                <h1
                  style={{
                    fontSize: FONT_SIZES.xl,
                    fontWeight: 900,
                    margin: 0,
                    color: COLORS.primaryBlue,
                    lineHeight: 1.2,
                  }}
                >
                  CAPITEK
                </h1>
                <p
                  style={{
                    fontSize: FONT_SIZES.lg,
                    fontWeight: 600,
                    color: COLORS.textDimmed,
                    margin: `${SPACING.xs} 0 0 0`,
                  }}
                >
                  Fatura para o Pedido #{order.id}
                </p>
              </td>
            </tr>

            <tr>
              <td style={{ padding: `${SPACING.sm} 0` }}>
                <hr
                  style={{
                    border: 0,
                    borderTop: `1px solid #e9ecef`,
                    margin: 0,
                  }}
                />
              </td>
            </tr>

            <tr>
              <td
                style={{ paddingBottom: SPACING.lg, color: COLORS.textStrong }}
              >
                <p style={{ margin: `0 0 ${SPACING.md} 0` }}>Olá {userName}!</p>
                <p
                  style={{
                    fontSize: FONT_SIZES.sm,
                    lineHeight: 1.5,
                    color: COLORS.textDimmed,
                    margin: 0,
                  }}
                >
                  Obrigado pela sua compra! Estamos a preparar a sua encomenda
                  para envio.
                  <br /> Abaixo segue um resumo da sua encomenda feita a{" "}
                  <span style={{ fontWeight: 600 }}>{formattedDate}</span>.
                </p>
              </td>
            </tr>

            <tr>
              <td style={{ padding: 0 }}>
                <table
                  role="presentation"
                  width="100%"
                  cellPadding={0}
                  cellSpacing={0}
                  style={{ marginBottom: SPACING.xl, padding: "16px" }}
                >
                  <thead>
                    <tr>
                      <th
                        style={{
                          textAlign: "left",
                          paddingBottom: SPACING.sm,
                          color: COLORS.textDimmed,
                          fontSize: FONT_SIZES.sm,
                          borderBottom: `1px solid #e9ecef`,
                        }}
                      >
                        Produto
                      </th>
                      <th
                        style={{
                          textAlign: "right",
                          paddingBottom: SPACING.sm,
                          color: COLORS.textDimmed,
                          fontSize: FONT_SIZES.sm,
                          borderBottom: `1px solid #e9ecef`,
                        }}
                      >
                        Qnt
                      </th>
                      <th
                        style={{
                          textAlign: "right",
                          paddingBottom: SPACING.sm,
                          color: COLORS.textDimmed,
                          fontSize: FONT_SIZES.sm,
                          borderBottom: `1px solid #e9ecef`,
                        }}
                      >
                        Preço
                      </th>
                    </tr>
                  </thead>
                  <tbody>
                    {items.map((item) => (
                      <tr
                        key={item.product.id}
                        style={{ borderBottom: `1px solid #e9ecef` }}
                      >
                        <td
                          style={{
                            paddingTop: SPACING.sm,
                            paddingBottom: SPACING.sm,
                            fontSize: FONT_SIZES.sm,
                            fontWeight: 500,
                          }}
                        >
                          {item.product.name}
                        </td>
                        <td
                          style={{
                            textAlign: "right",
                            paddingTop: SPACING.sm,
                            paddingBottom: SPACING.sm,
                            fontSize: FONT_SIZES.sm,
                          }}
                        >
                          {item.quantity}
                        </td>
                        <td
                          style={{
                            textAlign: "right",
                            paddingTop: SPACING.sm,
                            paddingBottom: SPACING.sm,
                            fontSize: FONT_SIZES.sm,
                          }}
                        >
                          €{Number(item.product.price).toFixed(2)}
                        </td>
                      </tr>
                    ))}
                    <tr>
                      <td
                        colSpan={2}
                        style={{
                          textAlign: "right",
                          paddingTop: SPACING.sm,
                          fontSize: FONT_SIZES.sm,
                          color: COLORS.textDimmed,
                        }}
                      >
                        Subtotal:
                      </td>
                      <td
                        style={{
                          textAlign: "right",
                          paddingTop: SPACING.sm,
                          fontSize: FONT_SIZES.sm,
                          color: COLORS.textDimmed,
                        }}
                      >
                        €{subTotal.toFixed(2)}
                      </td>
                    </tr>
                  </tbody>
                </table>
              </td>
            </tr>

            <tr>
              <td
                style={{
                  padding: `${SPACING.sm} 0`,
                }}
              >
                <hr
                  style={{
                    border: 0,
                    borderTop: `1px solid #e9ecef`,
                    margin: 0,
                  }}
                />
              </td>
            </tr>

            <tr>
              <td style={{ paddingTop: SPACING.md, paddingBottom: SPACING.md }}>
                <table
                  width="100%"
                  role="presentation"
                  cellPadding={0}
                  cellSpacing={0}
                >
                  <tr>
                    <td
                      style={{
                        fontSize: FONT_SIZES.lg,
                        fontWeight: 700,
                        color: COLORS.textStrong,
                      }}
                    >
                      Total
                    </td>
                    <td
                      style={{
                        textAlign: "right",
                        fontSize: FONT_SIZES.xl,
                        fontWeight: 700,
                        color: COLORS.primaryBlue,
                      }}
                    >
                      €{Number(order.total).toFixed(2)}
                    </td>
                  </tr>
                </table>
              </td>
            </tr>

            <tr>
              <td style={{ padding: `${SPACING.xl} 0` }}>
                <hr
                  style={{
                    border: 0,
                    borderTop: `1px solid #e9ecef`,
                    margin: 0,
                  }}
                />
              </td>
            </tr>

            <tr>
              <td style={{ textAlign: "center" }}>
                <p
                  style={{
                    fontSize: FONT_SIZES.xs,
                    color: COLORS.textDimmed,
                    margin: `0 0 ${SPACING.xs} 0`,
                  }}
                >
                  Se tiver alguma dúvida, contacte-nos no email
                  support@capitek.com
                </p>
                <p
                  style={{
                    fontSize: FONT_SIZES.xs,
                    color: COLORS.textDimmed,
                    margin: 0,
                  }}
                >
                  © {new Date().getFullYear()} Capitek. Todos os direitos
                  reservados.
                </p>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  );
};
