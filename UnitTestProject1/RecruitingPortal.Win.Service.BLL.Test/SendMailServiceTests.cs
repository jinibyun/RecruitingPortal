using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using RecruitingPortal.Domain;
using RecruitingPortal.Win.Service.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestProject1;

namespace UnitTestProject1.RecruitingPortal.Win.Service.BLL.Test
{
    [TestClass()]
    public class SendMailServiceTests
    {
        private ISendMailService Service { get; set; }
        [TestInitialize]
        public void TestInitialize()
        {
            var kernel = new StandardKernel(new DerivedNinjectModule());
            Service = kernel.Get<ISendMailService>();
        }

        [TestMethod()]
        public void SendMailTest()
        {
            // arrange
            // act                      
            // assert
            try
            {
                Service.SendMail();                
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false, ex.Message);
            }
        }
    }
}