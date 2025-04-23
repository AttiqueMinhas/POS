using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using POS.Data.Models.ModelVM.Request;
using POS.Data.Repositories.Account;
using POS.Helper;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace POS.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Logout Action
        [HttpPost]
        [ValidateAntiForgeryToken] // To prevent CSRF attacks
        public async Task<IActionResult> Logout()
        {
            // Clear the session (optional)
            HttpContext.Session.Clear();

            // Sign the user out
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page after logout
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request, string returnUrl = null)
        {
            LoginResponse response = await _accountRepository.GetUserByEmailOrUserNameAsync(request.UserNameOrEmail);

            if (response == null)
            {
                // Handle case where user is not found
                ViewBag.emailMessage = "Invalid login attempt. Please check your email or username.";
                return View();
            }

            bool isPasswordMatched = VerifyPassword(request.Password, response.Password, response.Salt);

            if (!isPasswordMatched)
            {
                ViewBag.passwordMessage = "Wrong Password. Check your Password and try Again!";
                return View();
            }

            bool isPasswordMathed = VerifyPassword(request.Password, response.Password, response.Salt);
            if (response == null)
            {
                // Handle case where user is not found
                // Example: Add a model error or redirect to a login failed view
                ViewBag.emailMessage = "Invalid login attempt. Please check your email or username.";
                return View();
            }
            else if (!isPasswordMathed)
            {
                ViewBag.passwordMessage = "Wrong Password\",\"Check your Password and try Again!";
                return View();
            }

            // Get user roles from repository
            var userRoles = await _accountRepository.GetUserRolesAsync(response.UserID); // You'll need to implement this

            // Store the login response in the session
            HttpContext.Session.SetObjectAsJson("LoginResponse", response);
            // Create user claims and sign in
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, response.UserName),
                new Claim(ClaimTypes.Email, response.Email),
                new Claim("UserID", response.UserID.ToString()) // Add UserID to claims
            };

            // Add all roles as claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Check if the returnUrl is valid
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }
            // Successful login logic here (e.g., create claims, sign in user, etc.)
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Private Methods
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }



        private bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
        {
            // Convert stored hash from hexadecimal string to byte array
            byte[] storedHashBytes = Convert.FromHexString(storedHash);

            // Generate hash from entered password using stored salt and other parameters
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(enteredPassword),
                storedSalt,
                iterations,
                hashAlgorithm,
                keySize);

            // Compare the generated hash with the stored hash
            return hash.SequenceEqual(storedHashBytes);
        }

        #endregion
    }
}
