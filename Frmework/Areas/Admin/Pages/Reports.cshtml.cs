using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Frmework.Areas.Admin.Pages
{
    public class ReportsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ReportsModel> _logger;

        public string LabelsJson { get; set; } = "[]";
        public string DataJson { get; set; } = "[]";

        public ReportsModel(IConfiguration configuration, ILogger<ReportsModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");
            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "1")
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Page("/Reports", new { area = "Admin" }) });
            }

            try
            {
                // simple synthetic data: last 7 days counts from Order_Master
                var labels = new List<string>();
                var data = new List<int>();
                string conStr = _configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    await con.OpenAsync();
                    for (int i = 6; i >= 0; i--)
                    {
                        var day = DateTime.Today.AddDays(-i);
                        using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Order_Master WHERE CAST(Order_Date AS DATE) = @d", con))
                        {
                            cmd.Parameters.AddWithValue("@d", day.Date);
                            var cnt = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
                            labels.Add(day.ToString("MM-dd"));
                            data.Add(cnt);
                        }
                    }
                }

                LabelsJson = System.Text.Json.JsonSerializer.Serialize(labels);
                DataJson = System.Text.Json.JsonSerializer.Serialize(data);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to build reports data; showing empty chart");
            }

            return Page();
        }
    }
}
