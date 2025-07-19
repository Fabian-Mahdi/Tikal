using IdentityAPI.Authentication.Domain.Models;
using IdentityAPI.Authentication.Infrastructure.Entities;

namespace IdentityAPI.Authentication.Infrastructure.Mappers.Interfaces;

public interface IUserMapper
{
    Task<User> FromEntity(ApplicationUser entity);
}