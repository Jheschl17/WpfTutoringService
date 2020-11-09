using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Navigation;

namespace TutoringService.data
{
    public sealed class StudentsSingleton
    {
        private static readonly Lazy<StudentsSingleton> lazy =
            new Lazy<StudentsSingleton>(() => new StudentsSingleton());

        public static StudentsSingleton Instance { get { return lazy.Value; } }

        public List<Student> Students { get; set; }

        private StudentsSingleton()
        {
            LoadData();
        }

        public void LoadData()
        {
            // Read Students
            Students = File.ReadAllLines(Configuration.StudentsPath)
                .Select(line => Student.Parse(line))
                .ToList();

            // Read tutorings
            File.ReadAllLines(Configuration.TutoringsPath)
                .ToList()
                .ForEach(line =>
                {
                    var elements = line.Split(";");
                    var studentName = elements[0];
                    var tutoring = new Tutoring { Subject = elements[1], Level = int.Parse(elements[2]) };

                    var tutoringStudent = (from std in Students
                                          where std.Name == studentName
                                          select std).First();

                    tutoringStudent.Tutorings.Add(tutoring);
                });
        }
    }
}
