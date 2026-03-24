namespace PulpaAPI.src.Shared.DTOs
{
    public class GetAllDTO
    {
        public GetAllDTO(IQueryCollection queries)
        {
            foreach (var query in queries)
                QueryParams.Add(query.Key, query.Value!);
        }
        public Dictionary<string, string> QueryParams { get; set; } = [];
    }

    public class RequestDTO
    {
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public string DeletedBy { get; set; } = string.Empty;
    }

    public class DeleteDTO : RequestDTO
    {
        public string Id { get; set; } = string.Empty;
    }
}
