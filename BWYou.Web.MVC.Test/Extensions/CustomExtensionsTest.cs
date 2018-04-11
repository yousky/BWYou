using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using BWYou.Web.MVC.Models;
using BWYou.Web.MVC.Attributes;
using BWYou.Web.MVC.Extensions;
using BWYou.Web.MVC.Etc;

namespace BWYou.Web.MVC.Test.Extensions
{
    [TestFixture]
    class CustomExtensionsTest
    {
        class FilterableModel : BWModel<int?>
        {
            public int? Col1 { get; set; }
            [Filterable]
            public int? Col2 { get; set; }
            [Filterable(true)]
            public int? Col3 { get; set; }
            [Filterable(false)]
            public int? Col4 { get; set; }
        }

        [Test]
        public void GetWhereClause_FilterableModel_NotExistFilter()
        {
            // 정렬
            var model = new FilterableModel() { };

            // 동작

            // 어설션
            var ex = Assert.Throws<Exception>(delegate { model.GetWhereClause(); });
            Assert.That(ex.Message, Is.EqualTo("Not Exist Filter"));
        }
        [Test]
        public void GetWhereClause_FilterableModel_NotFilterableAttributeRequired()
        {
            // 정렬
            var model = new FilterableModel() { Col1 = 1 };

            // 동작
            var r = model.GetWhereClause(true, false);

            // 어설션
            Assert.AreEqual(r.ToString(), "t => (Convert(t.Col1) == 1)");
        }
        [Test]
        public void GetWhereClause_FilterableModel_NotIgnoreNull()
        {
            // 정렬
            var model = new FilterableModel() { Col1 = 1 };

            // 동작
            var r = model.GetWhereClause(false);

            // 어설션
            Assert.AreEqual(r.ToString(), "t => ((t.Col2 == null) AndAlso (t.Col3 == null))");
        }
        [Test]
        public void GetWhereClause_FilterableModel_NecessaryAddFields()
        {
            // 정렬
            var model = new FilterableModel() { Col1 = 1, Col4 = 4 };
            List<string> a = new List<string>();
            a.Add("Col4");


            // 동작
            var r = model.GetWhereClause(true, true, a);

            // 어설션
            Assert.AreEqual(r.ToString(), "t => (Convert(t.Col4) == 4)");
        }
        [Test]
        public void GetWhereClause_FilterableModel_ManualExpressionFilter()
        {
            // 정렬
            var model = new FilterableModel() { Col1 = 1, Col4 = 4 };
            List<ExpressionFilter> a = new List<ExpressionFilter>();
            a.Add(new ExpressionFilter("Col1", Op.GreaterThan, 3));
            a.Add(new ExpressionFilter("Col4", Op.LessThanOrEqual, 4));


            // 동작
            var r = model.GetWhereClause(a);

            // 어설션
            Assert.AreEqual(r.ToString(), "t => ((Convert(t.Col1) > 3) AndAlso (Convert(t.Col4) <= 4))");
        }
    }
}
