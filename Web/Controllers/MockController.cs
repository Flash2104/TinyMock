using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TinyMock;

namespace Web.Controllers
{
    public class MockController : BaseWebController
    {
        [HttpPost]
        public ActionResult Execute(string text)
        {
            var collection = new MockCollection(text);
            var mocks = collection.Get();
            List<MockModel> result = new List<MockModel>();
            foreach (var mock in mocks)
            {
                var m = mock.Get();
                var executor = new MockExecutor(m);
                result = executor.PerformAction();
            }
            return JsonSuccess(result);
        }

        [HttpPost]
        public ActionResult ExecuteVoice(string fileName)
        {
            var filePath = Path.Combine(Path.GetDirectoryName(MockApp.tdb), fileName);
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var collection = new MockCollection(fs);
                var mocks = collection.Get();
                List<MockModel> result = new List<MockModel>();
                foreach (var mock in mocks)
                {
                    var m = mock.Get();
                    var executor = new MockExecutor(m);
                    result = executor.PerformAction();
                }
                return JsonSuccess(result);
            }
        }
    }
}