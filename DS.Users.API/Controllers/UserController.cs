using DS.Users.Application.UseCases.CreateUser;
using DS.Users.Application.UseCases.DeleteUser;
using DS.Users.Application.UseCases.GetUser;
using DS.Users.Application.UseCases.UpdateUser;
using DS.Users.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DS.Users.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ICreateUserUseCase _createUserUseCase;
    private readonly IGetUserByIdUseCase _getUserByIdUseCase;
    private readonly IUpdateUserUseCase _updateUserUseCase;
    private readonly IDeleteUserUseCase _deleteUserUseCase;

    public UserController(
        ICreateUserUseCase createUserUseCase,
        IGetUserByIdUseCase getUserByIdUseCase,
        IUpdateUserUseCase updateUserUseCase,
        IDeleteUserUseCase deleteUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
        _getUserByIdUseCase = getUserByIdUseCase;      
        _updateUserUseCase = updateUserUseCase;
        _deleteUserUseCase = deleteUserUseCase;
    }

    [HttpPost(Name = "CreateUser")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync(
        [FromBody] ICreateUserUseCase.CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createUserUseCase.CreateUserAsync(request, cancellationToken);
        var routeValues = new { id = result.Value };
        return result.IsSuccess
            ? CreatedAtAction("GetUserByIdAsync", routeValues, result.Value)
            : BadRequest(new ProblemDetails
            {
                Title = result.Error.Code,
                Detail = result.Error.Message,
                Status = StatusCodes.Status400BadRequest
            });
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getUserByIdUseCase.GetUserByIdAsync(id, cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new ProblemDetails
            {
                Title = result.Error.Code,
                Detail = result.Error.Message,
                Status = StatusCodes.Status404NotFound
            });
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync(Guid id,
        [FromBody] IUpdateUserUseCase.UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest("ID mismatch");

        var result = await _updateUserUseCase.UpdateUserAsync(request, cancellationToken);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteUserUseCase.DeleteUserAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(result.Error);
    }
}
