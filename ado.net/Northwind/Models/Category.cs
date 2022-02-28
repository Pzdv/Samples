
namespace Northwind.Models
{
    public class Category
    {
        private byte[] picture;
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture
        {
            get
            {
                return picture;
            }
            set
            {
                var normalPicture = new byte[value.Length - 78];
                for (var i = 78; i < value.Length; i++)
                {
                    normalPicture[i - 78] = value[i];
                }
                picture = normalPicture;
            }
        }
    }
}
