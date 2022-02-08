using MediatorCommandHandler.Characters;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ZCommon;

namespace MediatorCommandHandler;

public class Program : BaseProgram
{
    public static async Task Main(string[] args)
    {
        await Init<Program, MediatorApp>();
    }

    protected override void Startup(ServiceCollection services)
    {
        services.AddTransient<ICharacterHandlerBuilder, CharacterHandlerBuilder>();

        services.AddMediatR(typeof(Program));
    }

    internal class MediatorApp : BaseApp
    {
        private readonly ICharacterHandlerBuilder _characterHandlerBuilder;

        public MediatorApp(ICharacterHandlerBuilder characterHandlerBuilder)
        {
            _characterHandlerBuilder = characterHandlerBuilder;
        }

        public override async Task Run()
        {
            var characters = new[]
            {
                _characterHandlerBuilder.BuildCharacters(new WarriorCharacter(5, 5)),
                _characterHandlerBuilder.BuildCharacters(new WizardCharacter(5, 5)),
                _characterHandlerBuilder.BuildCharacters(new BardCharacter(5, 5)),
            };

            do
            {
                foreach (var character in characters.Where(x => x.IsAlive))
                {
                    character.Walk();

                    foreach (var other in characters.Where(x => x.IsAlive).Except(new[] { character }))
                    {
                        if (InteractionHelper.IsInThreadRange(character, other))
                        {
                            Console.WriteLine($"{character.Name} can reach {other.Name}");

                            var damage = await character.AttackAsync();

                            other.Defend(damage);
                        }
                        else
                        {
                            Console.WriteLine($"{character.Name} must use range attach to reach {other.Name}");

                            var damage = await character.DoMagicAsync();

                            other.ReceiveMagic(damage);
                        }

                        await Task.Delay(500);
                    }


                }
            }
            while (true);
        }
    }
}
