using System;
using Microsoft.EntityFrameworkCore;

namespace UserHub.Infrastructure.Data;

public class UserHubDbContext : DbContext
{
    public UserHubDbContext(DbContextOptions<UserHubDbContext> options) : base(options)
    {
        
    }
}