using AutoMapper;
using DS.Domain.Entities;
using DS.Users.Application.Commands.CreateUser;
using DS.Users.Application.DTOs;

namespace DS.Users.Application.Mapping
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, UserDto>();
        }
    }
}
