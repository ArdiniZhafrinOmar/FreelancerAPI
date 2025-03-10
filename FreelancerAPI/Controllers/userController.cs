using FreelancerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    // Constructor with Dependency Injection
    private readonly ILogger<UserController> _logger;
    public UserController(AppDbContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }
  
    // Cache response for 60 seconds
    [ResponseCache(Duration = 60)] 
    // GET: api/users (Retrieve all users)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers(int page = 1, int pageSize = 10, string skillset = null)
    {

        if (pageSize < 1) pageSize = 1;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Users.AsQueryable(); 
        
        // Filter by skillset if provided
        if (!string.IsNullOrEmpty(skillset)) 
        {
            query = query.Where(u => u.Skillsets.Contains(skillset));
        }

        _logger.LogInformation("Fetching users - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var totalUsers = await query.CountAsync();
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Always return `Data`, even if empty
        if (users.Count == 0)
        {
            _logger.LogWarning("No users found");
            return Ok(new { TotalUsers = 0, Page = page, PageSize = pageSize, TotalPages = 0, Data = new List<User>() });
        }

        _logger.LogInformation("Returning {UserCount} users", users.Count);

        return Ok(new
        {
            TotalUsers = totalUsers,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize),
            Data = users
        });
    }

    // GET: api/users/{id} (Retrieve a single user by ID)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        _logger.LogInformation("Fetching user with ID: {UserId}", id);

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User with ID {UserId} not found!", id);
            throw new NotFoundException("User not found");
        }

        _logger.LogInformation("User {UserId} found!", id);
        return Ok(user);
    }

    // POST: api/users (Create a new user)
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User newUser)
    {
        if (newUser == null)
        {
            return BadRequest(new { message = "Invalid user data" });
        }

        _logger.LogInformation("Creating user: {Username}", newUser.Username);
       
        newUser.Id = 0;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User {Username} created successfully!", newUser.Username);

        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }

    // PUT: api/users/{id} (Update an existing user)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
        {
            return NotFound(new { message = "User not found" });
        }

        // Update existing user fields
        existingUser.Username = updatedUser.Username;
        existingUser.Email = updatedUser.Email;
        existingUser.Phone = updatedUser.Phone;
        existingUser.Skillsets = updatedUser.Skillsets;
        existingUser.Hobby = updatedUser.Hobby;

        // Save changes
        await _context.SaveChangesAsync();
        return NoContent(); // 204 - Success
    }

    // DELETE: api/users/{id} (Delete a user)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [Authorize] 
    
    // Require JWT token for access
    [HttpGet("secure-data")]
    public IActionResult GetSecureData()
    {
        return Ok(new { message = "This is a protected API endpoint!" });
    }

}
