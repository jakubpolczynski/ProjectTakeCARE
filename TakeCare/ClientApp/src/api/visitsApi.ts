import { apiClient } from "./client";
import { FindDateDto } from "@/models/FindDateDto";
import { VisitDto } from "@/models/VisitDto";


export const findAvailableVisits = async (data: FindDateDto) => {
  return await apiClient.post("/Visit/FindAvailableVisits", data);
};

export const bookVisit = async (data: VisitDto) => {
  return await apiClient.post("/Visit/BookVisit", data);
};

export const bookVisitAsReceptionist = async (data: VisitDto) => {
  return await apiClient.post("/Visit/BookVisitAsReceptionist", data);
};

export const getPatientVisits = async (patientEmail: string) => {
  return await apiClient.get("/Visit/GetPatientVisits", { params: { patientEmail } });
};

export const getDoctorVisits = async (doctorEmail: string) => {
  return await apiClient.get("/Visit/GetDoctorVisits", { params: { doctorEmail } });
};

export const deleteBookedVisit = async (data: VisitDto) => {
  return await apiClient.post(`/Visit/DeleteBookedVisit`, data);
};

export const getVisit = async (visitId: number) => {
  return await apiClient.get(`/Visit/GetVisit?id=${visitId}`);
};

export const getPatientExecutedVisits = async (patientEmail: string) => {
  return await apiClient.get("/Visit/GetPatientExecutedVisits", { params: { patientEmail } });
};

export const getDoctorExecutedVisits = async (doctorEmail: string) => {
  return await apiClient.get("/Visit/GetDoctorExecutedVisits", { params: { doctorEmail } });
};
