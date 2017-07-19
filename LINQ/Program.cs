using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            Company apple = new Company {Name = "APPLE"};
            Company nokia = new Company {Name = "Nokia"};
            Company samsumg = new Company {Name = "Samsumg"};
            Company asus = new Company {Name = "Asus"};
            Company leeco = new Company { Name = "Leeco"};
            Company zte = new Company {Name = "ZTE"};

            var companies = new List<Company>
            {
                apple, nokia, samsumg, asus, leeco, zte
            };
           
            var smartphones = new List<Smartphone>
            {
                new Smartphone {Model = "Iphone 7", Producer=apple, Ram=4, Rom=64, System="IOS"},
                new Smartphone {Model = "Galaxy 7", Producer=samsumg , Ram =4, Rom=32, System = "Android"},
                new Smartphone {Model = "Iphone 6", Producer=apple, Ram=3, Rom=32, System="IOS"},
                new Smartphone {Model = "Nokia 7", Producer=nokia, Ram=6, Rom=64, System = "Windows"},
                new Smartphone {Model = "Leeco 2", Producer=leeco, Ram=2, Rom=32, System = "Android"},
                new Smartphone {Model = "Leeco LeMAX", Producer=leeco, Ram=3, Rom=32, System = "Android"}
            };

            var newSmartphones = new List<Smartphone>
            {
                new Smartphone {Model = "Iphone 8", Producer=apple, Ram=8, Rom=128, System="IOS"},
                new Smartphone {Model = "ZTE X4", Producer=zte , Ram=10, Rom=512, System = "Android"},
                new Smartphone {Model = "Nokia 7", Producer=nokia, Ram=6, Rom=64, System = "Windows"},
                new Smartphone {Model = "Leeco LeMAX 2", Producer=leeco, Ram=3, Rom=32, System = "EUI7"}
            };

            // Filtering: Where, Take, Skip, TakeWhile, SkipWhile, Distinct
            // Projection Select, SelectMany

            // filtering, projection
            WriteLine("Filtering and projection:");
            var filterQuery = smartphones
                .Where(s => s.Ram > 3)
                .Select(s => s.Producer.Name.ToUpper() + " " + s.Model.ToUpper());

            foreach (var result in filterQuery)
            {
                WriteLine(result);
            }
            WriteLine();

           // Joining: Join, GroupJoin, Zip
            WriteLine("Joining:");
            var q =
                from c in companies
                join s in smartphones on c equals s.Producer
                select new
                {Company ="Company: " + c.Name,
                Description = "Model: " + s.Model + " System " + s.System};
  
            foreach (var v in q)
            {
                WriteLine(v.Company + ": " + v.Description);
            }

            WriteLine();
            // join fluid query
            WriteLine("Joining fluid method:");
            var q1 = companies.Join(smartphones, c => c, s => s.Producer, (c, s) =>
                new { Company = c.Name, Product = " " + s.Model });

            foreach (var v in q1)
            {
                WriteLine(v.Company + v.Product);
            }

            WriteLine();

            // Ordering: OrderBy, ThenBy, OrderByDescending, ThenByDescending, Reverse
            WriteLine("Ordering:");
            var orderingQuery = smartphones
                .OrderByDescending(s => s.Ram) 
                .Select(s => s.Model.ToUpper() + " Ram: " + s.Ram);

            foreach (var result in orderingQuery)
            {
                WriteLine(result);
            }
            WriteLine();

            // Grouping: GroupBy
            WriteLine("Grouping:");
            var gruopQuery = smartphones
                .GroupBy(s => s.Producer)
                .Select(s => s);

            foreach (var group in gruopQuery)
            {
                WriteLine(group.Key.Name );
                foreach (var smartphone in group)
                {
                    WriteLine("   " + smartphone.Model);
                }
            }
            WriteLine();

            // Sets: Concat, Union, Intersect, Except
            WriteLine("Unique company from both List:");

            var uniqueCompanies = smartphones
                .Select(s => s.Producer)
                .Union(newSmartphones
                .Select(n => n.Producer));

            foreach (var company in uniqueCompanies)
            {
                WriteLine(company.Name);
            }
            WriteLine();

            WriteLine("System present in  NewSmartphones List but not in second:");
            var newSmartphonesSystem = newSmartphones
                .Select(s => s.System)
                .Except(smartphones
                    .Select(s => s.System));

            foreach (var sys in newSmartphonesSystem)
            {
                WriteLine(sys);
            }
            WriteLine();

            // Conversion methods: OfType, Cast, ToArray, ToList, ToDictionary, ToLookup, AsEnumerable, AsQueryable
            WriteLine("Convert query in List<Company>:");
            List<Company> companyList  = uniqueCompanies.ToList();
            WriteLine(companyList.GetType());
            WriteLine();

            // Element operators: First, FirstOrDefault, Last, LastOrDefault, ElementAt, ElementAtOrDefault, DefaultIfEmpty
            WriteLine("Select first element:");
            var elementBeginWithI = smartphones.First(s => s.Model[0] == 'I');
            var elementBeginWithT = smartphones.FirstOrDefault(s => s.Model[0] == 'T');
            WriteLine(elementBeginWithI.Model);
            WriteLine("Product begining with 'T' exists: {0}", elementBeginWithT != null);
            WriteLine();

            // Agregation Methods: Count, LongCount, Min, MAx, Sum, Average, Aggregate
            WriteLine("Average Smartphones Rom:");
            var avgSmartphoneRom = smartphones.Average(s => s.Rom);
            WriteLine(Math.Round(avgSmartphoneRom, 0));
            WriteLine();

            // Quantifiers: Contains, Any, All, SequenceEqual
            WriteLine("If Windows System is present in Smartphones:");
            bool systemWindows = smartphones.Any(s => s.System == "Windows");
            WriteLine(systemWindows);
            WriteLine();

            // Generation methods: Empty, Repeat, Range
            WriteLine("generation methods");
            var greatings = Enumerable.Repeat("Hello, World", 10);

            foreach (var n in greatings)
            {
                Console.WriteLine(n);
            }
            WriteLine();

            var numbers = Enumerable.Range(0, 10)
                        .Select(n => new { Number = n , OddEven = n % 2 == 1 ? "odd" : "even" });

            foreach (var n in numbers)
            {
                Console.WriteLine("The Number: " + n.Number + " is " + n.OddEven);
            }
            WriteLine();
            // Closures

            var i = GiveMeAction();
            i();


            var resultGetAFunc = GetAFunc();
            
            WriteLine(resultGetAFunc(0)); //2
            WriteLine(resultGetAFunc(3)); //5
            ReadKey();
        }

        static Action GiveMeAction()
        {
            int i = 3;
            Action action = () => { WriteLine(i); };
            return action;
        }

        static Func<int, int> GetAFunc()
        {
            var myVar = 1;
            Func<int, int> inc = delegate(int var1)
            {
                myVar++;
                return var1 + myVar;

            };
            return inc;
        }

    }
}
