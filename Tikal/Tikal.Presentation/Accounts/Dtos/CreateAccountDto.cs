namespace Tikal.Presentation.Accounts.Dtos;

/// <summary>
///     Holds all data needed for a create account operation
/// </summary>
/// <param name="Name">The name of the account to be created</param>
public record CreateAccountDto(string Name);