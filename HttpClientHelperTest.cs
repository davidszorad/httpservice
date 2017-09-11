using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace HttpClient
{
    [TestFixture]
    public class HttpClientHelperTest
    {
        private HttpClientHelper sut;

        [SetUp]
        public void SetUp()
        {
            sut = new HttpClientHelper();
        }

        [Test]
        public void GetHttpClient_GetCorrectData_ReturnsClientWithCorrectConfiguration()
        {
            //Arrange
            var url = "https://SomeUrl.com";
            var key = "SomeKey";

            //Act
            var client = sut.GetHttpClient(url, key);

            //Assert
            Assert.AreEqual(client.BaseAddress, new Uri(url));
            Assert.AreEqual(client.DefaultRequestHeaders.GetValues("Ocp-Apim-Subscription-Key").FirstOrDefault(), key);
        }

        [Test]
        public void GetHttpClient_GetEmptyKey_ThrowsArgumentExcpetion()
        {
            //Arrange
            var url = "https://SomeUrl.com";
            var key = "";

            //Assert
            Assert.That(() => sut.GetHttpClient(url, key), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GetHttpClient_GetEmptyUrl_ThrowsArgumentExcpetion()
        {
            //Arrange
            var url = "";
            var key = "SomeKey";

            //Assert
            Assert.That(() => sut.GetHttpClient(url, key), Throws.TypeOf<ArgumentException>());
        }
    }
}
