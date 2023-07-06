using System.Text.Json.Serialization;

namespace Slayer.DTOS
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class CommercialProduct
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("technical-products")]
        public object TechnicalProducts { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("packageName")]
        public string PackageName { get; set; }

        [JsonPropertyName("catalogCacheModelPmidId")]
        public string CatalogCacheModelPmidId { get; set; }

        [JsonPropertyName("packageId")]
        public string PackageId { get; set; }

        [JsonPropertyName("businessId")]
        public string BusinessId { get; set; }

        [JsonPropertyName("commercialCode")]
        public string CommercialCode { get; set; }

        [JsonPropertyName("commercialCodePos")]
        public string CommercialCodePos { get; set; }

        [JsonPropertyName("segmentId")]
        public string SegmentId { get; set; }

        [JsonPropertyName("segmentName")]
        public string SegmentName { get; set; }

        [JsonPropertyName("tmCode")]
        public object TmCode { get; set; }

        [JsonPropertyName("primaryOffering")]
        public string PrimaryOffering { get; set; }

        [JsonPropertyName("supplementaryOffering")]
        public object SupplementaryOffering { get; set; }

        [JsonPropertyName("technical-codes")]
        public object? TechnicalCodes { get; set; }

        [JsonPropertyName("commercial-product")]
        public List<CommercialProduct> CommercialProduct { get; set; }
    }


}


