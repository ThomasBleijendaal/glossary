namespace Repository
{
    public class EmployeeEntity : BaseEntity
    {
        public string? Name { get; set; }

        public CompanyEntity? Company { get; set; }

        public int? CompanyId { get; set; }
    }
}
