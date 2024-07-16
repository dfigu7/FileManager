using AutoMapper;
using BLL.Models;
using DataAccess.Entities;

namespace FMAPI;
using DataAccess.Entities;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapping configurations for File entity and FileModel
        CreateMap<FileItem, FileModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
            .ForMember(dest => dest.FolderId, opt => opt.MapFrom(src => src.FolderId));

        CreateMap<FileModel, FileItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Path, opt => opt.MapFrom(src => src.Path))
            .ForMember(dest => dest.FolderId, opt => opt.MapFrom(src => src.FolderId));

        // Mapping configurations for Folder entity and FolderModel
        CreateMap<Folder, FolderModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ParentFolderId, opt => opt.MapFrom(src => src.ParentFolderId)).ReverseMap();

    }
}