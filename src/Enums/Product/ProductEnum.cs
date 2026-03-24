namespace PulpaAPI.src.Enums.Product
{
    public enum CategoryEnum
    {
        TropicalFruits,
        ExoticFruits,
        Mix
    }

    public enum WeightEnum
    {
        Grams100 = 100,
        Grams500 = 500,
        Kg1 = 1000
    }

    public enum PaymentMethodEnum
    {
        Cash,
        CreditCard,
        DebitCard,
        Pix,
        Mixed
    }

    public enum SaleStatusEnum
    {
        Pending,
        Completed,
        Cancelled
    }
}
