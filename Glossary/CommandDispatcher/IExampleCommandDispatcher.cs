namespace CommandDispatcher
{
    // This interface is not required, but prevents having the more complicated interface X<C,R> to be 
    // everywhere in the application, which could make refactoring or changing models hard.
    public interface IExampleCommandDispatcher : ICommandDispatcher<BaseCommand, BaseResult>
    {

    }
}
