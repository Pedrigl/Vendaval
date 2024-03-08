export interface CreatePreAuthenticatedRequest {
  accessType: number,
  accessUri: string,
  bucketListingAction: number,
  fullPath: string,
  id: number,
  name: string,
  timeCreated: Date,
  timeExpires: Date,
}
