using System.Net;

namespace CIPLOK_SI_BE.Models
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }

        // Pagination properties added for non-generic ResponseModel
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public int? TotalData { get; set; }
        public int? TotalPages { get; set; }

    }

    public class ResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalData { get; set; }
        public int TotalPages { get; set; }

        public string? SortBy { get; set; }
        public string? Order { get; set; }

        public ResponseModel(HttpStatusCode statusCode = HttpStatusCode.OK, string status = "success", string message = "", T? data = default,
                             int pageNumber = 1, int pageSize = 10, int totalData = 0, string? sortBy = null, string? order = null)
        {
            StatusCode = statusCode;
            Status = status;
            Message = message;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalData = totalData;
            TotalPages = (int)System.Math.Ceiling((double)totalData / pageSize);
            SortBy = sortBy;
            Order = order;
        }
    }
}
