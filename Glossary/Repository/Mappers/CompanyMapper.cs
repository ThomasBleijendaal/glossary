namespace Repository
{
    public class CompanyMapper : IMapper<CompanyEntity, BasicCompanyModel>
    {
        public CompanyEntity Map(BasicCompanyModel model)
        {
            return new CompanyEntity
            {
                Id = model.Id,
                Name = model.Name,
                ParentCompanyId = model.ParentCompanyId
            };
        }

        public CompanyEntity Apply(CompanyEntity orignalEntity, CompanyEntity newEntity)
        {
            orignalEntity.Name = newEntity.Name;
            orignalEntity.ParentCompanyId = newEntity.ParentCompanyId;
            return orignalEntity;
        }

        public BasicCompanyModel Map(CompanyEntity entity)
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
