using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.Controllers;
using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.ViewModels;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BWYou.Web.MVC.Test.Controllers
{
    [TestFixture]
    class BWLongApiControllerTest
    {
        public class Product : BWLongModel
        {
            [Filterable]
            public string Name { get; set; }
            public string NotFilterName { get; set; }
        }

        public class TestContext : DbContext
        {
            public TestContext(string nameOrConnectionString)
                : base(nameOrConnectionString)
            {
            }

            public DbSet<Product> Products { get; set; }
        }

        private class ProductApiController : BWLongApiController<Product>
        {
            public ProductApiController(DbContext dbContext)
                : base(dbContext)
            {

            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetListAsync()
            {
                return base.BaseGetListAsync();
            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetListAsync(string sort)
            {
                return base.BaseGetListAsync(sort);
            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetListAsync(int page, string sort)
            {
                return base.BaseGetListAsync(page, sort);
            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetFilteredListAsync(Product searchModel)
            {
                return base.BaseGetFilteredListAsync(searchModel);
            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetFilteredListAsync(Product searchModel, string sort)
            {
                return base.BaseGetFilteredListAsync(searchModel, sort);
            }

            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetFilteredListAsync(Product searchModel, int page, string sort)
            {
                return base.BaseGetFilteredListAsync(searchModel, page, sort);
            }
        }


        [OneTimeSetUp]
        public void Initialize()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            //XXX 제대로 한다면 Mock 서비스와 그 하위 객체들을 모두 Mock으로 만들어서 테스트 해야 함... 어렵...  
            using (var context = new TestContext("TestDBContext"))
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                context.Products.AddRange(new List<Product>{
                         new Product
                         {
                             Id = 1,
                             Name = "test1",
                             NotFilterName = "test1"
                         },
                         new Product
                         {
                             Id = 2,
                             Name = "test2",
                             NotFilterName = "test2"
                         },
                         new Product
                         {
                             Id = 3,
                             Name = "test3",
                             NotFilterName = "test3"
                         },
                         new Product
                         {
                             Id = 4,
                             Name = "test2",
                             NotFilterName = "test2"
                         },
                    });
                context.SaveChanges();
            }
        }

        [Test]
        public async Task BaseGetListAsync()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // 동작
            var result = await controller.BaseGetListAsync();
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 3);
        }
        [Test]
        public async Task BaseGetListAsyncWithSort()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            string sort = "-Id";

            // 동작
            var result = await controller.BaseGetListAsync(sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 3);
            Assert.AreEqual(products.Last().Id, 1);
        }
        [Test]
        public void BaseGetListAsyncWithNoSort()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            string sort = "-NoId";

            // 동작

            // 어설션
            var ex = Assert.ThrowsAsync<System.ArgumentNullException>(async delegate { await controller.BaseGetListAsync(sort); });
            Assert.That(ex.ParamName, Is.EqualTo("property"));
        }
        [Test]
        public async Task BaseGetListAsyncWithPageSort()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            int page = 1;
            string sort = "-Id";

            // 동작
            var result = await controller.BaseGetListAsync(page, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<PageResultViewModel<Product>>(content);

            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Result.Count(), 3);
            Assert.AreEqual(products.Result.Last().Id, 1);
            Assert.AreEqual(products.MetaData.IsFirstPage, true);
        }
        [Test]
        public async Task BaseGetFilteredListAsync()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "test2" };

            // 동작
            var result = await controller.BaseGetFilteredListAsync(searchModel);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 1);
            Assert.AreEqual(products.First().Id, 2);
        }
        [Test]
        public void BaseGetFilteredListAsyncWithNotFilterable()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { NotFilterName = "test2" };

            // 동작

            // 어설션
            var ex = Assert.ThrowsAsync<Exception>(async delegate { await controller.BaseGetFilteredListAsync(searchModel); });
            Assert.That(ex.Message, Is.EqualTo("Not Exist Filter"));
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithSort()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "test2" };
            string sort = "-Id";

            // 동작
            var result = await controller.BaseGetFilteredListAsync(searchModel, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 2);
            Assert.AreEqual(products.Last().Id, 2);
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithPageSort()
        {
            // 정렬
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "test2" };
            int page = 1;
            string sort = "-Id";

            // 동작
            var result = await controller.BaseGetFilteredListAsync(searchModel, page, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<PageResultViewModel<Product>>(content);
            
            // 어설션
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Result.Count(), 2);
            Assert.AreEqual(products.Result.Last().Id, 2);
            Assert.AreEqual(products.MetaData.IsFirstPage, true);
        }
    }
}
