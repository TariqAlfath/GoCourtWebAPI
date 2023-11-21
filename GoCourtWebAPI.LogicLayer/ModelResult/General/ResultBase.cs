using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoCourtWebAPI.LogicLayer.ModelResult.General
{
    public class ResultBase<T>
    {
        public ResultBase()
        {
            ResultCode = "1000";
            ResultMessage = "Success";
            ResultCountData = 0;
        }

        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public int? ResultCountData { get; set; }
        public T? Data { get; set; }

    }

    public class ResultBasePaginated<T> : ResultBase<T>
    {
        public Paginated? Pagination { get; set; }

        public class Paginated
        {
            public int Page { get; set; }
            public int Size { get; set; }
            public long Total { get; set; }
            public int TotalPage { get; set; }
        }
    }

    public static class ResultBaseExtension
    {
        public static ActionResult GenerateActionResult<T>(this ResultBase<T> result)
        {
            if (result.ResultCode is "1000" or "200")
            {
                return new OkObjectResult(result);
            }

            if (result.ResultCode is "1001" or "201")
            {
                return new ObjectResult(result) { StatusCode = 201 };
            }

            if (result.ResultCode is "2002" or "404")
            {
                return new NotFoundObjectResult(result);
            }

            if (result.ResultCode is "4000" or "400")
            {
                return new BadRequestObjectResult(result);
            }

            if (result.ResultCode is "4001" or "401")
            {
                return new UnauthorizedObjectResult(result);
            }

            if (result.ResultCode is "9999" or "500")
            {
                return new ObjectResult(result) { StatusCode = 500 };
            }

            return new ObjectResult(result) { StatusCode = 500 };
        }
    }
}
