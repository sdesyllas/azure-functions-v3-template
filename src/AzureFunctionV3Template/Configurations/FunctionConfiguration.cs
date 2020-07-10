namespace AzureFunctionDependencyInjection.Configurations
{
    public class FunctionConfiguration
    {
        public string ServiceName { get; set; }
        public string NegativeResponseMessage { get; set; }
        public string PositiveResponseMessage { get; set; }
        public string ASecret { get; set; }
    }
}