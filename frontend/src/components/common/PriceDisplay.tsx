import { Text, type TextProps } from "@mantine/core";
import { calculateDiscountedPrice, formatPrice, getPriceAsNumber } from "@utils/price";
import type { Discount } from "@_types/product";

interface PriceDisplayProps extends TextProps {
  price: string | number;
  discount?: Discount;
}

export function PriceDisplay({ price, discount, ...textProps }: PriceDisplayProps) {
  const originalPrice = getPriceAsNumber(price);

  if (discount) {
    const discountedPrice = calculateDiscountedPrice(price, discount.percentage);
    return (
      <Text {...textProps}>
        <span style={{ textDecoration: "line-through", marginRight: "6px", opacity: 0.6, fontSize: "0.6em" }}>
          €{formatPrice(originalPrice)}
        </span>
        €{formatPrice(discountedPrice)}
      </Text>
    );
  }

  return (
    <Text {...textProps}>
      €{formatPrice(originalPrice)}
    </Text>
  );
}
