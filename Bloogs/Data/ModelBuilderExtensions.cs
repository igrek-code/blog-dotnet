﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bloogs.Entities;

namespace Bloogs.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            var pwd = "AlphaDuSantor14*";
            var passwordHasher = new PasswordHasher<User>();

            // Seed Roles
            var adminRole = new IdentityRole("Admin");
            adminRole.NormalizedName = adminRole.Name.ToUpper();

            var memberRole = new IdentityRole("User");
            memberRole.NormalizedName = memberRole.Name.ToUpper();

            var supervisorRole = new IdentityRole("Supervisor");
            supervisorRole.NormalizedName = supervisorRole.Name.ToUpper();

            List<IdentityRole> roles = new List<IdentityRole>() {
            adminRole,
            memberRole,
            supervisorRole
            };

            builder.Entity<IdentityRole>().HasData(roles);


            // Seed Users
            var supervisorUser = new User
            {
                UserName = "supervisor@blog.fr",
                Email = "supervisor@blog.fr",
                EmailConfirmed = true,
                FirstName = "Supervisor",
                LastName = "Supervisor",
            };
            supervisorUser.NormalizedUserName = supervisorUser.UserName.ToUpper();
            supervisorUser.NormalizedEmail = supervisorUser.Email.ToUpper();
            supervisorUser.PasswordHash = passwordHasher.HashPassword(supervisorUser, pwd);

            List<User> users = new List<User>() {
                supervisorUser
            };

            builder.Entity<User>().HasData(users);

            // Seed UserRoles
            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>();

            userRoles.Add(new IdentityUserRole<string>
            {
                UserId = users[0].Id,
                RoleId = roles.First(q => q.Name == "Supervisor").Id
            });



            builder.Entity<IdentityUserRole<string>>().HasData(userRoles);

        }
    }

}