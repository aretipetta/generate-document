using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateDocument.WebDataConnector
{
    class CustomResponseFromFBDB
    {

        private bool ok;
        private Object responseBody;

        public CustomResponseFromFBDB() { }

        public CustomResponseFromFBDB(bool statusOK, Object responseBody)
        {
            this.ok = statusOK;
            this.responseBody = responseBody;
        }

        public bool OK
        {
            get { return ok; }
            set { ok = value; }
        }

        public Object ResponseBody
        {
            get { return responseBody; }
            set { responseBody = value; }
        }
    }
}
