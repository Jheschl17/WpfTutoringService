using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TutoringService.data
{
    public class Tutoring
    {
        public int Level { get; set; }

        public string Subject { get; set; }

        public override string ToString()
        {
            return $"{Subject} in {Level}.Klassen";
        }

        public string ToCsvString(string studentName)
        {
            return $"{studentName};{Subject};{Level}";
        }
    }
}
