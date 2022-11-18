using System;
using Microsoft.Extensions.Logging;

namespace Common.Services.Command
{
    public abstract class Command : ICommand
    {
        public abstract CommandResponse Execute();

        public virtual void SetData(object data)
        {

        }
    }
}

