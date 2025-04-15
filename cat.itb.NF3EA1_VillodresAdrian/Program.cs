using MongoDB.Bson;
using MongoDB.Driver;
using UF3_test.connections;
using UF3_test.model;
using Newtonsoft.Json;
using cat.itb.NF3EA1_VillodresAdrian.Model;
class Program
{
    static void Main(string[] args)
    {

        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Insertar tres estudiants més a la col·lecció grades");
            Console.WriteLine("2. Mostra totes les dades dels estudiants del grup DAMv1.");
            Console.WriteLine("3. Mostra totes les dades dels estudiants que tenen un 90 al exam.");
            Console.WriteLine("4. Mostra totes les dades dels estudiants que tenen més d’un 99 al quiz.");
            Console.WriteLine("5. Mostra només els interessos del l’estudiant amb student_id=888666333.");
            Console.WriteLine("6. Mostra només el nom i el cognom de l’estudiant amb student_id=444777888.");
            Console.WriteLine("7. Importar people.json");
            Console.WriteLine("8. Importar books.json");
            Console.WriteLine("9. Importar products.json");
            Console.WriteLine("10. Importar restaurants.json");
            Console.WriteLine("11. Importar students.json");
            Console.WriteLine("12. Importar grades.json");
            Console.WriteLine("13. Importar countries.json");
            Console.WriteLine("0. Exit");
            Console.Write("Option: ");


            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("");
                    InsertStudents();
                    Console.WriteLine("");
                    break;
                case "2":
                    Console.WriteLine("");
                    SelectStudentsInClass();
                    Console.WriteLine("");
                    break;
                case "3":
                    Console.WriteLine("");
                    SelectStudentsByGrade(90);
                    Console.WriteLine("");
                    break;
                case "4":
                    Console.WriteLine("");
                    SelectStudentsMoreThan(99.0f);
                    Console.WriteLine("");
                    break;
                case "5":
                    Console.WriteLine("");
                    SelectStudentById(888666333);
                    Console.WriteLine("");
                    break;
                case "6":
                    Console.WriteLine("");
                    SelectStudentNameById(444777888);
                    Console.WriteLine("");
                    break;
                case "7":
                    Console.WriteLine("");
                    LoadBooksCollection();                    
                    Console.WriteLine("");
                    break;
                case "8":
                    Console.WriteLine("");
                    LoadPeopleCollection();
                    Console.WriteLine("");
                    break;
                case "9":
                    Console.WriteLine("");
                    LoadProductsCollection();
                    Console.WriteLine("");
                    break;
                case "10":
                    Console.WriteLine("");
                    LoadCountriesCollection();
                    Console.WriteLine("");
                    break;
                case "11":
                    Console.WriteLine("");
                    LoadGradesCollection();
                    Console.WriteLine("");
                    break;
                case "12":
                    Console.WriteLine("");
                    LoadRestaurantsCollection();
                    Console.WriteLine("");
                    break;
                case "13":
                    Console.WriteLine("");
                    LoadStudentsCollection();
                    Console.WriteLine("");
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida, intente de nuevo.");
                    break;
            }
        }
    }

    private static void InsertStudents()
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");

        var students = new List<BsonDocument>
    {
        new BsonDocument
        {
            { "student_id", 222333999 },
            { "name", "Adrian" },
            { "surname", "Villodres Fernandez" },
            { "class_id", 555 },
            { "group", "Damv1" },
            { "scores", new BsonArray
                {
                    new BsonDocument { { "type", "exam" }, { "score", 100 } },
                    new BsonDocument { { "type", "teamWork" }, { "score", 50.00 } }
                }
            }
        },
        new BsonDocument
        {
            { "student_id", 444777888 },
            { "name", "Carlos" },
            { "surname", "González Pérez" },
            { "class_id", 555 },
            { "group", "Damv1" },
            { "interests", new BsonArray { "music", "gym", "code", "electronics" } }
        },

        new BsonDocument
        {
            { "student_id", 888666333 },
            { "name", "Laura" },
            { "surname", "Martínez López" },
            { "class_id", 555 },
            { "group", "Damv1" },
            { "interests", new BsonArray { "rap", "runner", "movies", "comic" } },
            { "scores", new BsonArray
                {
                    new BsonDocument { { "type", "exam" }, { "score", 90 } },
                    new BsonDocument { { "type", "teamWork" }, { "score", 60 } },
                    new BsonDocument { { "type", "quiz" }, { "score", 96 } },
                    new BsonDocument { { "type", "teamWork" }, { "homework", 23 } }
                }
            }
        }
    };

        collection.InsertMany(students);

        Console.WriteLine("Estudiants insertats correctament.");
    }


    private static void SelectStudentsInClass()
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");
        var filter = Builders<BsonDocument>.Filter.Eq("group", "Damv1");
        var studentDocuments = collection.Find(filter).ToList();

        foreach (var student in studentDocuments)
        {
            Console.WriteLine(student.ToString());
        }
    }


    private static void SelectStudentsByGrade(int grade)
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");
        var filter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("scores", new BsonDocument { { "type", "exam" }, { "score", new BsonDocument { { "$eq", grade } } } });
        var exam = collection.Find(filter).ToList();
        foreach(var student in exam)
        {
            Console.WriteLine(student.ToString());
        }
    }


    private static void SelectStudentsMoreThan(float grade)
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");
        var filter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("scores", new BsonDocument { { "type", "quiz" }, { "score", new BsonDocument { { "$gt", grade } } } });
        var exam = collection.Find(filter).ToList();
        foreach (var student in exam)
        {
            Console.WriteLine(student.ToString());
        }
    }

    private static void SelectStudentById(int id)
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");
        var filter = Builders<BsonDocument>.Filter.Eq("student_id", id);
        var student = collection.Find(filter).FirstOrDefault();
        var studentinterests = student.GetElement("interests");
        Console.WriteLine(studentinterests.ToString());
    }

    private static void SelectStudentNameById(int id)
    {
        var database = MongoLocalConnection.GetDatabase("sample_training");
        var collection = database.GetCollection<BsonDocument>("grades");
        var filter = Builders<BsonDocument>.Filter.Eq("student_id", id);
        var student = collection.Find(filter).FirstOrDefault();
        var studentname = student.GetElement("name");
        var studentsurname = student.GetElement("surname");
        Console.WriteLine(studentname.ToString() + " " + studentsurname.ToString());
    }

    private static void LoadPeopleCollection()
    {
        FileInfo file = new FileInfo("../../../files/people.json");
        StreamReader sr = file.OpenText();
        string fileString = sr.ReadToEnd();
        sr.Close();
        List<Person> people = JsonConvert.DeserializeObject<List<Person>>(fileString);

        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("people");
        var collection = database.GetCollection<BsonDocument>("people");

        if (people != null)
            foreach (var person in people)
            {
                Console.WriteLine(person.name);
                string json = JsonConvert.SerializeObject(person);
                var document = new BsonDocument();
                document.Add(BsonDocument.Parse(json));
                collection.InsertOne(document);
            }
    }

    private static void LoadBooksCollection()
    {
        FileInfo file = new FileInfo("../../../files/books.json");
        StreamReader sr = file.OpenText();
        string fileString = sr.ReadToEnd();
        sr.Close();
        List<Book> books = JsonConvert.DeserializeObject<List<Book>>(fileString);

        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("books");
        var collection = database.GetCollection<BsonDocument>("books");

        if (books != null)
            foreach (var book in books)
            {
                Console.WriteLine(book.title);
                string json = JsonConvert.SerializeObject(book);
                var document = new BsonDocument();
                document.Add(BsonDocument.Parse(json));
                collection.InsertOne(document);
            }
    }

    private static void LoadProductsCollection()
    {

        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("products");
        var collection = database.GetCollection<BsonDocument>("products");

        FileInfo file = new FileInfo("../../../files/products.json");

        using (StreamReader sr = file.OpenText())
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Product product = JsonConvert.DeserializeObject<Product>(line);
                Console.WriteLine(product.name);
                string json = JsonConvert.SerializeObject(product);
                var document = new BsonDocument();
                document.Add(BsonDocument.Parse(json));
                collection.InsertOne(document);
            }
        }
    }

    private static void LoadCountriesCollection()
    {
        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("countries");
        var collection = database.GetCollection<BsonDocument>("countries");

        FileInfo file = new FileInfo("../../../files/countries.json");

        using (StreamReader sr = file.OpenText())
        {
            string jsonContent = sr.ReadToEnd();
            List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(jsonContent);

            foreach (var country in countries)
            {
                Console.WriteLine(country.name);
                string json = JsonConvert.SerializeObject(country);
                var document = new BsonDocument();
                document.Add(BsonDocument.Parse(json));
                collection.InsertOne(document);
            }
        }
    }

    private static void LoadGradesCollection()
    {
        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("grades");
        var collection = database.GetCollection<BsonDocument>("grades");

        FileInfo file = new FileInfo("../../../files/grades.json");

        using (StreamReader sr = file.OpenText())
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Grade grades = JsonConvert.DeserializeObject<Grade>(line);
                if (grades._id != null && !string.IsNullOrEmpty(grades._id.oid))
                {
                    Console.WriteLine(grades._id.oid);
                }
                string json = JsonConvert.SerializeObject(grades);
                var document = BsonDocument.Parse(json);
                document.Remove("_id");
                collection.InsertOne(document);
            }
        }
    }

    private static void LoadRestaurantsCollection()
    {
        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("restaurants");
        var collection = database.GetCollection<BsonDocument>("restaurants");

        FileInfo file = new FileInfo("../../../files/restaurants.json");

        using (StreamReader sr = file.OpenText())
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Restaurant restaurants = JsonConvert.DeserializeObject<Restaurant>(line);
                if (restaurants.name != null && !string.IsNullOrEmpty(restaurants.name))
                {
                    Console.WriteLine(restaurants.name);
                }
                string json = JsonConvert.SerializeObject(restaurants);
                var document = new BsonDocument();
                document = BsonDocument.Parse(json);
                collection.InsertOne(document);
            }
        }
    }


    private static void LoadStudentsCollection()
    {
        var database = MongoLocalConnection.GetDatabase("itb");
        database.DropCollection("students");
        var collection = database.GetCollection<BsonDocument>("students");

        FileInfo file = new FileInfo("../../../files/students.json");

        using (StreamReader sr = file.OpenText())
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Student students = JsonConvert.DeserializeObject<Student>(line);
                if (students.firstname != null && !string.IsNullOrEmpty(students.firstname))
                {
                    Console.WriteLine(students.firstname);
                }
                string json = JsonConvert.SerializeObject(students);
                var document = BsonDocument.Parse(json);
                document = BsonDocument.Parse(json);
                collection.InsertOne(document);
            }
        }
    }

}