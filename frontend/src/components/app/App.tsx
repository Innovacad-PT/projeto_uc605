import { BrowserRouter, Route, Routes } from "react-router-dom";
import NotFound from "@components/notfound/NotFound";
import Landing from "@components/landing";
import ProductPageWrapper from "@components/products/product/wrapper";
import { CartProvider } from "@contexts/CartContext";
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
import ProductsListPage from "@pages/ProductsList";
import Contact from "@components/contact";
import OrderConfirmation from "@components/order_confirmation";
import ProfilePage from "@pages/profile";
import OrdersPage from "@pages/orders";

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
              <Route path="/products" element={<ProductsListPage />} />

              <Route
                path="/checkout"
                element={
                  <ProtectedRoute requiredRole="guest">
                    <CheckoutPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/order-confirmation"
                element={
                  <ProtectedRoute requiredRole="guest">
                    <OrderConfirmation />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/profile"
                element={
                  <ProtectedRoute requiredRole="guest">
                    <ProfilePage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/orders"
                element={
                  <ProtectedRoute requiredRole="guest">
                    <OrdersPage />
                  </ProtectedRoute>
                }
              />

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
                <Route path="contacts" element={<Contact />} />
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
