using System.Collections.Generic;

namespace KiStudioCalculator
{
    public interface ICalculator
    {
        IEnumerable<float> Calculate(string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData);
    }
}