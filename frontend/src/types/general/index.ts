export type ApiResponse<T> = {
  hasError: boolean;
  value: T[];
  message: string;
  code: string;
};
