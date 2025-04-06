namespace DS.Users.Application.DTOs
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime CreatedAt,
        DateTime LastModified
        );
}
