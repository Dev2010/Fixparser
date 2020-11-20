namespace fix_analyzer.Controllers
{
    using fix_analyzer;
    using fix_analyzer.Models;
    using log4net;
    using System.Reflection;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            FixMessageLoad model = new FixMessageLoad();
            return base.View(model);
        }

        [HttpPost]
        public ActionResult Parse(FixMessageLoad fixMessageLoad)
        {
            fixMessageLoad = new FixProcessing().ProcessFixMessageLoad(fixMessageLoad);
            return base.View("Index", fixMessageLoad);
        }

        public ActionResult Sample()
        {
            FixMessageLoad fixMessageLoad = new FixMessageLoad {
                FIXMessages = "2011-11-24 19:46:17,586 INFO  out.SENDER_RECV - >187 OrderSingle (8=FIX.4.2\x00019=261\x000135=D\x000149=SENDER\x000156=RECV\x000134=187\x000152=20111125-00:46:17\x0001128=FBSI\x000150=SMITH\x000143=Y\x0001122=20111124-00:06:34\x000111=1234571217\x000121=1\x0001100=USA\x000155=ACME\x000148=8970023\x000122=2\x0001207=NYS\x0001106=ACME Corp\x0001107=ACME Corp\x000154=5\x0001114=N\x000160=20111124-00:06:26\x000138=1900000\x000140=1\x000115=USD\x000159=0\x000110=183\x0001)"
            };
            return this.Parse(fixMessageLoad);
        }
    }
}

