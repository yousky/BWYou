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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BWYou.Web.MVC.Test.Controllers
{
    [TestFixture]
    class BWLongApiControllerTest
    {
        public class ProductModel : BWLongModel
        {
            [Required]
            [Filterable]
            [Updatable]
            public string Name { get; set; }

            public string NotFilterName { get; set; }

            [JsonIgnore]
            [CascadeRelation(CascadeRelationAttribute.CascadeDirection.Down)]
            public virtual ICollection<ProductSpec> ProductSpecs { get; set; }
        }
        public class Product : ProductModel
        {
        }
        public class ProductSpecModel : BWLongModel
        {
            [ForeignKey("Product")]
            public long? ProductId { get; set; }

            [Required]
            [Filterable]
            [Updatable]
            public string Name { get; set; }

            [JsonIgnore]
            [CascadeRelation(CascadeRelationAttribute.CascadeDirection.Up)]
            public virtual Product Product { get; set; }
        }
        public class ProductSpec : ProductSpecModel
        {
        }

        public class TestContext : DbContext
        {
            public TestContext(string nameOrConnectionString)
                : base(nameOrConnectionString)
            {
#if DEBUG
                this.Database.Log += m => System.Diagnostics.Debug.WriteLine(m);
#endif
                this.Configuration.LazyLoadingEnabled = false;
            }

            public virtual DbSet<Product> Products { get; set; }
            public virtual DbSet<ProductSpec> ProductSpecs { get; set; }

            //protected override void OnModelCreating(DbModelBuilder modelBuilder)
            //{
            //    modelBuilder.Entity<Product>().HasMany(s => s.ProductSpecs).WithOne(s => s.);
            //    base.OnModelCreating(modelBuilder);
            //}
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
            internal new Task<System.Net.Http.HttpResponseMessage> BaseGetFilteredListAsync(Product searchModel, string sort, string limitBaseColName, long? after, long? before, int limit)
            {
                return base.BaseGetFilteredListAsync(searchModel, sort, limitBaseColName, after, before, limit);
            }
            internal new Task<HttpResponseMessage> BaseGetAsync(long? id)
            {
                return base.BaseGetAsync(id);
            }
            internal new Task<HttpResponseMessage> BasePostAsync(Product model)
            {
                return base.BasePostAsync(model);
            }
            internal new Task<HttpResponseMessage> BasePostAsync(IEnumerable<Product> models)
            {
                return base.BasePostAsync(models);
            }

            internal new Task<HttpResponseMessage> BasePutAsync(long? id, Product model)
            {
                return base.BasePutAsync(id, model);
            }
            internal new Task<HttpResponseMessage> BaseDeleteAsync(long? id)
            {
                return base.BaseDeleteAsync(id);
            }
            internal new Task<HttpResponseMessage> BaseDeleteAsync(IEnumerable<long?> ids)
            {
                return base.BaseDeleteAsync(ids);
            }
            internal new Task<HttpResponseMessage> BaseCloneAsync(long? id)
            {
                return base.BaseCloneAsync(id);
            }
        }


        [OneTimeSetUp]
        public void Initialize()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            using (var context = new TestContext("TestDBContext"))
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                //Id는 참고만 해야 할 듯. 스스로 알아서 만드는데?
                context.Products.AddRange(new List<Product>{
                         new Product
                         {
                             Id = 1,
                             Name = "List1",
                             NotFilterName = "List1"
                         },
                         new Product
                         {
                             Id = 2,
                             Name = "List2",
                             NotFilterName = "List2"
                         },
                         new Product
                         {
                             Id = 3,
                             Name = "SearchTest",
                             NotFilterName = "SearchTest"
                         },
                         new Product
                         {
                             Id = 4,
                             Name = "SearchTest",
                             NotFilterName = "SearchTest"
                         },
                         new Product
                         {
                             Id = 5,
                             Name = "CloneTest1",
                             NotFilterName = "CloneTest1",
                             ProductSpecs = new List<ProductSpec>{
                                 new ProductSpec{
                                     Name = "ps5_1"
                                 },
                                 new ProductSpec{
                                     Name = "ps5_2"
                                 }
                             }
                         },
                         new Product
                         {
                             Id = 6,
                             Name = "CloneTest2",
                             NotFilterName = "CloneTest2",
                             ProductSpecs = new List<ProductSpec>{
                                 new ProductSpec{
                                     Name = "ps6_1"
                                 },
                                 new ProductSpec{
                                     Name = "ps6_2"
                                 }
                             }
                         },
                         new Product
                         {
                             Id = 7,
                             Name = "UpdateTest",
                             NotFilterName = "UpdateTest"
                         },
                         new Product
                         {
                             Id = 8,
                             Name = "DeleteTest",
                             NotFilterName = "DeleteTest"
                         },
                         new Product
                         {
                             Id = 9,
                             Name = "DeleteListTest",
                             NotFilterName = "DeleteListTest"
                         },
                         new Product
                         {
                             Id = 10,
                             Name = "DeleteListTest",
                             NotFilterName = "DeleteListTest"
                         },
                         new Product
                         {
                             Id = 11,
                             Name = "DeleteListRecurTest",
                             NotFilterName = "DeleteListRecurTest",
                             ProductSpecs = new List<ProductSpec>{
                                 new ProductSpec{
                                     Name = "ps11_1"
                                 },
                                 new ProductSpec{
                                     Name = "ps11_2"
                                 }
                             }
                         },
                         new Product
                         {
                             Id = 12,
                             Name = "DeleteListRecurTest",
                             NotFilterName = "DeleteListRecurTest",
                             ProductSpecs = new List<ProductSpec>{
                                 new ProductSpec{
                                     Name = "ps12_1"
                                 },
                                 new ProductSpec{
                                     Name = "ps12_2"
                                 }
                             }
                         },
                         new Product
                         {
                             Id = 13,
                             Name = "InfScllList",
                             NotFilterName = "InfScllList1"
                         },
                         new Product
                         {
                             Id = 14,
                             Name = "InfScllList",
                             NotFilterName = "InfScllList2"
                         },
                         new Product
                         {
                             Id = 15,
                             Name = "InfScllList",
                             NotFilterName = "InfScllList3"
                         },
                         new Product
                         {
                             Id = 16,
                             Name = "InfScllList",
                             NotFilterName = "InfScllList4"
                         },
                    });
                context.SaveChanges();
            }
        }
        public static HttpContext FakeHttpContext()
        {
            var httpRequest = new HttpRequest("", "http://stackoverflow/", "");
            var stringWriter = new StringWriter();
            var httpResponse = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponse);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                    new HttpStaticObjectsCollection(), 10, true,
                                                    HttpCookieMode.AutoDetect,
                                                    SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                        BindingFlags.NonPublic | BindingFlags.Instance,
                                        null, CallingConventions.Standard,
                                        new[] { typeof(HttpSessionStateContainer) },
                                        null)
                                .Invoke(new object[] { sessionContainer });


            return httpContext;
        }

        [Test]
        public async Task BaseGetListAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            var result = await controller.BaseGetListAsync();
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 2);
        }
        [Test]
        public async Task BaseGetListAsyncWithSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            string sort = "-Id";

            // Act
            var result = await controller.BaseGetListAsync(sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 2);
            Assert.AreEqual(products.Last().Id, 1);
        }
        [Test]
        public void BaseGetListAsyncWithNoSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            string sort = "-NoId";

            // Act

            // Assert
            var ex = Assert.ThrowsAsync<System.ArgumentNullException>(async delegate { await controller.BaseGetListAsync(sort); });
            Assert.That(ex.ParamName, Is.EqualTo("property"));
        }
        [Test]
        public async Task BaseGetListAsyncWithPageSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            int page = 1;
            string sort = "-Id";

            // Act
            var result = await controller.BaseGetListAsync(page, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<PageResultViewModel<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Result.Count(), 3);
            Assert.AreEqual(products.Result.Last().Id, 1);
            Assert.AreEqual(products.MetaData.IsFirstPage, true);
        }
        [Test]
        public async Task BaseGetFilteredListAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "SearchTest" };

            // Act
            var result = await controller.BaseGetFilteredListAsync(searchModel);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 2);
            Assert.AreEqual(products.First().Id, 3);
        }
        [Test]
        public void BaseGetFilteredListAsyncWithNotFilterable()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { NotFilterName = "SearchTest" };

            // Act

            // Assert
            var ex = Assert.ThrowsAsync<Exception>(async delegate { await controller.BaseGetFilteredListAsync(searchModel); });
            Assert.That(ex.Message, Is.EqualTo("Not Exist Filter"));
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "SearchTest" };
            string sort = "-Id";

            // Act
            var result = await controller.BaseGetFilteredListAsync(searchModel, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Count(), 2);
            Assert.AreEqual(products.Last().Id, 3);
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithPageSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "SearchTest" };
            int page = 1;
            string sort = "-Id";

            // Act
            var result = await controller.BaseGetFilteredListAsync(searchModel, page, sort);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<PageResultViewModel<Product>>(content);
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.GreaterOrEqual(products.Result.Count(), 2);
            Assert.AreEqual(products.Result.Last().Id, 3);
            Assert.AreEqual(products.MetaData.IsFirstPage, true);
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithInfiniteScrollSort()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "InfScllList" };
            string sort = "-Id";
            string limitBaseColName = "Id";
            long? after = 13;
            long? before = 16;
            int limit = 2;

            // Act
            var result = await controller.BaseGetFilteredListAsync(searchModel, sort, limitBaseColName, after, before, limit);
            var content = await result.Content.ReadAsStringAsync();
            var crvm = JsonConvert.DeserializeObject<CursorResultViewModel<Product, long?>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(crvm.Result.Count(), 2);
            Assert.AreEqual(crvm.Result.Last().Id, 14);
            Assert.AreEqual(crvm.Result.First().Id, 15);
            Assert.GreaterOrEqual(crvm.MetaData.TotalUnlimitItemCount, 2);
            Assert.AreEqual(crvm.MetaData.Before, 14);
            Assert.AreEqual(crvm.MetaData.After, 15);
            Assert.AreEqual(crvm.MetaData.Limit, limit);
            Assert.AreEqual(crvm.MetaData.IsDescending, true);
            Assert.AreEqual(crvm.MetaData.IsRemaining, false);
        }
        [Test]
        public async Task BaseGetFilteredListAsyncWithInfiniteScrollSortWhenNullValue()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            Product searchModel = new Product() { Name = "InfScllList" };
            string sort = "Id";
            string limitBaseColName = "Id";
            long? after = 13;
            long? before = null;
            int limit = 2;

            // Act
            var result = await controller.BaseGetFilteredListAsync(searchModel, sort, limitBaseColName, after, before, limit);
            var content = await result.Content.ReadAsStringAsync();
            var crvm = JsonConvert.DeserializeObject<CursorResultViewModel<Product, long?>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(crvm.Result.Count(), 2);
            Assert.AreEqual(crvm.Result.Last().Id, 15);
            Assert.AreEqual(crvm.Result.First().Id, 14);
            Assert.GreaterOrEqual(crvm.MetaData.TotalUnlimitItemCount, 3);
            Assert.AreEqual(crvm.MetaData.Before, 14);
            Assert.AreEqual(crvm.MetaData.After, 15);
            Assert.AreEqual(crvm.MetaData.Limit, limit);
            Assert.AreEqual(crvm.MetaData.IsDescending, false);
            Assert.AreEqual(crvm.MetaData.IsRemaining, true);
        }
        [Test]
        public async Task BaseGetAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            long? id = 1;

            // Act
            var result = await controller.BaseGetAsync(id);
            var content = await result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(product.Id, id);
        }
        [Test]
        public async Task BaseGetAsyncWithNullId()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            long? id = null;

            // Act
            var result = await controller.BaseGetAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound);
        }
        [Test]
        public async Task BasePostAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();


            Product model = new Product() { Name = "newProduct" };

            // Act
            var result = await controller.BasePostAsync(model);
            var content = await result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(product.Name, model.Name);
        }
        [Test]
        public async Task BasePostAsyncWithNotValid()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();


            Product model = new Product() { Name = "" };

            // Act
            var result = await controller.BasePostAsync(model);
            var content = await result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<ErrorResultViewModel>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(vm.ModelValidResult.First().ObjectName, "Name");
        }
        [Test]
        public async Task BasePostAsyncArray()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            var models = new List<Product>();
            Product model1 = new Product() { Name = "newProduct1" };
            Product model2 = new Product() { Name = "newProduct2" };
            models.Add(model1);
            models.Add(model2);

            // Act
            var result = await controller.BasePostAsync(models);
            var content = await result.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Created);
            Assert.GreaterOrEqual(products.Count(), 2);
        }
        [Test]
        public async Task BasePostAsyncArrayWithNotValid()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            var models = new List<Product>();
            Product model1 = new Product() { Name = "newProduct1" };
            Product model2 = new Product() { Name = "" };
            models.Add(model1);
            models.Add(model2);

            // Act
            var result = await controller.BasePostAsync(models);
            var content = await result.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<ErrorResultViewModel>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(vm.ModelValidResult.First().ObjectName, "[1]Name");
        }
        [Test]
        public async Task BasePutAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            long? id = 7;
            Product model = new Product() { Name = "updateProduct" };

            // Act
            var result = await controller.BasePutAsync(id, model);
            var content = await result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(product.Name, model.Name);
            //Assert.Greater(product.UpdateDT, product.CreateDT);   //BWIdentityDbContext has responsibility for UpdateDT changes.
        }
        [Test]
        public async Task BaseDeleteAsync()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            long? id = 8;

            // Act
            var result = await controller.BaseDeleteAsync(id);
            var content = await result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

        }
        [Test]
        public async Task BaseDeleteAsyncArray()
        {
            // Arrange
            var controller = new ProductApiController(new TestContext("TestDBContext"));
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            var ids = new List<long?>();
            ids.Add(9);
            ids.Add(10);

            // Act
            var result = await controller.BaseDeleteAsync(ids);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task BaseDeleteAsyncArrayRecur()
        {
            // Arrange
            var context = new TestContext("TestDBContext");
            var controller = new ProductApiController(context);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            var ids = new List<long?>();
            ids.Add(11);
            ids.Add(12);

            // Act
            var a = context.Products.Include(p => p.ProductSpecs).Where(w => ids.Contains(w.Id)).ToList();  //In unit testing, lazy loading is not done. Lazy loading is forced by eager loading.
            var result = await controller.BaseDeleteAsync(ids);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task BaseCloneAsync()
        {
            // Arrange
            var context = new TestContext("TestDBContext");
            var controller = new ProductApiController(context);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());
            HttpContext.Current = FakeHttpContext();

            long? id = 5;

            // Act
            var a = context.Products.Include(p => p.ProductSpecs).Where(w => w.Id == id).First();  //In unit testing, lazy loading is not done. Lazy loading is forced by eager loading.
            var aProductSpecsCount = a.ProductSpecs.Count;
            var result = await controller.BaseCloneAsync(id);
            var content = await result.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Product>(content);
            var b = context.Products.Include(p => p.ProductSpecs).Where(w => w.Id == product.Id).First();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.AreNotEqual(product.Id, id);
            Assert.IsNotNull(b.ProductSpecs);
            Assert.AreEqual(aProductSpecsCount, b.ProductSpecs.Count);    //Child element clone check
        }
    }
}
