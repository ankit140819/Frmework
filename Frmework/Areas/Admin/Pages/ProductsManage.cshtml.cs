using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Frmework.Areas.Admin.Pages
{
    public class ProductsManageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsManageModel> _logger;

        [BindProperty]
        public ProductEditModel Product { get; set; } = new();

        public bool IsEdit => !string.IsNullOrEmpty(Product.ProductId);

        public ProductsManageModel(IConfiguration configuration, ILogger<ProductsManageModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnGet(string id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "1")
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Page("/ProductsManage", new { area = "Admin" }) });

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    string conStr = _configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection con = new SqlConnection(conStr))
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Product_Id, Product_Name, Product_Amount, Stock, Short_Description FROM Products_Master WHERE Product_Id = @id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Product.ProductId = dr["Product_Id"].ToString();
                                Product.ProductName = dr["Product_Name"].ToString();
                                Product.Price = Convert.ToDecimal(dr["Product_Amount"]);
                                Product.Stock = dr["Stock"] != DBNull.Value ? Convert.ToInt32(dr["Stock"]) : 0;
                                Product.ShortDescription = dr["Short_Description"]?.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading product {Id}", id);
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                string conStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conStr))
                using (SqlCommand cmd = new SqlCommand("", con))
                {
                    con.Open();
                    if (IsEdit)
                    {
                        cmd.CommandText = "UPDATE Products_Master SET Product_Name=@name, Product_Amount=@amt, Stock=@stock, Short_Description=@short WHERE Product_Id=@id";
                        cmd.Parameters.AddWithValue("@id", Product.ProductId);
                    }
                    else
                    {
                        // Create a new product id if not provided
                        if (string.IsNullOrWhiteSpace(Product.ProductId))
                            Product.ProductId = "PRD-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();

                        cmd.CommandText = "INSERT INTO Products_Master (Product_Id, Product_Name, Product_Amount, Stock, Short_Description) VALUES (@id,@name,@amt,@stock,@short)";
                        cmd.Parameters.AddWithValue("@id", Product.ProductId);
                    }

                    cmd.Parameters.AddWithValue("@name", Product.ProductName ?? "");
                    cmd.Parameters.AddWithValue("@amt", Product.Price);
                    cmd.Parameters.AddWithValue("@stock", Product.Stock);
                    cmd.Parameters.AddWithValue("@short", Product.ShortDescription ?? "");

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving product");
                ModelState.AddModelError(string.Empty, "Unable to save product. See logs.");
                return Page();
            }

            return RedirectToPage("./Products");
        }

        public class ProductEditModel
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string ShortDescription { get; set; }
        }
    }
}
