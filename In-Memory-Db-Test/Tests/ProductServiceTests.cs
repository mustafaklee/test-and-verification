using In_Memory_Db_Test.Context;
using In_Memory_Db_Test.Entities;
using In_Memory_Db_Test.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace In_Memory_Db_Test.Tests
{
    public class ProductServiceTests
    {
        private readonly ITestOutputHelper _output;

        public ProductServiceTests(ITestOutputHelper output)
        {
            _output = output;
        }
        //In Memory Database Izolasyon Çözümü:
        //Bu sorunun temel nedeni, testler arasında bellek içi veri tabanının(In-Memory Database) paylaşılıyor olmasıdır.
        //Bu durumda bir test metodu, 
        //diğer testlerin bıraktığı verilerden 
        //etkilenir ve bu da beklenmeyen sonuçlara yol açabilir.
        //Her test için izole bir veri tabanı oluşturmak en kesin çözümdür.
        //UseInMemoryDatabase çağrısında her test için benzersiz
        //bir veritabanı adı vererek bunu kolayca sağlayabilirsiniz.

        private ProductService CreateProductService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var context = new AppDbContext(options);
            return new ProductService(context);
        }

        private Product generateRandomProduct()
        {
            Random rnd = new Random();
            return new Product { Name = $"Test Product-{rnd.Next(0, 100)}", Price = rnd.Next(0, 100) };

        }


        [Fact]
        public async Task AddProductAsync_Should_Add_Product_To_Database()
        {
            //arrange 
            var productService = CreateProductService();
            var product1 = new Product { Name = "Test Product-1", Price = 13 };

            //act
            await productService.AddProductAsync(product1);
            var allProducts = await productService.GetAllProductaAsync();

            //assert
            Assert.Single(allProducts);
            Assert.Equal(1, allProducts.Count);
            Assert.Equal(product1.Name, allProducts[0].Name);
            Assert.Equal(product1.Price, allProducts[0].Price);
        }


        [Fact]
        public async Task GetAllProductsAsync_Should_Return_All_Products()
        {
            //arrange 
            var productService = CreateProductService();
            var product1 = new Product { Name = "Test Product-1", Price = 12 };
            var product2 = new Product { Name = "Test Product-2", Price = 13 };


            //act
            await productService.AddProductAsync(product2);
            await productService.AddProductAsync(product1);
            var allProducts = await productService.GetAllProductaAsync();


            //assert
            Assert.Equal(2, allProducts.Count);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Correct_Product()
        {
            //arrange
            var productService = CreateProductService();
            var product1 = new Product { Name = "Test Product-1", Price = 12 };
            var product2 = new Product { Name = "Test Product-2", Price = 13 };


            //act
            await productService.AddProductAsync(product1);
            await productService.AddProductAsync(product2);
            var productById = productService.GetProductByIdAsync(product1.Id);

            //assert
            Assert.True(productById.Id == product1.Id);
            Assert.NotNull(productById);
        }


        [Fact]
        public async Task DeleteProductByIdAsync_Should_Delete_Product_If_Exists()
        {
            //arrange
            Product p1 = generateRandomProduct();
            ProductService productService = CreateProductService();


            //act
            await productService.AddProductAsync(p1);
            var result = await productService.DeleteProductByIdAsync(p1.Id);

            //assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetProductByIdAsync_Should_Return_Null_If_Not_Found()
        {
            //arrange 
            ProductService productService = CreateProductService();

            //act
            var result = await productService.GetProductByIdAsync(2);

            //assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductByIdAsync_Should_Return_False_If_Product_Not_Found()
        {
            //arrange 
            ProductService productService = CreateProductService();

            //act
            var result = await productService.DeleteProductByIdAsync(2);

            //assert
            Assert.False(result);

        }



}
}
