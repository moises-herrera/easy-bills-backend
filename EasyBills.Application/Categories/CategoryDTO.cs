using EasyBills.Domain.Entities;

namespace EasyBills.Application.Categories;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Transaction> Transactions { get; set; }
}
