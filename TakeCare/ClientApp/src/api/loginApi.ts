import { LoginDto } from "@/models/LoginDto";
import { apiClient } from "./client";


export const loginUser = async (data: LoginDto) => {
  return await apiClient.post("/Login/Login", data);
};

export const getUser = async () => {
  return await apiClient.get("/Login/GetUser");
};

export const logoutUser = async () => {
  return await apiClient.post("/Login/Logout");
};
