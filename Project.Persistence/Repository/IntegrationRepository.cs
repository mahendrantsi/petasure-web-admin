using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Project.Core.Enum;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.CommonModel;
using Project.Models.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public class IntegrationRepository : GenericRepository<Integration>
    {
        private readonly ProjectDbContext _dbContext;
        private readonly IMapper _mapper;
        public IntegrationRepository(ProjectDbContext ProjectDbContext, IMapper mapper) : base(ProjectDbContext)
        {
            this._dbContext = ProjectDbContext;
            this._mapper = mapper;
        }
        public virtual IEnumerable<IntegrationViewModel> Get(
            Expression<Func<Integration, bool>> filter = null,
            Func<IQueryable<Integration>, IOrderedQueryable<Integration>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Integration> query = Entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).Select(x => new IntegrationViewModel 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    IsActive = x.IsActive,
                    Status = x.Status
                }).ToList();
            }
            else
            {
                return query.Select(x => new IntegrationViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Image = x.Image,
                    IsActive = x.IsActive,
                    Status = x.Status
                }).ToList();
            }
        }


        public async Task<IntegrationViewModel> CreateIntegration(IntegrationViewModel model)
        {
            try
            {
                var integration = await this._dbContext.Set<Integration>().AddAsync(new Integration
                {
                    Name = model.Name,
                    Description = model.Description,
                    Image = model.Image,
                    IsActive = model.IsActive,
                    Status = model.Status
                });
                await this._dbContext.SaveChangesAsync();
                model.Id = integration.Entity.Id;
                model.Success = true;
                model.Message =  MessageStatus.Success;
                return model;
            }
            catch (Exception ex)
            {
                model.Success = false;
                model.Message = MessageStatus.Error;
                return model;
            }
            return model;
        }
        public async Task<Integration> GetIntegrationByID(Guid id)
        {
            return await this._dbContext.Set<Integration>().Where(x => x.IsActive).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IntegrationViewModel> UpdateIntegration(IntegrationViewModel model) 
        {
            try
            {
                var integration = await GetIntegrationByID(model.Id);
                if (integration != null)
                {
                    integration = this._mapper.Map<Integration>(model);
                    var response = this._dbContext.Set<Integration>().Update(integration);
                    await this._dbContext.SaveChangesAsync();
                    model.Success = true;
                    model.Message = MessageStatus.Success;
                    return this._mapper.Map<IntegrationViewModel>(response.Entity);
                }
                else
                {
                    model.Success = false;
                    model.Message = MessageStatus.NotFound;
                    return model;
                }
            }
            catch (Exception ex)
            {
                model.Success = false;
                model.Message = MessageStatus.Error;
                return model;
            }
        }
    }
}
