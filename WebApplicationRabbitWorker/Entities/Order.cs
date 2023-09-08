namespace WebApplicationRabbitWorker.Entities
{
    public class Order
    {
        public Order()
        {

        }

        public Order(int orderNumber, string itemName, double price)
        {
            OrderNumber = orderNumber;
            ItemName = itemName;
            Price = price;
        }

        public int OrderNumber { get; private set; }
        public string ItemName { get; private set; }
        public double Price { get; private set; }
    }
}
