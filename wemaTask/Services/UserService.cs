namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Authorization;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

public interface IUserService
{
    Response Authenticate(Login model);
    IEnumerable<User> GetAll();
    User GetById(int id);
    void Register(Register model);
    void Update(int id, UpdateUsers model);
    void Delete(int id);
}

public class UserService : IUserService
{
    private UserContext _context;
    private IJwtUtils _jwtUtils;
    private readonly IMapper _mapper;

    public UserService(
        UserContext context,
        IJwtUtils jwtUtils,
        IMapper mapper)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _mapper = mapper;
    }

    public Response Authenticate(Login model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == model.Email && x.PasswordHash == model.Password);

        // validate
        try
        {
            if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
                if (model.Password == model.Password)
                    throw new AppException("Email or password is incorrect");
        }
        catch (Exception)
        {

            throw;
        }

        // authentication successful
        var response = _mapper.Map<Response>(user);
        response.Token = _jwtUtils.GenerateToken(user);
        return response;
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(int id)
    {
        return getUser(id);
    }

    public void Register(Register model)
    {
        // validate
        if (_context.Users.Any(x => x.Email == model.Email))
            throw new AppException("Email Address '" + model.Email + "' is already taken");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(int id, UpdateUsers model)
    {
        try
        {
            var user = getUser(id);

            // validate
            if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
                throw new AppException("Email Address '" + model.Email + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void Delete(int id)
    {
        var user = getUser(id);
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    private User getUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User details not found");
        return user;
    }
}