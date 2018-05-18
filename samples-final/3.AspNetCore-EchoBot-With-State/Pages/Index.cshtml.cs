﻿using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCore_EchoBot_With_State
{
    public class IndexModel : PageModel
    {
        private IHostingEnvironment _hostingEnv;

        public string DebugLink { get; private set; }

        public string EmulatorDeepLink { get; private set; }

        public IndexModel(IHostingEnvironment hostingEnv)
        {
            this._hostingEnv = hostingEnv;
        }

        public void OnGet()
        {
            string botUrl = $"{ Request.Scheme }://{ Request.Host }/api/messages";
            DebugLink = botUrl;

            // construct emulator protocol URI
            string botFilePath = Path.Combine(this._hostingEnv.ContentRootPath, "AspNetCore-EchoBot-With-State.bot");
            string protocolUri = $"bfemulator://bot.open?path={ HttpUtility.UrlEncode(botFilePath) }";
            EmulatorDeepLink = protocolUri;
        }
    }
}
