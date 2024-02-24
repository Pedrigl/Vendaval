using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vendaval.Infrastructure.Data.Contexts
{
    public class ConnStr
    {
        public string VendavalDb { get; set; }

        public string Redis { get; set; }

        public ConnStr(IConfiguration configuration) {
            VendavalDb = ParseConnectionString(configuration.GetConnectionString("VendavalDb"));

            Redis = configuration["Redis:Url"];

        }

        private string ParseConnectionString(string url)
        {
            var uriString = new Uri(url);
            var db = uriString.AbsolutePath.TrimStart('/');
            var user = uriString.UserInfo.Split(':')[0];
            var password = uriString.UserInfo.Split(':')[1];
            var port = uriString.Port > 0 ? uriString.Port : 5432;
            var connStr = $"Server={uriString.Host};Database={db};User Id={user};Password={password};Port={port}";
            return connStr;
        }
    }
}
