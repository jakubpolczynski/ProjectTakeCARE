import { DoctorDto } from "@/models/DoctorDto";
import { PatientDto } from "@/models/PatientDto";
import { ReceptionistDto } from "@/models/ReceptionistDto";
import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5184/api",
  headers: {
    Accept: "application/json",
    "Content-Type": "application/json",
    "Access-Control-Allow-Origin": "*",
  },
});

export const addDoctor = async (data: DoctorDto) => {
  return await apiClient.post("/User/AddDoctor", data);
};

export const addPatient = async (data: PatientDto) => {
  return await apiClient.post("/User/AddPatient", data);
};

export const addReceptionist = async (data: ReceptionistDto) => {
  return await apiClient.post("/User/AddReceptionist", data);
};
