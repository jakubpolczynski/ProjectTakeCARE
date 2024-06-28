import { PatientDto } from "@/models/PatientDto";
import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5184/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

export const getDoctorPatients = async (doctorEmial: string) => {
  return await apiClient.get(`/Patient/GetDoctorPatients?email=${doctorEmial}`);
};
