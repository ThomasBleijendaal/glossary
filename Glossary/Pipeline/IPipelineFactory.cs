namespace Pipeline
{
    public interface IPipelineFactory
    {
        Pipeline<int> BuildPipeline();
    }
}
