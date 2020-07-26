using System;

namespace NRecoLinqLambdaParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var calculatorV1PerformanceTest = new CalculatorV1PerformanceTest();
            //calculatorV1PerformanceTest.Execute(2);

            var calculatorV2PerformanceTest = new CalculatorV2PerformanceTest();
            calculatorV2PerformanceTest.Execute(100);
        }
    }
}
