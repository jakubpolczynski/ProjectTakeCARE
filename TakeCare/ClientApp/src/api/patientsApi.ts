import { PatientDto } from "@/models/PatientDto";
import { apiClient } from "./client";


export const getDoctorPatients = async (doctorEmail: string) => {
  return await apiClient.get("/Patient/GetDoctorPatients", { params: { email: doctorEmail } });
};
