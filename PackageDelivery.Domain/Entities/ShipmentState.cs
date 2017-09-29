namespace PackageDelivery.Domain.Entities {
    public enum ShipmentState {
        ReadyToStartDelivery,
        ReadyToStartPickup,
        WaitingForManagerApprovalToStartDelivery,
        WaitingForManagerApprovalToStartPickUp,
        InDelivery,
        InPickup,
        PickupDone,
        DeliveryDone
    }
}
