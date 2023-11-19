using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared;

public class SubForm
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    public string Name { get; set; }

    public SubForm(User user, string name)
    {
        this.User = user;
        this.Name = name;
    }
    
    private SubForm(){}
}