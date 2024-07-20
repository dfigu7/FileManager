using AutoMapper;
using BLL.DTO;


namespace FMAPI;
using DataAccess.Entities;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping configurations for File 
        CreateMap<FileItem, FileModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
            .ForMember(dest => dest.FolderId, opt => opt.MapFrom(src => src.FolderId));

      
        // Mapping configurations for Folder 
        CreateMap<Folder, FolderModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId)).ReverseMap();

    }
}