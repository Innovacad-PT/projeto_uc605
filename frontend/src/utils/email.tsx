import ReactDOMServer from "react-dom/server";
import { ReceiptTemplateHtml } from "@components/email/ReceiptTemplate";
import type { OrderResult } from "@services/order";
import type { CartItem } from "@contexts/CartContext";
import { LogType } from "@_types/debug";
import { logger } from "@utils/debug";

export const generateEmailHtml = (
  order: OrderResult | { id: string; total: number; createdAt: string },
  items: CartItem[]
): string => {
  return ReactDOMServer.renderToStaticMarkup(
    <ReceiptTemplateHtml order={order} items={items} />
  );
};

export const sendOrderEmail = async (
  order: OrderResult | { id: string; total: number; createdAt: string },
  items: CartItem[]
) => {
  try {
    const emailHtml = generateEmailHtml(order, items);

    logger(LogType.INFO, "📧 MOCK EMAIL SENT 📧");
    logger(LogType.INFO, "Subject: Order Receipt #" + order.id);
    console.log(emailHtml);
    console.log(order);
    console.log(items);

    logger(
      LogType.INFO,
      "Content",
      "Copy the console output above into a .html file to preview the email."
    );
  } catch (error) {
    logger(LogType.ERROR, "Failed to generate/send email", String(error));
  }
};
