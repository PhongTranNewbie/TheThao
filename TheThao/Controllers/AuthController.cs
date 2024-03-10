using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;

namespace TheThao.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult SignInWithGoogle()
        {
            // Redirect to the Google authentication handler
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Auth", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult GoogleResponse()
        {
            // Handle Google authentication response
            return RedirectToAction("Client", "Login"); // Redirect to the desired page after successful authentication
        }
    }
}
