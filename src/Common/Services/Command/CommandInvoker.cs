using System;
using Microsoft.AspNetCore.Mvc;

namespace Common.Services.Command
{
    public class CommandInvoker
    {
        private ICommand _mainCommand;
        private ICommandIntegrate? _integrationCommand;

        public CommandInvoker(ICommand mainCommand)
        {
            _mainCommand = mainCommand;
            _integrationCommand = null;
        }

        public CommandInvoker(ICommand mainCommand, ICommandIntegrate integrationCommand)
        {
            _mainCommand = mainCommand;
            _integrationCommand = integrationCommand;
        }

        public IActionResult Invoke()
        {
            var mainResult = _mainCommand.Execute();
            if (!mainResult.Success)
            {
                return new StatusCodeResult(500);
            }
            if(_integrationCommand != null)
            {
                _integrationCommand.SetData(mainResult.Response);
                _integrationCommand.Execute();
            }

            return new OkObjectResult(mainResult.Response);
        }
    }
}

