using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Identity.Infrastructure.Entities;

namespace IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;

public interface IUserMapper
{
    Task<User> FromEntity(ApplicationUser entity);
}