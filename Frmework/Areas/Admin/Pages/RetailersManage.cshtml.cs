using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Frmework.Areas.Admin.Pages
{
    public class RetailersManageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RetailersManageModel> _logger;

        [BindProperty]
        public RetailerEditModel Retailer { get; set; } = new();

        public bool IsEdit => !string.IsNullOrEmpty(Retailer.CustomerId);

        public RetailersManageModel(IConfiguration configuration, ILogger<RetailersManageModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult OnGet(string id)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "1")
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Page("/Customers", new { area = "Admin" }) });

            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    string conStr = _configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection con = new SqlConnection(conStr))
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Customer_ID, FullName, Email, MobileNo FROM Customer_Master WHERE Customer_ID=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        using (var dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                Retailer.CustomerId = dr["Customer_ID"].ToString();
                                Retailer.FullName = dr["FullName"].ToString();
                                Retailer.Email = dr["Email"].ToString();
                                Retailer.Phone = dr["MobileNo"].ToString();
                                Retailer.Role = "Retailer"; // default - if DB has role column, prefer that
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading retailer {Id}", id);
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
                        cmd.CommandText = "UPDATE Customer_Master SET FullName=@name, Email=@email, MobileNo=@phone WHERE Customer_ID=@id";
                        cmd.Parameters.AddWithValue("@id", Retailer.CustomerId);
                    }
                    else
                    {
                        Retailer.CustomerId = Guid.NewGuid().ToString();
                        cmd.CommandText = "INSERT INTO Customer_Master (Customer_ID, FullName, Email, MobileNo) VALUES (@id,@name,@email,@phone)";
                        cmd.Parameters.AddWithValue("@id", Retailer.CustomerId);
                    }

                    cmd.Parameters.AddWithValue("@name", Retailer.FullName ?? "");
                    cmd.Parameters.AddWithValue("@email", Retailer.Email ?? "");
                    cmd.Parameters.AddWithValue("@phone", Retailer.Phone ?? "");

                    cmd.ExecuteNonQuery();

                    // If Customer_Master has a Role/Type column, set it to the selected Role (best-effort)
                    try {
                        using (var cmd2 = new SqlCommand("IF COL_LENGTH('Customer_Master','Role') IS NOT NULL UPDATE Customer_Master SET Role=@role WHERE Customer_ID=@id", con))
                        {
                            cmd2.Parameters.AddWithValue("@role", Retailer.Role ?? "Retailer");
                            cmd2.Parameters.AddWithValue("@id", Retailer.CustomerId);
                            cmd2.ExecuteNonQuery();
                        }
                    } catch {}
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving retailer");
                ModelState.AddModelError(string.Empty, "Unable to save retailer. See logs.");
                return Page();
            }

            return RedirectToPage("./Customers");
        }

        public class RetailerEditModel
        {
            public string CustomerId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Role { get; set; }
        }
    }
}
