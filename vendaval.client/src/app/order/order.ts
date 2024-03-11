import { Product } from "../product/product";
import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { OrderStatus } from "./orderStatus";
import { PaymentMethod } from "./paymentMethod";
import { Address } from "../shared/common/interfaces/address"; 
import { DeliveryType } from "./deliveryType";
export interface Order extends BaseModel{
  costumerId: number;
  products: Product[];
  total: number;
  installments: number;
  installmentsValue: number;
  status: OrderStatus;
  paymentDate: Date | null;
  paymentMethod: PaymentMethod;
  deliveryAddress: Address | null;
  deliveryType: DeliveryType;
  trackingCode: string | null;
  orderNotes: string | null;
}
