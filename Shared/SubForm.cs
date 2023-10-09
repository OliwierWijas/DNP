namespace Shared;

public class SubForm
{
    public int Id { get; set; }
    public User User { get; }
    public string Name { get; set; }

    public SubForm(User user, string name)
    {
        this.User = user;
        this.Name = name;
    }
}