using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asp_Web_Lib.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Category> Subcategories { get; set; }
        [Index]
        public int? ParentCategoryId { get; set; }
        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; }

        public Category()
        {
            Books = new HashSet<Book>();
            Subcategories = new HashSet<Category>();
        }
        public bool HasCircularReference(Category parent)
        {
            var current = parent;
            while (current != null)
            {
                if (current.Id == this.Id)
                    return true;
                current = current.ParentCategory;
            }
            return false;
        }

        public void AddSubcategory(Category childCategory)
        {
            if (!HasCircularReference(childCategory) && !Subcategories.Contains(childCategory))
            {
                Subcategories.Add(childCategory);
                childCategory.ParentCategory = this;
            }
        }

        public string GetFullPath => ParentCategory == null ? Name : $"{ParentCategory.GetFullPath} > {Name}";


    }
}