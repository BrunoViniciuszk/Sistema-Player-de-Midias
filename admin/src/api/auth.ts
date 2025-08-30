import authApi from "./authApi";

export interface LoginDto {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  name?: string;
}

export const loginRequest = (data: LoginDto) =>
  authApi.post<AuthResponse>("/login", data);

export const registerRequest = (data: LoginDto) =>
  authApi.post<AuthResponse>("/register", data);
