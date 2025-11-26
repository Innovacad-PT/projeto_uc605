export const getPriceAsNumber = (price: string | number): number => {
  if (typeof price === 'number') return price;
  if (!price) return 0;
  // Handle "1.000,00" or "10,00" formats if necessary, but for now simple replace comma with dot
  // Assuming format is either "123.45" or "123,45"
  return Number(price.replace(',', '.'));
};

export const calculateDiscountedPrice = (price: string | number, discountPercentage: number): number => {
  const originalPrice = getPriceAsNumber(price);
  const discountAmount = originalPrice * (discountPercentage / 100);
  return originalPrice - discountAmount;
};

export const formatPrice = (price: number): string => {
  return price.toFixed(2);
};

export const getFinalPrice = (price: string | number, discountPercentage?: number): number => {
  const originalPrice = getPriceAsNumber(price);
  if (discountPercentage) {
    return calculateDiscountedPrice(price, discountPercentage);
  }
  return originalPrice;
};
