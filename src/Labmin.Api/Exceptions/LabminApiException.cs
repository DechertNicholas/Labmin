using Labmin.Core.Models;
using System;
using System.Runtime.Serialization;

namespace Labmin.Api
{
    public class LabminApiException : Exception
    {
        public LabminApiException()
        {
        }

        public LabminApiException(string message) : base(message)
        {
        }

        public LabminApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LabminApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class PoolNotFoundException : LabminApiException
    {
        public PoolNotFoundException(Pool pool) : this(pool.Name)
        {
        }

        public PoolNotFoundException(string poolName) : base($"Pool {poolName} was not found.")
        {
        }
    }

    public class MachineNotFoundException : LabminApiException
    {
        public MachineNotFoundException(Machine machine)
            : base($"Machine {machine.Name} was not found.")
        {
        }

        public MachineNotFoundException(string machineName)
            : base($"Machine {machineName} was not found.")
        {
        }
    }
}
