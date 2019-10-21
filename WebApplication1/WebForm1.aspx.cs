using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1.Models;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CmdGenerar_Click(object sender, EventArgs e)
        {
            JobLogger log = new JobLogger(true,false,false,true,true,false);
            log.LogMessage("pepe", true, false, true);
           
        }
    }
}