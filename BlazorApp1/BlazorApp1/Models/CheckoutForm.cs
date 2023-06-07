using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models
{
    public class CheckoutForm
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "We need the address to deliver the product.")]
        public string Adress { get; set; } ="";


    }
}
