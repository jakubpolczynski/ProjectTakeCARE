import axios from "axios";
import { FindDateDto } from "@/models/FindDateDto";
import { VisitDto } from "@/models/VisitDto";

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

export const bookVisit = async (data: VisitDto) => {
  return await apiClient.post("/Visit/BookVisit", data);
};

export const getPatientVisits = async (patientEmail: string) => {
  return await apiClient.get(`/Visit/GetPatientVisits?patientEmail=${patientEmail}`);
};

export const getDoctorVisits = async (doctorEmail: string) => {
  return await apiClient.get(`/Visit/GetDoctorVisits?doctorEmail=${doctorEmail}`);
};

export const deleteBookedVisit = async (data: VisitDto) => {
  return await apiClient.post(`/Visit/DeleteBookedVisit`, data);
};

export const getVisit = async (visitId: number) => {
  return await apiClient.get(`/Visit/GetVisit?id=${visitId}`);
};
