namespace AsmPj.Models;

public class PermissionMenu
{
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }

    public int MenuId { get; set; }
    public Menu Menu { get; set; }
}