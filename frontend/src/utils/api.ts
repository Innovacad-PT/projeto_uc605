export const BASE_URL = import.meta.env.VITE_API_URL || "";

export interface ApiResponse<T> {
  hasError: boolean;
  message: string;
  value: T;
  code: string;
}

function getToken(): string | null {
  return localStorage.getItem("accessToken");
}

async function request<T>(
  method: string,
  endpoint: string,
  body?: any,
  queryParams?: Record<string, string | number | undefined>
): Promise<T> {
  let url: string;

  const urlObj = new URL(BASE_URL + endpoint);
  if (queryParams) {
    Object.entries(queryParams).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        urlObj.searchParams.append(key, String(value));
      }
    });
  }
  url = urlObj.toString();

  const headers: HeadersInit = {};

  const token = getToken();
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  let requestBody: BodyInit | undefined;

  if (
    method === "PUT" &&
    body &&
    typeof body === "object" &&
    "imageUrl" in body
  ) {
    const formData = new FormData();
    Object.entries(body).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        formData.append(key, value as string | Blob);
      }
    });
    requestBody = formData;
  } else {
    headers["Content-Type"] = "application/json";
    requestBody = body ? JSON.stringify(body) : undefined;
  }

  const response = await fetch(url, {
    method,
    headers,
    body: requestBody,
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`API error ${response.status}: ${errorText}`);
  }

  if (response.status === 204) {
    return undefined as any;
  }

  let apiResponse: ApiResponse<T>;
  try {
    const responseText = await response.text();
    apiResponse = JSON.parse(responseText) as ApiResponse<T>;
  } catch (parseError) {
    throw new Error(
      `Invalid JSON response from API. Response was not in expected format.`
    );
  }

  console.log(apiResponse);

  if (apiResponse.hasError) {
    throw new Error(`API Error [${apiResponse.code}]: ${apiResponse.message}`);
  }

  return apiResponse.value;
}

export const apiClient = {
  get: <T>(
    endpoint: string,
    queryParams?: Record<string, string | number | undefined>
  ) => request<T>("GET", endpoint, undefined, queryParams),
  post: <T>(endpoint: string, body: any) => request<T>("POST", endpoint, body),
  put: <T>(endpoint: string, body: any) => request<T>("PUT", endpoint, body),
  delete: <T>(endpoint: string) => request<T>("DELETE", endpoint),
};
