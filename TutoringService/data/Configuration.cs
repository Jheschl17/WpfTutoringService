using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TutoringService.data
{
    static class Configuration
    {
        public static readonly string StudentsPath = "resources/csv/students.csv";

        public static readonly string SubjectsPath = "resources/csv/subjects.csv";

        public static readonly string TutoringsPath = "resources/csv/tutorings.csv";

        public static readonly string ImagesPath = "resources/images";

        public static int[] Levels { get; } = { 1, 2, 3, 4, 5 };

        public static List<String> Subjects { get; }

        static Configuration()
        {
            var subjectLine = File.ReadAllLines(SubjectsPath)[0];
            Subjects = subjectLine.Split(";").ToList();
        }
    }
}
