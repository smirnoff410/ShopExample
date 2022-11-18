using System;
namespace Common.Services.Command
{
    public interface ICommand
    {
        void SetData(object data);

        CommandResponse Execute();
    }
}

