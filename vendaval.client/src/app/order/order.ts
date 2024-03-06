import { Product } from "../product/product";
import { OrderStatus } from "./orderStatus";

export interface Order {
  id: number;
  userId: number;
  products: Product[];
  total: number;
  orderDate: Date;
  status: OrderStatus;
  statusDate: Date;
}
