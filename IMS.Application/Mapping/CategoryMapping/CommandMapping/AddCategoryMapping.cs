﻿using IMS.Application.DTOs.CategoryDTOs;
using IMS.Domain.Entities;

namespace IMS.Application.Mapping.CategoryMapping
{
    public partial class CategoryProfile
    {
        public void AddCategoryMapping()
        {
            CreateMap<AddCategoryDTO, Category>();
        }
    }
}
