using Microsoft.AspNetCore.Authorization;

namespace Baron.Web
{
    [Authorize]
    public class SecureDbController : DbController
    {

    }
}
