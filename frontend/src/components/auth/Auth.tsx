import { AuthEnum, type AuthProps } from "@_types/auth";
import Login from "@components/login/Login";
import Register from "@components/register/Register";

const Auth = ({ type }: AuthProps) => {
  return <>{type === AuthEnum.REGISTER ? <Register /> : <Login />}</>;
};

export default Auth;
