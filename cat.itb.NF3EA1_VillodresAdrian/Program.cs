using MongoDB.Bson;
using MongoDB.Driver;
using UF3_test.connections;
using UF3_test.model;
using Newtonsoft.Json;
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
            Console.WriteLine("7. Crea els mètodes per importar a la base de dades \"itb\" els fitxers JSON \"people.json\", \"books.json\",\r\n\"products.json\", \"restaurants.json\", \"students.json\", \"grades.json\" i \"countries.json\" . Has de crear les classes model\r\ncorresponents per cada col·lecció");
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
                    SelectStudentsByGrade();
                    Console.WriteLine("");
                    break;
                case "4":
                    Console.WriteLine("");

                    Console.WriteLine("");
                    break;
                case "5":
                    Console.WriteLine("");

                    Console.WriteLine("");
                    break;
                case "6":
                    Console.WriteLine("");

                    Console.WriteLine("");
                    break;
                case "7":
                    Console.WriteLine("");
                    LoadBooksCollection();
                    LoadPeopleCollection();
                    LoadProductsCollection();
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


    private static void SelectStudentsByGrade()
    {
    
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


}