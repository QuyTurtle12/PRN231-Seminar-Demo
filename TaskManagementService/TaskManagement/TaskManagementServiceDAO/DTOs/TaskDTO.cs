namespace TaskManagementServiceDAO.DTOs
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Category { get; set; }
        public List<string>? AssignedUserNames { get; set; }
    }
}
