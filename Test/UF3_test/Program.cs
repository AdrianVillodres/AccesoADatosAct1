using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using UF3_test.connections;
using UF3_test.model;

namespace UF3_test
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            //GetAllDBs();         
            //GetCollections();
            //SelectAllStudents();
            //InsertOneStudent();
            //SelectOneStudent();
            SelectStudentFields();            
            //LoadPeopleCollection();
            //LoadBooksCollection();
            //LoadProductsCollection();
        }

        private static void GetAllDBs()
        {
            

            var dbClient = MongoLocalConnection.GetMongoClient();
            
            var dbList = dbClient.ListDatabases().ToList();
            Console.WriteLine("The list of databases on this server is: ");
            foreach (var db in dbList)
            {
                Console.WriteLine(db);
            }

        }

        private static void GetCollections()
        {
            
            var database = MongoLocalConnection.GetDatabase("sample_training");

            var colList = database.ListCollections().ToList();
            Console.WriteLine("The list of collection on this database is: ");
            foreach (var col in colList)
            {
                Console.WriteLine(col);
            }
        }

        private static void SelectAllStudents()
        {
          

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var studentDocuments = collection.Find(new BsonDocument()).ToList();

            foreach (var student in studentDocuments)
            {
                Console.WriteLine(student.ToString());
            }

        }

        private static void InsertOneStudent()
        {
           
            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var document = new BsonDocument
            {
                { "student_id", 9999923 },
                { "scores", new BsonArray
                {
                        new BsonDocument{ {"type", "exam"}, {"score", 88.12334193287023 } },
                        new BsonDocument{ {"type", "quiz"}, {"score", 74.92381029342834 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 89.97929384290324 } },
                        new BsonDocument{ {"type", "homework"}, {"score", 82.12931030513218 } }
                    }
                },
                { "class_id", 480}
            };


            collection.InsertOne(document);

        }

        private static void SelectOneStudent()
        {
            

            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            Console.WriteLine(studentDocument.ToString());

        }

     
        private static void SelectStudentFields()
        {
            
            var database = MongoLocalConnection.GetDatabase("sample_training");
            var collection = database.GetCollection<BsonDocument>("grades");

            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 9999923);
            var studentDocument = collection.Find(filter).FirstOrDefault();
            var id = studentDocument.GetElement("student_id");
            var scores = studentDocument.GetElement("scores");

            Console.WriteLine(id.ToString());
            Console.WriteLine(scores.ToString());
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
}