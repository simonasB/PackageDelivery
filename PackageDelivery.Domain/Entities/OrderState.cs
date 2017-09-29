namespace PackageDelivery.Domain.Entities {
    public enum OrderState {
        ReadyToBeDelivered,
        ReadyToBePickedUp,
        WaitingForManagerApprovalToBeDelivered,
        WaitingForManagerApprovalToBePickedUp,
        OnPickingUp,
        OnDelivery,
        PickedUp,
        Delivered,
    }
}
