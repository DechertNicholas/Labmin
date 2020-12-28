using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Labmin.Core.Models;

namespace Labmin.PowerShell.Client
{
    [Cmdlet(VerbsCommon.Get, "LabPool")]
    [OutputType(typeof(Pool))]
    public class GetLabPoolCmdlet : Cmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true)]
        public string PoolName { get; set; }

        static HttpClient client = new HttpClient();

        protected override void BeginProcessing()
        {
            client.BaseAddress = new Uri("https://localhost:44365/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        static async Task<Pool> GetPoolAsync(string path)
        {
            Pool pool = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                pool = await response.Content.ReadAsAsync<Pool>();
            }
            return pool;
        }
    }
}
