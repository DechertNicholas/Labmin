using System;
using System.Collections.Generic;
using System.Text;

namespace Labmin.Core.Models
{
    public class Machine
    {
        public string Key { get; set; }
        public string Name { get; set; }

        public string Role { get; set; }

        public string ServiceState { get; set; }

        public string MaintenanceState { get; set; }

        public Pool Pool { get; set; }

        public string Forest { get; set; }

        public string CurrentVersion { get; set; }

        public string DesiredVersion { get; set; }
    }
}
