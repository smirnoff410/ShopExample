using System;
namespace Common.Services.Command
{
    public interface ICommand
    {
        CommandResponse Execute();
    }
}

