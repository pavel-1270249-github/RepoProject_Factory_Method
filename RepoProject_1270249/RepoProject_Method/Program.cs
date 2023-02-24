using RepoProject_Method.Models;
using RepoProject_Method.Repoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoProject_Method
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositoryManger manger = new RepositoryManger();
            var repoB = manger.Get<Batch>();
            repoB.Add(new Batch(1, "CS/ACSL-M/52/01"));
            repoB.Add(new Batch(2, "GAVE/ACSL-M/53/01"));
            repoB.Add(new Batch(3, "CCO/ACSL-M/51/01"));
            Console.WriteLine("Read operation");
            repoB.GetAll()
                .ToList()
                .ForEach(b => Console.WriteLine(b));
            Console.WriteLine("Get batch with id 2");
            var bb = repoB.Get(x => x.Id == 2);
            Console.WriteLine(bb);
            Console.WriteLine("Update batch with id 2");
            repoB.Update(x => x.Id == 2, new Batch { Id = 2, BatchCode="CS/ACSL-A/50/01"});
            repoB.GetAll()
            .ToList()
            .ForEach(b => Console.WriteLine(b));
            Console.WriteLine("Delete batch with id 2");
            repoB.Delete(x => x.Id == 2);
           
            repoB.GetAll()
            .ToList()
            .ForEach(t => Console.WriteLine(t));
            Console.WriteLine();
            var repoT = manger.Get<Trainee>();
            Trainee t1 = new Trainee (101, "Pavel",1, true);
            Trainee t2 = new Trainee(102, "Sohel", 2, true); 
            Trainee t3 = new Trainee(101, "Arman", 3, true); 
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
                return $"Id: {Id} Name: {Name} Batch Id: {BatchId} Continuing: {(IsContinuing? "Yes": "No")}";
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
        public interface IRepositoryManger
        {
            Repository<T> Get<T>() where T : class, new();
        }
        public class RepositoryManger : IRepositoryManger
        {
            public Repository<T> Get<T>() where T : class, new()
            {
                return new Repository<T>(new List<T>());
            }
        }
    }
}

