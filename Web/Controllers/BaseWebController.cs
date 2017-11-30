using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Web.Controllers
{
    public class BaseWebController : Controller
    {
        public class PlatformJsonResult : JsonResult
        {
            public PlatformJsonResult(object data, bool convertToLowerCamelCase = false)
            {
                this.convertToLowerCamelCase = convertToLowerCamelCase;
                this.Data = data;
                this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";

                if (this.ContentEncoding != null)
                    response.ContentEncoding = this.ContentEncoding;

                if (this.Data == null)
                {
                    return;
                }
                var result = JsonFormatter.Encode(this.Data, this.convertToLowerCamelCase); //buffer output to string for correct serialization exception processing
                response.Write(result);
            }

            private readonly bool convertToLowerCamelCase;
        }

        /// <summary>
        /// Returns JsonResult for faled Ajax request with exception name additional data 
        /// </summary>
        /// <param name="exceptionCode"></param>
        /// <returns></returns>
        public JsonResult JsonFail(string exceptionCode)
        {
            var data = JsonResponseFormat.GetException(exceptionCode);
            return new PlatformJsonResult(data);
        }

        /// <summary>
        /// Returns JsonResult for successful Ajax request with no additional data
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonSuccess()
        {
            var data = JsonResponseFormat.GetSuccess();
            return new PlatformJsonResult(data);
        }


        public PlatformJsonResult JsonSuccess(object result)
        {
            var data = JsonResponseFormat.GetSuccess(result);
            return new PlatformJsonResult(data, true);
        }


        public static class JsonFormatter
        {
            private static readonly JsonSerializer LowerCamelCaseSerializer =
                JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        Converters =
                        {
                            new StringEnumConverter(),
                        },
                        NullValueHandling = NullValueHandling.Ignore
                    });

            private static readonly JsonSerializer RegularSerializer =
                JsonSerializer.Create(
                    new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        Converters =
                        {
                            new IsoDateTimeConverter(),
                            new StringEnumConverter()
                        },
                        NullValueHandling = NullValueHandling.Ignore
                    });

            public static string Encode(object value, bool convertToLowerCamelCase = false)
            {
                var serializer = convertToLowerCamelCase
                    ? LowerCamelCaseSerializer
                    : RegularSerializer;
                using (var str = new StringWriter())
                {
                    using (var writer = new JsonTextWriter(str))
                    {
                        serializer.Serialize(writer, value);
                        writer.Flush();
                    }
                    return str.ToString();
                }
            }

            public static void EncodeToStream(object value, Stream stream, bool convertToLowerCamelCase = false)
            {
                var serializer = convertToLowerCamelCase
                    ? LowerCamelCaseSerializer
                    : RegularSerializer;
                using (var str = new StreamWriter(stream))
                {
                    using (var writer = new JsonTextWriter(str))
                    {
                        serializer.Serialize(writer, value);
                        writer.Flush();
                    }
                }
            }
        }

        public static class JsonResponseFormat
        {

            /**
             *<summary>
             *Gets object for json exception responce
             *</summary>
            */
            public static object GetException(string exceptionCode)
            {
                return new
                {
                    success = false,
                    exceptionCode = exceptionCode

                };
            }

            public static object GetException(string exceptionCode, string message)
            {
                return new
                {
                    success = false,
                    exceptionCode = exceptionCode,
                    message = message
                };
            }
            /**
             *<summary>
             *Gets object for json exception responce with additional data and extraData attributes
             *</summary>
            */
            public static object GetException(string exceptionCode, string message, System.Collections.IDictionary data, object extraData)
            {
                return new
                {
                    success = false,
                    exceptionCode = exceptionCode,
                    message = message,
                    data = data,
                    extraData = extraData
                };
            }


            /**
            *<summary>
            *Gets object for json success responce
            *</summary>
           */
            public static object GetSuccess()
            {
                return new { success = true };
            }

            public static object GetRefresh()
            {
                return new { refresh = true, success = true };
            }

            /**
            *<summary>
            *Gets object for json success responce with additional data
            *</summary>
            */
            public static object GetSuccess(object data)
            {
                return new { success = true, data = data };
            }


        }
    }
}