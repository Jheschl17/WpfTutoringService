using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TutoringService.data
{
    public class Student
    {
        public string Clazz { get; set; }

        public string Firstname { get; set; }

        public string ImagePath { get => $"{Configuration.ImagesPath}/{Lastname}_{Firstname}.jpg"; }

        public string Lastname { get; set; }

        public string Name { get => $"{Lastname} {Firstname}"; }

        public List<Tutoring> Tutorings { get; set; } = new List<Tutoring>();

        public static Student Parse(string line)
        {
            var elements = Regex.Split(line, ";| ");
            return new Student { Clazz = elements[0], Lastname = elements[1], Firstname = elements[2] };
        }

        public override bool Equals(object obj)
        {
            return obj is Student student &&
                   Clazz == student.Clazz &&
                   Firstname == student.Firstname &&
                   ImagePath == student.ImagePath &&
                   Lastname == student.Lastname &&
                   Name == student.Name;
        }

        public override string ToString()
        {
            return Name;
        }


        public List<String> TutoringsCsv()
        {
            return Tutorings
                .Select(tutoring => tutoring.ToCsvString(Name))
                .ToList();
        }
    }
}
