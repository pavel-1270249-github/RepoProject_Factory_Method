using RepoProject_Factory.Factories;
using RepoProject_Factory.Models;
using RepoProject_Factory.Repoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RepoProject_Factory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var manger = new RepositoryFactory();
            var ef = new EntityFactory();
            var repoB = manger.Create<Batch>();
            repoB.Add(ef.Create< Batch>(1, "CS/ACSL-M/52/01"));
            repoB.Add(ef.Create <Batch>(2, "GAVE/ACSL-M/53/01"));
            repoB.Add(ef.Create <Batch>(3, "CCO/ACSL-M/51/01"));
            Console.WriteLine("Read operation");
            repoB.GetAll()
                .ToList()
                .ForEach(b => Console.WriteLine(b));
            Console.WriteLine("Get batch with id 2");
            var bb = repoB.Get(x => x.Id == 2);
            Console.WriteLine(bb);
            Console.WriteLine("Update batch with id 2");
            repoB.Update(x => x.Id == 2, new Batch { Id = 2, BatchCode = "CS/ACSL-A/50/01" });
            repoB.GetAll()
            .ToList()
            .ForEach(b => Console.WriteLine(b));
            Console.WriteLine("Delete batch with id 2");
            repoB.Delete(x => x.Id == 2);

            repoB.GetAll()
            .ToList()
            .ForEach(t => Console.WriteLine(t));
            Console.WriteLine();
            var repoT = manger.Create<Trainee>();
            Trainee t1 = ef.Create< Trainee>(101, "Pavel", 1, true);
            Trainee t2 = ef.Create< Trainee>(102, "Sohel", 2, true);
            Trainee t3 = ef.Create< Trainee>(101, "Arman", 3, true);
            repoT.Add(t1);
            repoT.Add(t2);
            repoT.Add(t3);
            Console.WriteLine("Read operation");
            repoT.GetAll()
                .ToList()
                .ForEach(t => Console.WriteLine(t));
            Console.WriteLine("Get trainee with id 102");
            var tt = repoT.Get(x => x.Id == 102);
            Console.WriteLine(tt);
            Console.WriteLine("Update trainee with id 102");
            t2.Name = "Rahim";
            repoT.Update(x => x.Id == 102, t2);
            repoT.GetAll()
                .ToList()
                .ForEach(t => Console.WriteLine(t));
            Console.WriteLine("Delete trainee with id 102");
            repoT.Delete(x => x.Id == 102);
            repoT.GetAll()
                .ToList()
                .ForEach(t => Console.WriteLine(t));
            Console.ReadLine();
        }
    }
    namespace Models
    {
        public class Batch
        {
            public Batch() { }
            public Batch(int id, string batchCode)
            {
                this.Id = id;

                this.BatchCode = batchCode;

            }
            public int Id { get; set; }

            public string BatchCode { get; set; }

            public override string ToString()
            {
                return $"Id: {Id} Batch Code:{BatchCode}";
            }
        }
        public class Trainee
        {
            public Trainee() { }
            public Trainee(int id, string name, int batchId, bool isContinuing)
            {
                Id = id;
                Name = name;
                this.BatchId = batchId;
                this.IsContinuing = isContinuing;
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public int BatchId { get; set; }
            public bool IsContinuing { get; set; }
            public override string ToString()
            {
                return $"Id: {Id} Name: {Name} Batch Id: {BatchId} Continuing: {(IsContinuing ? "Yes" : "No")}";
            }
        }
    }
    namespace Repoes
    {
        public interface IRepository<T> where T : class, new()
        {
            IList<T> GetAll();
            T Get(Func<T, bool> expression);
            void Add(T entity);
            void Update(Func<T, bool> expression, T entity);
            void Delete(Func<T, bool> expression);
        }
        public class Repository<T> : IRepository<T> where T : class, new()
        {
            IList<T> list;
            public Repository(IList<T> list)
            {
                this.list = list;
            }
            public IList<T> GetAll()
            {
                return list;
            }
            public T Get(Func<T, bool> expression)
            {
                return this.list.FirstOrDefault(expression);

            }
            public void Add(T entity)
            {
                this.list.Add(entity);
            }
            public void Update(Func<T, bool> expression, T entity)
            {
                var item = this.list.FirstOrDefault(expression);
                if (item != null)
                {
                    var i = list.IndexOf(item);
                    list.RemoveAt(i);
                    list.Insert(i, entity);
                }
            }
            public void Delete(Func<T, bool> expression)
            {
                var item = this.list.FirstOrDefault(expression);
                if (item != null)
                {
                    var i = list.IndexOf(item);
                    list.RemoveAt(i);

                }
            }
        }
        
    }
    namespace Factories
    {
        public interface IEntityFactory
        {
            
            T Create<T>(params object[] args) where T : class, new();
        }
        public class EntityFactory : IEntityFactory
        {
           
            public T Create<T>(params object[] args) where T : class, new()
            {
                return Activator.CreateInstance(typeof(T), args) as T;
            }
        }
        public interface IRepositoryFactory
        {
            Repository<T> Create<T>() where T : class,  new();
        }
        public class RepositoryFactory : IRepositoryFactory
        {
            public Repository<T> Create<T>() where T : class,  new()
            {
                return Activator.CreateInstance(typeof(Repository<T>), new object[] { new List<T>() }) as Repository<T>;
            }
        }
    }
}
