import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { Adress } from "../shared/common/interfaces/address";
import { UserType } from "./user-type";

export interface User extends BaseModel{
  userType: UserType;
  email: string;
  password: string;
  name: string;
  birthDate: Date;
  phoneNumber: string;
  address: Adress[] | undefined;
}
