import { Product } from "../product/product";
import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { OrderStatus } from "./orderStatus";

export interface Order extends BaseModel{
  costumerId: number;
  products: Product[];
  total: number;
  status: OrderStatus;
}
