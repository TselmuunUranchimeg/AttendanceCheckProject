using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class UserDbContext : IdentityDbContext<UserModel>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
}
