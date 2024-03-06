export enum OrderStatus {
  waitingForPayment = 1,
  payed = 2,
  inPreparation = 3,
  shipped = 4,
  outForDelivery = 5,
  delivered = 6,
  cancelRequested = 7,
  canceled = 8,
  waitingForReturnMail = 9,
  returned = 10,
  refundRequested = 11,
  refunded = 12
}
