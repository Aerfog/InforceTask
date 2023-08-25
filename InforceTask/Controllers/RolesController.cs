using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InforceTask.Controllers;

//[Route("[controller]")]
public class RolesController : Controller
{
    private const string AdminRole = "Administrators";
    private const string UserEmail = "test@example.com";
    private readonly ILogger<RolesController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;
    
    public RolesController(ILogger<RolesController> logger, RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        if (!await _roleManager.RoleExistsAsync(AdminRole))
        {
            await _roleManager.CreateAsync(new IdentityRole(AdminRole));
        }
        
        var user = await _userManager.FindByEmailAsync(UserEmail);
        
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = UserEmail,
                Email = UserEmail
            };
            var result = await _userManager.CreateAsync(
                user, "Pa$$w0rd");
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} created successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
            }
        }
        
        if (!user.EmailConfirmed)
        {
            var token = await _userManager
                .GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager
                .ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} email confirmed successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
            }
        }
        
        if (!await _userManager.IsInRoleAsync(user, AdminRole))
        {
            var result = await _userManager
                .AddToRoleAsync(user, AdminRole);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} added to {AdminRole} successfully.");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
            }
        }
        
        return Redirect("/");
    }
}