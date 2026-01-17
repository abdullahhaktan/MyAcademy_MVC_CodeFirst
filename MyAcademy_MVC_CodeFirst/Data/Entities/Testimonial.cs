namespace MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Job { get; set; }
        public int StarCount { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}