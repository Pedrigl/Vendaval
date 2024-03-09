import { BaseModel } from "../shared/common/interfaces/BaseModel";
import { ProductType } from "./product-type";

export interface Product extends BaseModel{
  name: string;
  description: string;
  price: number;
  stock: number;
  image: string;
  category: ProductType;
  avaliation: number
}
