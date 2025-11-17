import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthEnum } from "@_types/auth";
import NotFound from "@components/notfound/NotFound";
import Auth from "@components/auth/Auth";

const App = () => {
  return (
    <>
      <BrowserRouter>
        <Routes>
          <Route path="/auth/login" element={<Auth type={AuthEnum.LOGIN} />} />
          <Route
            path="/auth/register"
            element={<Auth type={AuthEnum.REGISTER} />}
          />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </BrowserRouter>
    </>
  );
};

export default App;
