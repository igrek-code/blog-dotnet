using Bloogs.Entities;
using Microsoft.AspNetCore.Identity;

namespace Bloogs.Data
{
    public class UserService: IUserService
    {
        private readonly SignInManager<User> _signInManager;
        public UserService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<User> SuperVisorAccountSeed()
        {
            var password = "admin";

            // Seed Users
            // var supervisorUser = new User
            // {
            //     //Email = "supervisor@blog.fr",
            //     EmailConfirmed = true,
            //     FirstName = "Supervisor2",
            //     LastName = ""
            // };
            var supervisorUser = await _signInManager.UserManager.FindByEmailAsync("supervisor@blog.fr");
            await _signInManager.UserManager.SetUserNameAsync(supervisorUser, "supervisor@blog.fr");
            await _signInManager.UserManager.SetEmailAsync(supervisorUser, "supervisor@blog.fr");
           
            
            //var result = await _signInManager.UserManager.CreateAsync(supervisorUser, password);

            //await _signInManager.UserManager.AddToRoleAsync(supervisorUser, "Supervisor");

            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(supervisorUser);

            await _signInManager.UserManager.ConfirmEmailAsync(supervisorUser, token);

            return supervisorUser;
        }
    }
}
