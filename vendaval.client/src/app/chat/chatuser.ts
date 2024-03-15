import { UserType } from "../shared/common/enums/user-type";
import { BaseModel } from "../shared/common/interfaces/BaseModel";

export interface ChatUser extends BaseModel {
  connectionId: string;
  name: string;
  email: string;
  userType: UserType;
  isOnline: boolean;
}
