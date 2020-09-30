using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
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

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
    }
}
