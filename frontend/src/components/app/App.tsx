import { BrowserRouter, Route, Routes } from "react-router-dom";
import NotFound from "@components/notfound/NotFound";
import Landing from "@components/landing";
import ProductPageWrapper from "@components/products/product/wrapper";
import { CartProvider, type CartItem } from "@contexts/CartContext";
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
import AdminUsers from "@pages/admin/Users";
import AdminTechSpecs from "@pages/admin/TechSpecs";
import ProductsListPage from "@pages/ProductsList";
import Contact from "@components/contact";
import OrderConfirmation from "@components/order_confirmation";
import ProfilePage from "@pages/profile";
import OrdersPage from "@pages/orders";
import { ReceiptTemplateHtml } from "@components/email/ReceiptTemplate";

const App = () => {
  const order = {
    id: "67652207",
    userId: "41e294d7-2400-4e43-a33d-feb0652bc4c9",
    createdAt: "2025-12-11T19:26:13.5533016+00:00",
    total: 509.97,
    status: "PENDING",
    orderItems: [
      {
        id: 0,
        productId: "7bd2718c-5e2c-48b0-8b99-04907b43e614",
        quantity: 3,
        unitPrice: 169.99,
      },
    ],
  };

  const items: CartItem[] = [
    {
      product: {
        id: "7bd2718c-5e2c-48b0-8b99-04907b43e614",
        name: "Portatil i9",
        description: "",
        price: "169.99",
        stock: 3,
        category: {
          id: "40c9354a-1002-425d-a561-45895910ad86",
          name: "Computador",
        },
        brand: {
          id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          name: "Lenovo",
        },
        imageUrl: "",
        technicalSpecs: [
          {
            id: "7bd2718c-5e2c-48b0-8b99-04907b43e614",
            technicalSpecsId: "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d",
            name: "Processor",
            value: "i9-14700K",
          },
        ],
        reviews: [],
        createdAt: "2025-12-11T19:25:55.8999824Z",
        updatedAt: "2025-12-11T19:25:55.9000399Z",
        discount: undefined,
      },
      quantity: 3,
    },
  ];

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
              {/*<Route
                path="/email-template"
                element={
                  <ReceiptTemplateHtml
                    order={order}
                    items={items}
                    userName="Pedro Guerra"
                  />
                }
              />*/}

              <Route
                path="/checkout"
                element={
                  <ProtectedRoute>
                    <CheckoutPage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/order-confirmation"
                element={
                  <ProtectedRoute>
                    <OrderConfirmation />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/profile"
                element={
                  <ProtectedRoute>
                    <ProfilePage />
                  </ProtectedRoute>
                }
              />
              <Route
                path="/orders"
                element={
                  <ProtectedRoute>
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
                <Route path="users" element={<AdminUsers />} />
                <Route path="tech-specs" element={<AdminTechSpecs />} />
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
