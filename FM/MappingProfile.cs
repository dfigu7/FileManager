using AutoMapper;
using BLL.DTO;


namespace FMAPI;
using DataAccess.Entities;
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

    }
}