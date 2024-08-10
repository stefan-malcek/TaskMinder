namespace Backend.Application.Common.Models.Identity;

public record UserViewModel(Guid Id, string Email, bool EmailConfirmed);
