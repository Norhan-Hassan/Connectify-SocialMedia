using SocialMedia.Helpers;
using System.Text.Json;

namespace SocialMedia.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPagenationHeader(this HttpResponse httpResponse,
                                                int currentPage,
                                                int itemsPerPage,
                                                int totalItems,
                                                int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            httpResponse.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
