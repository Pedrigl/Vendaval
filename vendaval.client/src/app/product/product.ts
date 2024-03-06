import { ProductType } from "./product-type";

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  image: string;
  category: ProductType;
  avaliation: number
}
