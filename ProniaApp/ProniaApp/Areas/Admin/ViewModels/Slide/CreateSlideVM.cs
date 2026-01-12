namespace ProniaApp.Areas.Admin.ViewModels.Slide
{
    public class CreateSlideVM
    {
        public string Title { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; }
    }
}
