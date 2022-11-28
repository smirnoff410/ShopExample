using System;
namespace Common.Services.Command
{
    public interface ICommandIntegrate : ICommand
    {
        public void SetData(object data);
    }
}

