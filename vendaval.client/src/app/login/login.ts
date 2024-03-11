import { TimeSpan } from "../shared/time-span/time-span";
import { User } from "../shared/common/interfaces/user";

export interface Login {
  email: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  token: string;
  tokenExpiration: TimeSpan;
  user: User;
}
