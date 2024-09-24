﻿using InventoryManagementSystem.Data.Entities.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace InventoryManagementSystem.Data.Entities
{
    public class Product : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Stock level is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock level must be a non-negative integer.")]
        public int StockLevel { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
        [Display(Name = "Photo")]
        public string? ImageUrl { get; set; }
        public virtual Category Category { get; set; }
    }
}
