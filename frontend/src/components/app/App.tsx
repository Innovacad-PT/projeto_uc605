import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthEnum } from "@_types/auth";
import NotFound from "@components/notfound/NotFound";
import Auth from "@components/auth/Auth";
import Landing from "@components/landing";
import ProductPageWrapper from "@components/products/product/wrapper";
import { CartProvider } from "@services/cart";
import CheckoutPage from "@components/checkout";

const App = () => {
  return (
    <>
      <CartProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Landing />} />
            <Route
              path="/auth/login"
              element={<Auth type={AuthEnum.LOGIN} />}
            />
            <Route path="/product/:id" element={<ProductPageWrapper />} />

            <Route
              path="/auth/register"
              element={<Auth type={AuthEnum.REGISTER} />}
            />
            <Route path="/checkout" element={<CheckoutPage />} />

            <Route path="*" element={<NotFound />} />
          </Routes>
        </BrowserRouter>
      </CartProvider>
    </>
  );
};

export default App;
