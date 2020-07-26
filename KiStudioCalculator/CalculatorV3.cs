using System;
using System.Collections.Generic;
using System.Linq;
using NReco.Linq;

namespace KiStudioCalculator
{
    public class CalculatorV3
    {
        private readonly LambdaParser _lambdaParser = new LambdaParser();
        private string _formulaDefinition;
        private readonly Dictionary<string, object> _formulaVariables = new Dictionary<string, object>();
        private SignalFormulaVariable[] _signalFormulaVariables = new SignalFormulaVariable[0];
        private int _samplingRate;
        private DecodedScan[] _signalsInputData;

        private int _currentSampleIndex = -1;

        public CalculatorV3()
        {
            Init();
        }


        private void Init()
        {
            _formulaVariables["sig"] = (Func<int, float>)((indexOfSignal) => _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal]);

            _formulaVariables["sqrt"] = (Func<float, float>)((v) => (float)Math.Sqrt(v));
            _formulaVariables["integrate"] = (Func<float, float>)((v) => v / _samplingRate);
            _formulaVariables["sigIntegrate"] = (Func<int, float>)((indexOfSignal) => _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal] / _samplingRate);
            _formulaVariables["sigDifferentiate"] = (Func<int, float>)((indexOfSignal)
                => (
                       _signalsInputData[_currentSampleIndex].ChannelValues[indexOfSignal]
                       - (_currentSampleIndex == 0 ? 0f : _signalsInputData[_currentSampleIndex - 1].ChannelValues[indexOfSignal])
                   )
                   * _samplingRate);

            
        }

        public void SetSignalFormulaVariables(IEnumerable<SignalFormulaVariable> newSignalFormulaVariables)
        {
            // remove old signal formula variables
            foreach (var signalFormulaVariable in _signalFormulaVariables)
            {
                _formulaVariables.Remove(signalFormulaVariable.Name);
            }

            // add new signal formula variables
            _signalFormulaVariables = newSignalFormulaVariables.ToArray();
            foreach (var signalFormulaVariable in _signalFormulaVariables)
            {
                _formulaVariables[signalFormulaVariable.Name] = signalFormulaVariable.Index;
            }

        }

        public IEnumerable<DecodedScan> Calculate(string formulaDefinition, int samplingRate, DecodedScan[] signalsInputData)
        {
            _formulaDefinition = formulaDefinition;
            _samplingRate = samplingRate;
            _signalsInputData = signalsInputData;
            _currentSampleIndex = -1;

            for (var sampleIndex = 0; sampleIndex < _signalsInputData.Length; sampleIndex++)
            {
                _currentSampleIndex = sampleIndex;

                var resultObject = _lambdaParser.Eval(_formulaDefinition, _formulaVariables);
                var resultValue = Convert.ToSingle(resultObject);
                yield return new DecodedScan
                {
                    EpochTimestamp = _signalsInputData[sampleIndex].EpochTimestamp,
                    ChannelValues = new[] { resultValue }
                };
            }
        }

    }
}