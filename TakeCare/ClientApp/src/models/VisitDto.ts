export class VisitDto {
  public id: number = 0;
  public doctorEmail: string = "";
  public slot: string = "";
  public patientEmail: string = "";
  public reason: string = "";
  public doctorFirstName: string = "";
  public doctorLastName: string = "";
  public doctorSpecialization: string = "";
  public isVisitExecuted: boolean = false;
  public patientFirstName: string = "";
  public patientLastName: string = "";
  public patientPesel?: string;
}
