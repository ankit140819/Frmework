namespace Frmework.Models
{
    public class Youth
    {
       public string Product_Id { get; set; }

        public string Product_Name { get; set; }

        public decimal Product_Amount { get;set; }

        public string Short_Description { get; set; }

        public string Full_Description { get; set; }

        // Skin type(s) the product is suited for (e.g. Dry, Oily, Combination, Sensitive, Normal)
        public string SkinType { get; set; }

    }
}
