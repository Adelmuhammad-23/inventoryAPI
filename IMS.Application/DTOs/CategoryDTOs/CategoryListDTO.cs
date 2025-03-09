namespace IMS.Application.DTOs.CategoryDTOs
{
    public class CategoryListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Productslist> Productslist { get; set; }
    }
    public class Productslist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
