using System;
namespace Common.Services.Command
{
    public abstract class CommandIntegrate : ICommandIntegrate
    {
        public abstract CommandResponse Execute();

        public abstract void SetData(object data);
    }
}

