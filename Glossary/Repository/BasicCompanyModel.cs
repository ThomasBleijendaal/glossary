namespace Repository
{
    public class BasicCompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentCompanyId { get; set; }
    }
}
