using Frmework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Frmework.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"]?.ToString();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Index(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            // Accept firstName/lastName from the signup form. Build a display name for the session.
            var name = (!string.IsNullOrWhiteSpace(firstName) ? firstName.Trim() : "") +
                       (!string.IsNullOrWhiteSpace(lastName) ? " " + lastName.Trim() : "");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Please fill all required fields.";
                return RedirectToAction("Index");
            }

            if (password != confirmPassword)
            {
                TempData["Error"] = "Passwords do not match.";
                return RedirectToAction("Index");
            }

            // For demo purposes we create a session id for the new user. Replace with DB create user when available.
            var custId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("CustId", custId);
            HttpContext.Session.SetString("CustName", name);
            HttpContext.Session.SetString("Email", email);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password, string returnUrl = null)
        {
            _logger?.LogInformation("Login POST attempt for email={Email}", email);
            string conStr = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("SP_Products_Master", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@flag", "Login");
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@MobileNo", password);
                con.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // Store customer information in session
                        var custId = dr["Customer_ID"].ToString();
                        HttpContext.Session.SetString("CustId", custId);
                        HttpContext.Session.SetString("CustName", dr["FullName"].ToString());
                        HttpContext.Session.SetString("Email", dr["Email"].ToString());

                        _logger?.LogInformation("Login successful for CustomerId={CustomerId}", custId);

                        // If returnUrl provided and local, redirect; otherwise redirect to previous requested URL stored in session
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        var pendingReturn = HttpContext.Session.GetString("PendingReturnUrl");
                        if (!string.IsNullOrEmpty(pendingReturn) && Url.IsLocalUrl(pendingReturn))
                        {
                            HttpContext.Session.Remove("PendingReturnUrl");
                            return Redirect(pendingReturn);
                        }

                        // Redirect authenticated users to the Checkout action on CheckoutController by default
                        return RedirectToAction("Checkout", "Checkout");
                    }
                }
            }

            ViewBag.Error = "Invalid Email or Password";
            return View();
        }
    }
}
