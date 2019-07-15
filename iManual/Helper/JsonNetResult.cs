using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iManual.Helper
{
    public class JsonNetResult : JsonResult
    {
        //return new JsonNetResult() { Data = new { ok = true, data = Obj } };
        public object Data { get; set; }

        public JsonNetResult()
        {
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";
            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting.Indented };
                JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings());
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}