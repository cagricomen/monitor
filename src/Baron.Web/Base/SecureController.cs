using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baron.Web
{
    [Authorize]
    public class SecureController : Controller
    {

    }
}
