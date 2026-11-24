using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Frmework.Areas.Admin.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProductsModel> _logger;

        public List<dynamic> Products { get; set; } = new List<dynamic>();

        public ProductsModel(IConfiguration configuration, ILogger<ProductsModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "1")
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Page("/Products", new { area = "Admin" }) });
            }

            try
            {
                string conStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conStr))
                using (SqlCommand cmd = new SqlCommand("SELECT Product_Id, Product_Name, Product_Amount FROM Products_Master", con))
                {
                    await con.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Products.Add(new {
                                ProductId = reader["Product_Id"].ToString(),
                                ProductName = reader["Product_Name"].ToString(),
                                Price = reader["Product_Amount"]?.ToString() ?? ""
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading products for admin");
            }

            return Page();
        }
    }
}
