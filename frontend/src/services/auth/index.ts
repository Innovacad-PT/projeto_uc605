import { USERS_LOGIN_USER_URL } from "@utils/api";
import axios, { HttpStatusCode, type AxiosResponse } from "axios";

export const LoginUser = async (user: string, password: string, type: string) => {
  let returnValue: string | null = null;

  await axios
    .post(
      USERS_LOGIN_USER_URL,
      {
        user,
        password,
        type,
      },
      {
        headers: {
          "Access-Control-Allow-Origin": "*",
        },
      }
    )
    .then((res: AxiosResponse) => {
      if (res.status == HttpStatusCode.Ok) {
        returnValue = res.data.value.token;
      }
    });

  return returnValue;
};

export const RegisterUser = () => {};
