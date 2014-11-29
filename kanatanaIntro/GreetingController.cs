using System.Web.Http;

namespace kanatanaIntro
{
    public class GreetingController :ApiController
    {
        public Greeting Get()
        {
            return new Greeting {Text = "Hello...what are you doing here"};
        }
    }
}
