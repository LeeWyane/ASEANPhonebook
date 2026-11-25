
using System;
using System.Collections.Generic;
using System.Linq;

// CLASS 1: STUDENT
// Represents one student in the phonebook.

class Student
{
    public string StudentNumber { get; private set; }
    public string Surname { get; private set; }
    public string FirstName { get; private set; }
    public string Occupation { get; private set; }
    public char Gender { get; private set; }
    public int CountryCode { get; private set; }
    public int AreaCode { get; private set; }
    public string PhoneNumber { get; private set; }

    public Student(string studentNumber, string surname,
        string firstName, string occupation, char gender,
        int countryCode, int areaCode, string phoneNumber)
    {
        StudentNumber = studentNumber;
        Surname = surname;
        FirstName = firstName;
        Occupation = occupation;
        Gender = gender;
        CountryCode = countryCode;
        AreaCode = areaCode;
        PhoneNumber = phoneNumber;
    }

    // Methods used when editing student information.

    public void ChangeStudentNumber(string value)
    {
        StudentNumber = value;
    }

    public void ChangeSurname(string value)
    {
        Surname = value;
    }

    public void ChangeGender(char value)
    {
        Gender = value;
    }

    public void ChangeOccupation(string value)
    {
        Occupation = value;
    }

    public void ChangeCountryCode(int value)
    {
        CountryCode = value;
    }

    public void ChangeAreaCode(int value)
    {
        AreaCode = value;
    }

    public void ChangePhoneNumber(string value)
    {
        PhoneNumber = value;
    }

    public void DisplayInformation()
    {
        Console.WriteLine(
            $"{FirstName} {Surname} is a {Occupation}. " +
            $"His/Her number is {CountryCode}-{AreaCode}-{PhoneNumber}"
        );
    }
}

// CLASS 2: ASEAN PHONEBOOK
// Manages the collection of student objects.

class AseanPhonebook
{
    private List<Student> students = new List<Student>();

    // Supported country codes and their names.
    private readonly Dictionary<int, string> countries =
        new Dictionary<int, string>()
        {
            { 60, "Malaysia" },
            { 62, "Indonesia" },
            { 63, "Philippines" },
            { 65, "Singapore" },
            { 66, "Thailand" }
        };

    public bool IsValidCountryCode(int code)
    {
        return countries.ContainsKey(code);
    }

    public string GetCountryName(int code)
    {
        return countries.ContainsKey(code)
            ? countries[code]
            : "Unknown";
    }

    public bool AddStudent(Student student)
    {
        if (FindStudent(student.StudentNumber) != null)
        {
            return false;
        }

        students.Add(student);
        return true;
    }

    public Student FindStudent(string studentNumber)
    {
        return students.FirstOrDefault(
            s => s.StudentNumber.Equals(
                studentNumber,
                StringComparison.OrdinalIgnoreCase
            )
        );
    }

    public bool IsStudentNumberUsed(
        string studentNumber, Student currentStudent)
    {
        Student existing = FindStudent(studentNumber);

        return existing != null && existing != currentStudent;
    }

    public List<Student> SearchByCountries(
        List<int> selectedCountries)
    {
        return students
            .Where(s => selectedCountries.Contains(s.CountryCode))
            .OrderBy(s => s.Surname)
            .ThenBy(s => s.FirstName)
            .ToList();
    }

    public List<Student> GetAllStudents()
    {
        return students
            .OrderBy(s => s.Surname)
            .ThenBy(s => s.FirstName)
            .ToList();
    }

    public bool HasStudents()
    {
        return students.Count > 0;
    }

    public void DisplayStudents(List<Student> results)
    {
        if (results.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        foreach (Student student in results)
        {
            Console.WriteLine(
                $"{student.Surname}, {student.FirstName}, " +
                $"with student number {student.StudentNumber}, " +
                $"is a {student.Occupation}. " +
                $"His/Her phone number is " +
                $"{student.CountryCode}-{student.AreaCode}-{student.PhoneNumber}"
            );
        }
    }
}


// CLASS 3: PROGRAM
// Handles the menus and user interaction.

class Program
{
    static AseanPhonebook phonebook = new AseanPhonebook();

    static void Main()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n==============================");
            Console.WriteLine("       ASEAN PHONEBOOK");
            Console.WriteLine("==============================");
            Console.WriteLine("[1] Store to ASEAN phonebook");
            Console.WriteLine("[2] Edit entry in ASEAN phonebook");
            Console.WriteLine("[3] Search ASEAN phonebook by country");
            Console.WriteLine("[4] Exit");
            Console.Write("\nEnter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    StoreEntry();
                    break;

                case "2":
                    EditEntry();
                    break;

                case "3":
                    SearchEntries();
                    break;

                case "4":
                    running = false;
                    Console.WriteLine("Thank you for using ASEAN Phonebook!");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }


    // ======================================
    // STORE STUDENT
    // ======================================

    static void StoreEntry()
    {
        bool addAnother = true;

        while (addAnother)
        {
            Console.WriteLine("\n--- STORE STUDENT ---");

            string studentNumber = ReadRequired("Enter student number: ");

            if (phonebook.FindStudent(studentNumber) != null)
            {
                Console.WriteLine("Student number already exists.");
                continue;
            }

            string surname = ReadRequired("Enter surname: ");
            string firstName = ReadRequired("Enter first name: ");
            string occupation = ReadRequired("Enter occupation: ");

            char gender;

            while (true)
            {
                Console.Write("Enter gender (M/F): ");
                string input = (Console.ReadLine() ?? "").ToUpper();

                if (input == "M" || input == "F")
                {
                    gender = input[0];
                    break;
                }

                Console.WriteLine("Please enter M or F.");
            }

            int countryCode;

            while (true)
            {
                countryCode = ReadInteger("Enter country code: ");

                if (phonebook.IsValidCountryCode(countryCode))
                    break;

                Console.WriteLine(
                    "Invalid country code. Allowed codes: 60, 62, 63, 65, 66."
                );
            }

            int areaCode = ReadInteger("Enter area code: ");
            string phoneNumber = ReadRequired("Enter number: ");

            Student newStudent = new Student(
                studentNumber,
                surname,
                firstName,
                occupation,
                gender,
                countryCode,
                areaCode,
                phoneNumber
            );

            if (phonebook.AddStudent(newStudent))
            {
                Console.WriteLine("Student successfully added!");
            }

            Console.Write("\nDo you want to enter another entry [Y/N]? ");
            string answer = (Console.ReadLine() ?? "").ToUpper();

            addAnother = answer == "Y";
        }
    }

    // EDIT STUDENT

    static void EditEntry()
    {
        Console.Write("\nEnter student number: ");
        string number = Console.ReadLine() ?? "";

        Student student = phonebook.FindStudent(number);

        if (student == null)
        {
            Console.WriteLine("Error: Student number does not exist.");
            return;
        }

        bool editing = true;

        while (editing)
        {
            Console.WriteLine("\nHere is the existing information:");
            Console.WriteLine(
                $"{student.FirstName} {student.Surname} is a " +
                $"{student.Occupation}. His/Her number is " +
                $"{student.CountryCode}-{student.AreaCode}-{student.PhoneNumber}"
            );

            Console.WriteLine("\nWhich information do you wish to change?");
            Console.WriteLine("[1] Student number");
            Console.WriteLine("[2] Surname");
            Console.WriteLine("[3] Gender");
            Console.WriteLine("[4] Occupation");
            Console.WriteLine("[5] Country code");
            Console.WriteLine("[6] Area code");
            Console.WriteLine("[7] Phone number");
            Console.WriteLine("[8] None - Go back to main menu");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    string newNumber = ReadRequired("Enter new student number: ");

                    if (phonebook.IsStudentNumberUsed(newNumber, student))
                        Console.WriteLine("That student number is already in use.");
                    else
                        student.ChangeStudentNumber(newNumber);
                    break;

                case "2":
                    student.ChangeSurname(ReadRequired("Enter new surname: "));
                    break;

                case "3":
                    while (true)
                    {
                        string input = ReadRequired("Enter new gender (M/F): ").ToUpper();

                        if (input == "M" || input == "F")
                        {
                            student.ChangeGender(input[0]);
                            break;
                        }

                        Console.WriteLine("Please enter M or F.");
                    }
                    break;

                case "4":
                    student.ChangeOccupation(ReadRequired("Enter new occupation: "));
                    break;

                case "5":
                    int newCountryCode;

                    while (true)
                    {
                        newCountryCode = ReadInteger("Enter new country code: ");

                        if (phonebook.IsValidCountryCode(newCountryCode))
                        {
                            student.ChangeCountryCode(newCountryCode);
                            break;
                        }

                        Console.WriteLine("Invalid country code.");
                    }
                    break;

                case "6":
                    student.ChangeAreaCode(ReadInteger("Enter new area code: "));
                    break;

                case "7":
                    student.ChangePhoneNumber(ReadRequired("Enter new phone number: "));
                    break;

                case "8":
                    editing = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    // SEARCH BY COUNTRY

    static void SearchEntries()
    {
        if (!phonebook.HasStudents())
        {
            Console.WriteLine("\nThe phonebook is empty.");
            return;
        }

        List<int> selectedCountries = new List<int>();

        Console.WriteLine("\nFrom which country?");
        Console.WriteLine("[1] Philippines");
        Console.WriteLine("[2] Thailand");
        Console.WriteLine("[3] Singapore");
        Console.WriteLine("[4] Indonesia");
        Console.WriteLine("[5] Malaysia");
        Console.WriteLine("[6] ALL");
        Console.WriteLine("[0] No More");

        while (true)
        {
            Console.Write("\nEnter choice: ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "0")
                break;

            if (choice == "6")
            {
                selectedCountries.Clear();

                selectedCountries.AddRange(
                    new List<int> { 60, 62, 63, 65, 66 }
                );

                break;
            }

            int countryCode = choice switch
            {
                "1" => 63,
                "2" => 66,
                "3" => 65,
                "4" => 62,
                "5" => 60,
                _ => -1
            };

            if (countryCode == -1)
            {
                Console.WriteLine("Invalid choice.");
                continue;
            }

            if (!selectedCountries.Contains(countryCode))
            {
                selectedCountries.Add(countryCode);
            }
            else
            {
                Console.WriteLine("Country already selected.");
            }
        }

        if (selectedCountries.Count == 0)
        {
            Console.WriteLine("No countries selected.");
            return;
        }

        List<Student> results =
            phonebook.SearchByCountries(selectedCountries);

        Console.WriteLine("\n--- SEARCH RESULTS ---");

        phonebook.DisplayStudents(results);
    }

    // INPUT HELPER METHODS

    static string ReadRequired(string message)
    {
        while (true)
        {
            Console.Write(message);
            string value = (Console.ReadLine() ?? "").Trim();

            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine("This field cannot be empty.");
        }
    }

    static int ReadInteger(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out int value) && value >= 0)
                return value;

            Console.WriteLine("Please enter a valid non-negative number.");
        }
    }
}
