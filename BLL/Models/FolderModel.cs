﻿// FileManager.BLL/Models/FolderModel.cs

namespace BLL.Models;

public class FolderModel
{
    public string Name { get; set; }
    public int? ParentFolderId { get; set; }
}