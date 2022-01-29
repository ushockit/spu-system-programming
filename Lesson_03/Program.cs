using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lesson_03
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class ProductsContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public string DbPath { get; }

        public ProductsContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Combine(path, "blogging.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }


    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main start");

            // async / await
            // Ex01();
            CancellationTokenSource source = new CancellationTokenSource();

            Ex02(source);

            Thread.Sleep(1500);
            source.Cancel();

            Console.WriteLine("Main end");
            Console.Read();
        }

        public static async void Ex01()
        {
            using (var ctx = new ProductsContext())
            {
                var products = await ctx.Products.ToListAsync();
                ;
            }
        }

        public static async void Ex02(CancellationTokenSource source)
        {
            Console.WriteLine("Ex02 start");

            await SaveDataAsync(source.Token);

            Console.WriteLine("Ex02 end");
        }

        private static Task SaveDataAsync(CancellationToken cancellationToken = default)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Start saving");
                for (int i = 0; i < 10; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                    Console.WriteLine($"Saving: {(i + 1) * 10}%");
                    Thread.Sleep(250);
                }
                Console.WriteLine("End saving");
            });
        }
    }
}
