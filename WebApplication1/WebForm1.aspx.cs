using System;
using WebApplication1.Models;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {        
        protected void CmdGenerar_Click(object sender, EventArgs e)
        {
            JobLogger log = new JobLogger();
            log.AddLogMessage();
            log.Message = "This is the log message";
            log.LogWarning = true;
            log.LogMessage = false;
            log.LogError = false;
           
        }
    }
}