namespace DataAccess
{
    public class ClientIdentity
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        /// <summary>
        /// Is Set Videos
        /// </summary>
        /// <returns></returns>
        public bool IsActive { get; set; }
        /// <summary>
        /// Is Online
        /// </summary>
        /// <returns></returns>
        public bool IsOnline { get; set; }
    }
}