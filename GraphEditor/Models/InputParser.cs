using GraphEditor.Exceptions;
namespace GraphEditor.Models
{
    static class InputParser
    {
        public static void ParseVertex(string? input, out int vertex,
                                       string emptyInputExcMsg,
                                       string parseFailsExcMsg)
        {
            if (input is null || input == string.Empty) throw new InvalidInputException(emptyInputExcMsg);
            if (!int.TryParse(input, out vertex)) throw new InvalidInputException(parseFailsExcMsg);
        }
        public static void ParseWeight(string? input, out int weight,
                                       string emptyInputExcMsg,
                                       string parseFailsExcMsg,
                                       string invalidValueExcMsg)
        {
            if (input is null || input == string.Empty) throw new InvalidInputException(emptyInputExcMsg);
            if (!int.TryParse(input, out weight)) throw new InvalidInputException(parseFailsExcMsg);
            if (weight <= 0) throw new InvalidInputException(invalidValueExcMsg);
        }
    }
}
