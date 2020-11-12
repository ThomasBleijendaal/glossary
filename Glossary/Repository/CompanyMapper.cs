namespace Repository
{
    public class CompanyMapper : IMapper<Company, BasicCompanyModel>
    {
        public Company Map(BasicCompanyModel model)
        {
            return new Company
            {
                Id = model.Id,
                Name = model.Name,
                ParentCompanyId = model.ParentCompanyId
            };
        }

        public Company Apply(Company orignalEntity, Company newEntity)
        {
            orignalEntity.Name = newEntity.Name;
            orignalEntity.ParentCompanyId = newEntity.ParentCompanyId;
            return orignalEntity;
        }

        public BasicCompanyModel Map(Company entity)
        {
            return new BasicCompanyModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ParentCompanyId = entity.ParentCompanyId
            };
        }
    }
}
