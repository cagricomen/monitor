using Baron.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Baron.Web
{
    public class DbController : Controller
    {
        private BContext _db;
        public BContext Db => _db ?? (BContext)HttpContext?.RequestServices.GetService(typeof(BContext));
    }
}
