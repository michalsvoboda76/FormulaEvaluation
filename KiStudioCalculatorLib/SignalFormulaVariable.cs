using System.Runtime.Serialization;

namespace KiStudioCalculatorLib
{
    [DataContract]
    public class SignalFormulaVariable
    {
        [DataMember]
        public int Index { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}