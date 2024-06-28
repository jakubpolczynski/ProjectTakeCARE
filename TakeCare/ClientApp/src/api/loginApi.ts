import { LoginDto } from "@/models/LoginDto";
import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5184/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
  withCredentials: true,
});

export const loginUser = async (data: LoginDto) => {
  return await apiClient.post("/Login/Login", data);
};

export const getUser = async () => {
  return await apiClient.get("/Login/GetUser");
};

export const logoutUser = async () => {
  return await apiClient.post("/Login/Logout");
};
