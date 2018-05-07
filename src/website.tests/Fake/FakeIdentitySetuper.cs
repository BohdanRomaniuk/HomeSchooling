using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Moq;
using database.Models;

namespace website.tests.Fake
{
    public static class FakeIdentitySetuper
    {
        public static void Setup(out Mock<Fake.FakeUserManager> userManager, out Mock<Fake.FakeSignInManager> signInManager,
            IQueryable<User> usersCollection)
        {
            userManager = new Mock<FakeUserManager>();
            userManager.Setup(s => s.Users).Returns(usersCollection);

            userManager.Setup(x => x.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var uservalidator = new Mock<IUserValidator<User>>();
            uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<User>>(), It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);
            var passwordvalidator = new Mock<IPasswordValidator<User>>();
            passwordvalidator.Setup(x =>
                    x.ValidateAsync(It.IsAny<UserManager<User>>(), It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            signInManager = new Mock<FakeSignInManager>();

            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
        }
    }
}