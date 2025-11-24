// src/utils/api.ts

export const BASE_URL = import.meta.env.VITE_API_URL || "";

/**
 * API response wrapper from backend
 */
export interface ApiResponse<T> {
  hasError: boolean;
  message: string;
  value: T;
  code: string;
}

/**
 * Get stored JWT token from localStorage.
 */
function getToken(): string | null {
  return localStorage.getItem("accessToken");
}

/**
 * Generic request helper.
 */
async function request<T>(
  method: string,
  endpoint: string,
  body?: any,
  queryParams?: Record<string, string | number | undefined>
): Promise<T> {
  // Build the URL
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

  const headers: HeadersInit = {
    "Content-Type": "application/json",
  };
  const token = getToken();
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const response = await fetch(url, {
    method,
    headers,
    body: body ? JSON.stringify(body) : undefined,
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`API error ${response.status}: ${errorText}`);
  }

  // If no content (204), return undefined as any
  if (response.status === 204) {
    return undefined as any;
  }

  // Parse the response wrapper
  let apiResponse: ApiResponse<T>;
  try {
    const responseText = await response.text();
    apiResponse = JSON.parse(responseText) as ApiResponse<T>;
  } catch (parseError) {
    console.error('Failed to parse API response:', parseError);
    throw new Error(`Invalid JSON response from API. Response was not in expected format.`);
  }
  
  // Check if the API returned an error
  if (apiResponse.hasError) {
    throw new Error(`API Error [${apiResponse.code}]: ${apiResponse.message}`);
  }
  
  // Return the unwrapped value
  return apiResponse.value;
}

export const apiClient = {
  get: <T>(endpoint: string, queryParams?: Record<string, string | number | undefined>) =>
    request<T>("GET", endpoint, undefined, queryParams),
  post: <T>(endpoint: string, body: any) => request<T>("POST", endpoint, body),
  put: <T>(endpoint: string, body: any) => request<T>("PUT", endpoint, body),
  delete: <T>(endpoint: string) => request<T>("DELETE", endpoint),
};
