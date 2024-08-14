using AutoMapper;
using DataAccess.DTO;



namespace FMAPI;
using DataAccess.Entities;
using System.Security.Cryptography;
using System.Text;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<FileModel, FileItem>().ReverseMap();
        // Mapping configurations for File 
        CreateMap<FileItem, FileModel>()
         
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            
            .ForMember(dest => dest.FolderId, opt => opt.MapFrom(src => src.FolderId));

      
        // Mapping configurations for Folder 
        CreateMap<Folder, FolderModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId)).ReverseMap();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                using (var hmac = new HMACSHA512(src.PasswordSalt))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dest.Password));
                    dest.Password = Convert.ToBase64String(computedHash);
                }
            });

        CreateMap<UserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                using (var hmac = new HMACSHA512())
                {
                    dest.PasswordSalt = hmac.Key;
                    dest.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(src.Password));
                }
            });

       


    }
}