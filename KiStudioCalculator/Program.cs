using System;

namespace KiStudioCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            const string signalsDefinition = @"
            [
              { Index: 0, Name: 'A' },
              { Index: 1, Name: 'B' },
              { Index: 2, Name: 'C' },
            ]";
            //var formula = "sig(A)+sig(B)+sig(C)+sig(D)+sig(E)+sig(F)+sig(G)";
            var formula = "sig(A)+sig(B)+sig(C)";
            CalculatorApi.SetSignals(signalsDefinition);
            Console.WriteLine($"formula: {formula}");
            Console.WriteLine(CalculatorApi.Calculate(formula));
        }
    }
}
