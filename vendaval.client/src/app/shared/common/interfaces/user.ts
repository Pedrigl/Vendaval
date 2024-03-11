import { BaseModel } from "./BaseModel";
import { Address } from "./address";
import { UserType } from "../enums/user-type";

export interface User extends BaseModel{
  userType: UserType;
  email: string;
  password: string;
  name: string;
  birthDate: Date;
  phoneNumber: string;
}
