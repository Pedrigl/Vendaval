import { AvaliationMedia } from "./avaliationMedia";
import { ProductAvaliationStars } from "./productavaliationstars";

export interface ProductAvaliation {
  id: number;
  productId: number;
  costumerName: string;
  title: string;
  description: string;
  media: AvaliationMedia[] | null | undefined;
  stars: ProductAvaliationStars
}
