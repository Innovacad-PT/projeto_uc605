import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthEnum } from "@_types/auth";
import NotFound from "@components/notfound/NotFound";
import Auth from "@components/auth/Auth";
import Landing from "@components/landing";
import ProductPageWrapper from "@components/products/product/wrapper";
import { CartProvider } from "@services/cart";
import { AuthProvider } from "@contexts/AuthContext";
import { ProtectedRoute } from "@components/ProtectedRoute";
import CheckoutPage from "@components/checkout";
import LoginPage from "@pages/auth/Login";
import RegisterPage from "@pages/auth/Register";
import AdminDashboard from "@pages/admin/Dashboard";
import AdminProducts from "@pages/admin/Products";
import AdminBrands from "@pages/admin/Brands";
import AdminCategories from "@pages/admin/Categories";
import AdminDiscounts from "@pages/admin/Discounts";

const App = () => {
  return (
    <>
      <AuthProvider>
        <CartProvider>
          <BrowserRouter>
            <Routes>
              <Route path="/" element={<Landing />} />
              <Route path="/login" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
              <Route path="/product/:id" element={<ProductPageWrapper />} />

              <Route
                path="/auth/register"
                element={<Auth type={AuthEnum.REGISTER} />}
              />
              <Route path="/checkout" element={<CheckoutPage />} />

              {/* Admin routes - protected */}
              <Route
                path="/admin"
                element={
                  <ProtectedRoute requiredRole="admin">
                    <AdminDashboard />
                  </ProtectedRoute>
                }
              >
                <Route path="products" element={<AdminProducts />} />
                <Route path="brands" element={<AdminBrands />} />
                <Route path="categories" element={<AdminCategories />} />
                <Route path="discounts" element={<AdminDiscounts />} />
              </Route>

              <Route path="*" element={<NotFound />} />
            </Routes>
          </BrowserRouter>
        </CartProvider>
      </AuthProvider>
    </>
  );
};

export default App;
