using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Project.Core.Extension;
using Project.Data;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using Project.Models.AccountModel;
using Project.Models.AdminModel;
using Project.Models.CommonModel;
using Project.Models.Master;
using Project.Models.ProfileModel;

namespace Project.Services.Mapping
{
    public class ViewModelToDatabaseMappingProfile : Profile
    {
        public ViewModelToDatabaseMappingProfile()
        {
            //CreateMap<PaymentResult, PaymentResultModel>().ReverseMap();
            CreateMap<RegisterViewModel, UserRegister>().ReverseMap();
            CreateMap<UserProfile, UserRegister>().ReverseMap();
            CreateMap<UserProfileResDTO, DerivedIdentityUser>().ReverseMap();
            CreateMap<UserProfileResDTO, UserProfile>().ReverseMap();
            CreateMap<UserProfileResDTO, UserProfileViewModel>().ReverseMap();
            CreateMap<EmailLogViewModel, EmailLog>().ReverseMap();
            CreateMap<UserListResult, UserViewModel>().ReverseMap();
            CreateMap<UserProfileViewModel, UserListResult>().ReverseMap();
            CreateMap<SearchUserModel, UserProfile>().ReverseMap();
            CreateMap<SearchUserModel, UserResult>().ReverseMap();
            CreateMap<FAQViewModel, FAQ>().ReverseMap();
            CreateMap<DerivedIdentityUser, UserViewModel>().ReverseMap();
            CreateMap<DerivedIdentityUser, ProfileViewModel>().ReverseMap();
            CreateMap<UserViewModel, UserProfile>().ReverseMap();
            CreateMap<UserRegister, UserViewModel>().ReverseMap();
            CreateMap<UserRegister, UserProfileResDTO>().ReverseMap();

            CreateMap<RegisterCustomerReqDTO, RegisterViewModel>().ReverseMap();
            CreateMap<RegisterViewModel, DerivedIdentityUser>().ReverseMap();
            CreateMap<UserProfile, UserProfileViewModel>().ReverseMap();
            CreateMap<RegisterCustomerResDTO, UserRegister>().ReverseMap();
            CreateMap<RegisterCustomerResDTO, RegisterViewModel>().ReverseMap();
           
            CreateMap<UserProfileViewModel, ProfileViewModel>().ReverseMap();
            CreateMap<DerivedIdentityUser, UserHistory>().ReverseMap();
           
            CreateMap<UserProfile, UserHistory>()
                 .ForMember(dest => dest.ProfleUserID, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<DerivedIdentityUser, UserHistory>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();



            CreateMap<UserRegister, UserHistoryViewModel>().ReverseMap();
            CreateMap<UserRegister, DerivedIdentityUser>().ReverseMap();
            CreateMap<UserHistoryViewModel, UserHistory>().ReverseMap();


         
            
             
            CreateMap<UserHistoryDetailsViewModel, UserHistoryLogViewModel>().ReverseMap();
            CreateMap<DerivedIdentityUser, RegisterViewUserModel>().ReverseMap();
            CreateMap<IntegrationViewModel, Integration>().ReverseMap();

        }
    }
}
