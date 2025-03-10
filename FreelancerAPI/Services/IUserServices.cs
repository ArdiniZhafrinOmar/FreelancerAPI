using FreelancerAPI.Models;

public interface IUserService
{
    List<User> GetUsers();
    User GetUserById(int id);
    void CreateUser(User user);
    void UpdateUser(User user);
    void DeleteUser(int id);
}
