import { BaseModel } from "../../shared/common/interfaces/BaseModel";
import { AvaliationMedia } from "./avaliationMedia";
import { ProductAvaliationStars } from "./productavaliationstars";

export interface ProductAvaliation extends BaseModel{
  productId: number;
  costumerName: string;
  title: string;
  description: string;
  media: AvaliationMedia[] | null | undefined;
  stars: ProductAvaliationStars
}
