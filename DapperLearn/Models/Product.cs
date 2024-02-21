namespace DapperLearn.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public List<Order> Orders { get; set; }
    }
}
