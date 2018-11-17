namespace LoginMVC.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public string UserName { get; set; }

        public Role Role { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }
    }

    public enum Role
    {
        Employee,
        Admin
    }
}