﻿using System.Threading.Tasks;

namespace CommandDispatcher
{
    public class CommandA2Handler : ICommandHandler<CommandA, Result2>
    {
        public Task<Result2> Handle(CommandA command)
        {
            return Task.FromResult(new Result2());
        }
    }
}
