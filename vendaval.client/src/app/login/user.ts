import { Adress } from "./adress";
import { UserType } from "./user-type";

export interface User {
  id: number;
  userType: UserType;
  email: string;
  password: string;
  name: string;
  birthDate: Date;
  phoneNumber: string;
  address: Adress[] | undefined;
}
