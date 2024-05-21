import { DoctorDto } from "@/models/DoctorDto";
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

export const addDoctor = async (data: DoctorDto) => {
  await apiClient.post("/User/AddDoctor", data);
};

export const addPatient = async (data: PatientDto) => {
  await apiClient.post("/User/AddPatient", data);
};

export const deleteUser = async (id: string) => {};

export const getUser = async (data: any) => {};

export const updatePatient = async (data: any) => {};

export const updateDoctor = async (data: any) => {};
