using FreelancerAPI.Models;

public class UserService : IUserService
{
    private readonly List<User> users = new List<User>();

    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int id) => users.FirstOrDefault(u => u.Id == id);

    public void CreateUser(User user)
    {
        user.Id = users.Count == 0 ? 1 : users.Max(u => u.Id) + 1;
        users.Add(user);
    }

    public void UpdateUser(User updatedUser)
    {
        var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Skillsets = updatedUser.Skillsets;
            user.Hobby = updatedUser.Hobby;
        }
    }

    public void DeleteUser(int id)
    {
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            users.Remove(user);
        }
    }


}
