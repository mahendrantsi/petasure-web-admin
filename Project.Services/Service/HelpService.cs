using AutoMapper;
using SmartPay.Core.Extension;
using SmartPay.Data.DBEntities;
using SmartPay.Models.CommonModel;
using SmartPay.Persistence.UOW;
using SmartPay.Services.IService;
using SmartPay.Services.ServiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPay.Services.Service
{
    public class HelpService : BaseService, IHelpService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HelpService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> SaveUserMessage(EnquiryViewModel dto)
        {
            ServiceResponse<String> objReturn = new ServiceResponse<String>();
            try
            {
                var userMessage = this._mapper.Map<EnquiryViewModel, Enquiry>(dto);
                userMessage.CreatedOn = DateTime.Now;
                this._unitOfWork.UserMessageRepository.Add(userMessage);
                if (await this._unitOfWork.SaveChangesAsync())
                {
                    objReturn = this.SetResultStatus<String>(null, "Submitted successfully.", true);
                }
                else
                {
                    objReturn = this.SetResultStatus<String>(null, "Unknown error occured!", true);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<String>(null, "Unknown error occured!", false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<List<EnquiryListResult>>> GetEnquires(JQueryDataTableModel param)
        {
            ServiceResponse<List<EnquiryListResult>> objReturn = new ServiceResponse<List<EnquiryListResult>>();
            List<EnquiryListResult> enquiries = new List<EnquiryListResult>();
            try
            {
                var propertyInfo = typeof(EnquiryListResult).GetProperty(param.ordercolumn);
                enquiries = this._unitOfWork.UserMessageRepository.GetEnquiries();
                if (enquiries.Count > 0)
                {
                    objReturn = this.SetResultStatus<List<EnquiryListResult>>(enquiries, MessageStatus.Success, true);
                    (objReturn.Data, objReturn.recordsTotal, objReturn.recordsFiltered) = DataTableShorting<EnquiryListResult>(enquiries, param, propertyInfo);
                }
            }
            catch (Exception ex)
            {
                objReturn = this.SetResultStatus<List<EnquiryListResult>>(null, MessageStatus.Error, false);
            }

            return objReturn;
        }

        public async Task<ServiceResponse<ContentMaster>> getContent(string ContentType)
        {
            ServiceResponse<ContentMaster> objReturn = new ServiceResponse<ContentMaster>();
            try
            {
                var content = this._unitOfWork.ContentRepository.Get(x => x.ContentType == ContentType).FirstOrDefault();
                if (content != null)
                {
                    objReturn = this.SetResultStatus<ContentMaster>(content, "Operation Successfully Completed.", true);
                }
                else
                {
                    objReturn = this.SetResultStatus<ContentMaster>(null, "Content not found.", true);
                }
            }
            catch
            {
                objReturn = this.SetResultStatus<ContentMaster>(null, "Unknown error occured!", false);
            }

            return objReturn;
        }
    }
}
