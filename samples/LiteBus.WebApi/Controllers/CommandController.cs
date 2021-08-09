﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LiteBus.WebApi.Commands;
using LiteBus.WebApi.Events;

namespace LiteBus.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandMediator _commandMediator;
        
        public CommandController(ICommandMediator commandMediator)
        {
            _commandMediator = commandMediator;
        }

        [HttpPost("with-result")]
        public async Task<string> SendCommandWithResult(CreateNumberCommandWithResult command)
        {
            return await _commandMediator.SendAsync(command);
        }

        [HttpPost("without-result")]
        public Task SendCommandWithoutResult(CreateNumberCommand command)
        {
            return _commandMediator.SendAsync(command);
        }
    }
}