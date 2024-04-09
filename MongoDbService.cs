using MongoDB.Driver;
using System.Collections.Generic;

namespace SafeDevelopment
{
    public class MongoDbService
    {
        private readonly IMongoCollection<Book> _books;

        public MongoDbService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _books = database.GetCollection<Book>(collectionName);
        }

        public Book AddBook(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public List<Book> GetBooks()
        {
            return _books.Find(book => true).ToList();
        }

        public Book GetBook(ObjectId id)
        {
            return _books.Find(book => book.Id == id).FirstOrDefault();
        }

        public void UpdateBook(Book book)
        {
            _books.ReplaceOne(book => book.Id == book.Id, book);
        }

        public void DeleteBook(ObjectId id)
        {
            _books.DeleteOne(book => book.Id == id);
        }
    }
}
