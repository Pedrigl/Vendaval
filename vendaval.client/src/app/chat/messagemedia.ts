import { Mediatype } from "../shared/common/enums/mediatype";
import { BaseModel } from "../shared/common/interfaces/BaseModel";

export interface MessageMedia extends BaseModel {
  messageId: number;
  url: string;
  mediaType: Mediatype;
}
