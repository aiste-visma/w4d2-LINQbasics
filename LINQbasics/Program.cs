using System.Numerics;
using System.Diagnostics.Tracing;
record Student(int Id, string Name, int GroupId);
record Group(int Id, string Name);

class Program
{
    static void Main()
    {

        //WordFiltering
        var words = new List<string>
        { "alus", "sula", "la", "vanduo", "programavimas",
          "katinas", "saulė", "medis", "oras", "knyga" };

        var longerThanFour = words.Where(w => w.Length > 4);
        foreach (var word in longerThanFour)
            Console.Write($"{word} ");
        Console.WriteLine("");

        var startsWithS = words.Where(w => w.StartsWith("s"));
        foreach (var word in startsWithS)
            Console.Write($"{word} ");
        Console.WriteLine("");


        var orderedByLength = words.OrderBy(w => w.Length);
        foreach (var word in orderedByLength)
            Console.Write($"{word} ");
        Console.WriteLine("");

        var wordsUpper = words.Select(w => w.ToUpper());
        foreach (var word in wordsUpper)
            Console.Write($"{word} ");
        Console.WriteLine("");

        var groupedByFirstLetter = words.GroupBy(w => w[0]);
        foreach (var key in groupedByFirstLetter)
        {
            Console.WriteLine($"Key: {key.Key}");
            foreach (var word in key)
                Console.WriteLine($"    {word}");
        }

        var longestWord = words.OrderBy(w => w.Length).Last();
        Console.WriteLine("Longest word: " + longestWord);

        var atLeastOneSymbol = words.All(w => w.Length >= 1);
        Console.WriteLine(atLeastOneSymbol);

        //Calculations
        Console.WriteLine("LINQ with numbers-----------------------------------------");

        var numbers = Enumerable.Range(1, 100).ToList();

        var evenNumbers = numbers.Where(num => num % 2 == 0);
        Console.WriteLine("Even numbers: ");
        foreach (var number in evenNumbers)
            Console.Write($"{number} ");

        Console.WriteLine();

        var unevenSum = numbers.Where(num => num % 2 != 0).Sum();
        Console.WriteLine("Sum of uneven numbers: " + unevenSum);

        var dividableByThree = numbers.Where(num => num % 3 == 0).Take(10);
        Console.WriteLine("10 numbers, dividable by 3: ");
        foreach (var number in dividableByThree)
            Console.Write($"{number} ");

        Console.WriteLine();

        var grouped = numbers.GroupBy(num =>
        {
            if (num < 34) return "maži";
            else if (num < 67) return "vidutiniški";
            else return "dideli";
        });
        foreach (var key in grouped)
        {
            Console.WriteLine($"Key: {key.Key}");
            foreach (var word in key)
                Console.WriteLine($"    {word}");
        }

        var primaryDict = new Dictionary<int, string>();

        string chkprime(int num)
        {
            if (num < 2)
                return "not primary";
            for (int i = 2; i < num; i++)
            {
                if (num % i == 0)
                {
                    return "not primary";
                }
            }
            return "primary";
        }

        var primaries = numbers.Select(num => new { Key = num, Value = chkprime(num) });

        primaryDict = primaries.ToDictionary(x => x.Key, x => x.Value);

        foreach (KeyValuePair<int, string> entry in primaryDict)
        {
            var key = entry.Key;
            var value = entry.Value;

            Console.WriteLine(key + " is " + value);
        }

        //SelectMany

        var sentences = new List<string>
            { "LINQ yra galingas", "C# yra puiki kalba", "Generics ir Delegates" };

        var sentenceWords = sentences.SelectMany(words => words.Split());

        foreach (var word in sentenceWords)
        {
            Console.WriteLine(word);
        }

        var wordQuery =
            (from string word in sentenceWords
             orderby word
             select word).Distinct();

        string[] result = wordQuery.ToArray();
        Console.WriteLine("Unique words: ");
        foreach (var word in result)
        { Console.WriteLine(word); }


        var wordCount = sentenceWords.GroupBy(w => w)
            .ToDictionary(c => c.Key, c => c.Count());

        foreach (var word in wordCount)
        {
            Console.WriteLine($"{word.Key}: {word.Value}");
        }

        //Join


        var students = new List<Student>
        {
            new Student(1, "Alice", 1),
            new Student(2, "Bob", 1),
            new Student(3, "Charlie", 2),
        };

        var groups = new List<Group>
        {
            new Group(1, "Math"),
            new Group(2, "Physics"),
        };


        var joinedStudents = students.Join(groups, s => s.GroupId, g => g.Id,
            (s, g) => $"Studentas {s.Name} yra {g.Name} grupeje");

        foreach (var student in joinedStudents)
        {
            Console.WriteLine(student);
        }

        var multipleStudentGroups = groups.GroupJoin(
            students, g => g.Id, s => s.GroupId,
            (g, students) => new { Group = g.Name, Students = students });

        Console.WriteLine("Groups with 2 or more students: ");
        foreach (var g in multipleStudentGroups.Where(g => g.Students.Count() >= 2))
        {
            Console.WriteLine(g.Group);
        }

        //Deferred execution
        

        var num1 = new List<int>{ 0, 1, 2, 3 };
        
        var deferred = num1.Where(num => num > 1);
        num1.Add(4); //4 prisideda, nes deferred dar nebuvo executed

        Console.WriteLine("Deferred execution: ");
        foreach (var n in deferred) //deferred executed tik šitoj vietoj, kai per jį iteruojama
            Console.WriteLine(n);

        var num2 = new List<int> { 5, 6, 7, 8 };
        var immediate = num2.Where(num => num > 7).ToList();
        num2.Add(9); //9 neįtraukiamas į sarašą, nes query jau executed dėl pridėto .ToList()

        Console.WriteLine("Immediate execution: ");
        foreach (var n in immediate)
            Console.WriteLine(n);
        

    }
}
