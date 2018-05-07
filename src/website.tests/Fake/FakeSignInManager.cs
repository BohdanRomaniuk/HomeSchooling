using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using database.Models;
using Moq;

namespace website.tests.Fake
{
    public class FakeSignInManager : SignInManager<User>
    {
        //public SignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes);
        public FakeSignInManager()
            : base(
                new FakeUserManager(),
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<User>>>().Object,
                            null)
        //new Mock<AuthenticationSchemeProvider>(
        //    MockBehavior.Default, 
        //    new Mock<IOptions<AuthenticationOptions>>().Object).Object)
        { }
    }
}
