using System.Collections.Generic;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;

namespace NRecoLinqLambdaParser.functionalTests
{
    public class LambdaParserTests
    {
        private readonly ITestOutputHelper _testOutput;

        public LambdaParserTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }


        [Fact]
        public void FormulasWithoutSignalsTest()
        {
            var lambdaParser = new NReco.Linq.LambdaParser();
            var formulaVariables = new Dictionary<string, object>();

            WriteLine();
            WriteLine("Formulas with constants");
            WriteLine();
            WriteLineResult("FORMULA", "RESULT");
            WriteLine();

            Evaluate(lambdaParser, "1", formulaVariables);
            Evaluate(lambdaParser, "(1+8)", formulaVariables);
            Evaluate(lambdaParser, "(1+8)/3", formulaVariables);
            Evaluate(lambdaParser, "(1+8)/3 + 1", formulaVariables);

            //-------------------------------------------
            WriteLine();
            WriteLine("Formulas with constants and variables");
            WriteLine();
            WriteLineResult("FORMULA", "RESULT");
            WriteLine();

            formulaVariables["pi"] = 3.14M;
            formulaVariables["var1"] = 1M;
            formulaVariables["var2"] = 2M;

            Evaluate(lambdaParser, "var1", formulaVariables);
            Evaluate(lambdaParser, "var2", formulaVariables);
            Evaluate(lambdaParser, "var1 + var2", formulaVariables);
            Evaluate(lambdaParser, "(1+8)/3 + 1 * var2", formulaVariables);
            Evaluate(lambdaParser, "pi", formulaVariables);
            Evaluate(lambdaParser, "pi+1 -var2", formulaVariables);
        }


        private void Evaluate(NReco.Linq.LambdaParser lambdaParser, string formulaDefinition, IDictionary<string, object> formulaVariables)
        {
            var evalResult = lambdaParser.Eval(formulaDefinition, formulaVariables);
            //var evalResultString = $"{evalResult:0.##}";
            var evalResultString = string.Format(CultureInfo.InvariantCulture, "{0:0.##}", evalResult);
            WriteLineResult(formulaDefinition, evalResultString);
        }

        private void WriteLineResult(string formulaDefinition, string evalResultString)
        {
            WriteLine($"{formulaDefinition,50} == {evalResultString,-10}");
        }

        private void WriteLineTitle(string formulaDefinition, string evalResultString)
        {
            WriteLine($"{formulaDefinition,50} {evalResultString,-10}");
        }


        protected void WriteLine(string s = "")
        {
            _testOutput.WriteLine(s);
        }
    }
}