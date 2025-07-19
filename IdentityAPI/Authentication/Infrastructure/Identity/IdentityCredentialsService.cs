using IdentityAPI.Authentication.Domain.DataAccess;
using IdentityAPI.Authentication.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityAPI.Authentication.Infrastructure.Identity;

public class IdentityCredentialsService : CredentialsDataAccess
{
    private readonly SignInManager<ApplicationUser> signInManager;

    public IdentityCredentialsService(SignInManager<ApplicationUser> signInManager)
    {
        this.signInManager = signInManager;
    }

    public async Task<bool> ValidateCredentials(string username, string password)
    {
        SignInResult result = await signInManager.PasswordSignInAsync(username, password, false, false);

        return result.Succeeded;
    }
}