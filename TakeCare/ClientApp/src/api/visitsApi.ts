import axios from "axios";
import { FindDateDto } from "@/models/FindDateDto";

const apiClient = axios.create({
  baseURL: "http://localhost:5184/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

export const findAvailableVisits = async (data: FindDateDto) => {
  return await apiClient.post("/Visit/FindAvailableVisits", data);
};
