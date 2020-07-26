using KiStudioCalculator;
using Xunit;
using Xunit.Abstractions;

namespace NRecoLinqLambdaParser.functionalTests
{
    public class CalculatorV2Tests : CalculatorEvaluator
    {
        private readonly ITestOutputHelper _testOutput;

        public CalculatorV2Tests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        protected override void WriteLine(string s = "")
        {
            _testOutput.WriteLine(s);
        }

        [Fact]
        public void FormulasWithSignals()
        {
            var calculator = new CalculatorV2();

            //-------------------------------------------
            var samplingRate = 10;

            WriteLine();
            WriteLine($"Formulas with signals: 3 signals (A, B, C), sampling rate = {samplingRate}");
            WriteLine();


            var signalsInputData = new[]
            {
                new DecodedScan(1, new[] {1f, 10f, 100f}),
                new DecodedScan(1, new[] {2f, 20f, 200f}),
                new DecodedScan(1, new[] {3f, 30f, 300f}),
                new DecodedScan(1, new[] {4f, 40f, 400f}),
                new DecodedScan(1, new[] {5f, 50f, 500f}),
                new DecodedScan(1, new[] {6f, 60f, 600f}),
                new DecodedScan(1, new[] {7f, 70f, 700f})
            };


            WriteLineTitle(
                "----------------------------- input signals",
                "----------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "sig(A)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(C)", samplingRate, signalsInputData);
            WriteLine();
            
            WriteLineTitle(
                "------------------------- simple operations",
                "----------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "integrate(25)", samplingRate, signalsInputData);
            Evaluate(calculator, "sqrt(25)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(C) + 7", samplingRate, signalsInputData);
            Evaluate(calculator, "sqrt(sig(C))", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) + sig(A)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) - sig(A)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) * sig(A)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) / sig(A)", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) / sig(A) +1", samplingRate, signalsInputData);
            Evaluate(calculator, "sig(B) / (sig(A) +1)", samplingRate, signalsInputData);
            WriteLine();
            WriteLineTitle(
                "--------------------- integration operation",
                "-------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "sig(C)", samplingRate, signalsInputData);
            Evaluate(calculator, "integrate(sig(C))", samplingRate, signalsInputData);
            Evaluate(calculator, "sigIntegrate(C)", samplingRate, signalsInputData);
            Evaluate(calculator, "integrate(sig(C)*10)", samplingRate, signalsInputData);
            Evaluate(calculator, "sigIntegrate(C)*10", samplingRate, signalsInputData);
            Evaluate(calculator, "111 - integrate(sig(C))", samplingRate, signalsInputData);
            Evaluate(calculator, "111 - sigIntegrate(C)", samplingRate, signalsInputData);
            WriteLine();

            WriteLineTitle(
                "-------------------differentiate operations",
                "-------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "sig(C)", samplingRate, signalsInputData);
            Evaluate(calculator, "sigDifferentiate(C)", samplingRate, signalsInputData);
            Evaluate(calculator, "111 - sigDifferentiate(C)", samplingRate, signalsInputData);
            WriteLine();


            WriteLineTitle(
                "---------------------------typical formulas",
                "-------------------------------------------------------");
            WriteLineResult("FORMULA", "RESULT VALUES");
            WriteLine();

            Evaluate(calculator, "sqrt(sig(A)*sig(A) + sig(B)*sig(B))", samplingRate, signalsInputData);

        }
    }
}