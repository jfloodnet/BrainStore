using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Messages;
using Microsoft.AspNet.SignalR;

namespace IEventSourcedMyBrain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }

    public class LiveController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }    
}
